using Ecomm.Application.Common.Interfaces;
using Ecomm.Domain.Abstractions;
using Ecomm.Domain.Entities;
using Ecomm.Domain.Specifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Application.Commands
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, Guid>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenericAsyncRepository<ShoppingCart> _cartRepository;
        private readonly ICustomerProxy _customerProxy;
        private readonly IDatetimeService _datetimeService;

        public CreateShoppingCartCommandHandler(ICurrentUserService currentUserService,
            IGenericAsyncRepository<ShoppingCart> cartRepository,
            ICustomerProxy customerProxy,
            IDatetimeService datetimeService)
        {
            _currentUserService = currentUserService;
            _cartRepository = cartRepository;
            _customerProxy = customerProxy;
            _datetimeService = datetimeService;
        }

        public async Task<Guid> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var customerData = await _customerProxy.GetCustomerWithDiscountsAsync(_currentUserService.UserId);

            ShoppingCart cart = await _cartRepository.GetBySpecAsync(new LastShoppingCartByCustomerIdSpec(customerData.Id), cancellationToken);

            //Normally I would argue that any shopping cart persistance should be done on the client
            //But for the sake of this exercise I added a 24 hour constraint so we don't allow creation of too many shopping carts
            //This could also be a one to one... so we can make this an Upsert command: insert only if it is null, else update
            if (cart is null || IsOlderThan24Hours(cart))
            {
                cart = new ShoppingCart(customerData.Id);
                var discounts = customerData.Discounts.Select(d => new CartDiscount(d.Code, d.Percentage)).ToList();
                cart.AddDiscounts(discounts);
                await _cartRepository.AddAsync(cart, cancellationToken);
            }

            return cart.Id;
        }

        private bool IsOlderThan24Hours(ShoppingCart cart)
            => (_datetimeService.UtcNow - cart.Audit.CreatedDate).Days >= 1;
    }
}
