using MediatR;
using System;

namespace Ecomm.Application.Commands
{
    public class AddCartItemCommand : IRequest<int>
    {
        public Guid CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
