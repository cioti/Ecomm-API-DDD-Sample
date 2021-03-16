using Ecomm.Application.Common.DTOs;
using Ecomm.Domain.Abstractions;
using Ecomm.Domain.Entities;
using Ecomm.Domain.Specifications;
using Ecomm.Shared.Exceptions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Application.Queries.Cart
{
    public class GetShoppingCartByIdQueryHandler : IRequestHandler<GetShoppingCartByIdQuery, ShoppingCartDto>
    {
        private readonly IGenericAsyncRepository<ShoppingCart> _cartRepository;

        public GetShoppingCartByIdQueryHandler(IGenericAsyncRepository<ShoppingCart> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<ShoppingCartDto> Handle(GetShoppingCartByIdQuery request, CancellationToken cancellationToken)
        {
            ShoppingCart cart = await _cartRepository.GetBySpecAsync(new ShoppingCartWithItemsAndDiscountsSpec(request.Id), cancellationToken);
            if (cart == null)
            {
                throw new NotFoundException(nameof(ShoppingCart));
            }

            //of course for better performance the select should be evaluated in the database and not here
            //maybe use automapper with projection 
            //for simplicity i left it like this
            return new ShoppingCartDto
            {
                CustomerId = cart.CustomerId,
                CartTotalPrice = cart.TotalPrice,
                CartTotalPriceWithDiscount = cart.CalculateTotalPriceWithDiscount(),
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Currency = i.UnitPrice.Currency.ToString(),
                    UnitPrice = i.UnitPrice.Value,
                    TotalPrice = i.TotalPrice,
                    TotalPriceWithDiscount = i.CalculateTotalPriceWithDiscount(),
                    Description = i.Description,
                    Image = i.ImageBytes,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
