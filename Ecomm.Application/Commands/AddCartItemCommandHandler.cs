using Ecomm.Application.Common.Interfaces;
using Ecomm.Domain.Abstractions;
using Ecomm.Domain.Entities;
using Ecomm.Domain.Specifications;
using Ecomm.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Application.Commands
{
    public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, int>
    {
        private readonly IGenericAsyncRepository<ShoppingCart> _cartRepository;
        private readonly IProductProxy _productProxy;

        public AddCartItemCommandHandler(IGenericAsyncRepository<ShoppingCart> cartRepository,
            IProductProxy productProxy)
        {
            _cartRepository = cartRepository;
            _productProxy = productProxy;
        }

        public async Task<int> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var productData = await _productProxy.GetProductWithDiscountsAsync(request.ProductId, cancellationToken);

            ShoppingCart cart = await _cartRepository.GetBySpecAsync(new ShoppingCartWithItemsAndDiscountsSpec(request.CartId), cancellationToken);

            if (cart == null)
            {
                throw new NotFoundException(nameof(ShoppingCart));
            }

            List<CartItemDiscount> discounts = null;
            if (productData.Discounts != null)
            {
                discounts = productData.Discounts
                    .Select(d => new CartItemDiscount(d.Code, d.Percentage))
                    .ToList();
            }

            var itemId = cart.AddItem(productData.Id,
                productData.Name,
                request.Quantity,
                productData.Price,
                productData.Currency,
                productData.ImageBytes,
                discounts);

            await _cartRepository.UpdateAsync(cart, cancellationToken);
            return itemId;
        }
    }
}
