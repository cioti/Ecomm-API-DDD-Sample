using Ecomm.Domain.Abstractions;
using Ecomm.Domain.Entities;
using Ecomm.Domain.Specifications;
using Ecomm.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Application.Commands
{
    public class RemoveCartItemCommandHandler : AsyncRequestHandler<RemoveCartItemCommand>
    {
        private readonly IGenericAsyncRepository<ShoppingCart> _cartRepository;

        public RemoveCartItemCommandHandler(IGenericAsyncRepository<ShoppingCart> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        protected override async Task Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            ShoppingCart cart = await _cartRepository.GetBySpecAsync(new ShoppingCartByIdWithItemsSpec(request.CartId), cancellationToken);
            if (cart == null)
            {
                throw new NotFoundException(nameof(ShoppingCart));
            }

            cart.RemoveItem(request.ProductId);
            await _cartRepository.UpdateAsync(cart, cancellationToken);
        }
    }
}
