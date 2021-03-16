using Ecomm.Application.Common.DTOs;
using MediatR;
using System;

namespace Ecomm.Application.Queries.Cart
{
    public class GetShoppingCartByIdQuery : IRequest<ShoppingCartDto>
    {
        public Guid Id { get; set; }
    }
}
