using MediatR;
using System;

namespace Ecomm.Application.Commands
{
    public class RemoveCartItemCommand : IRequest
    {
        public Guid CartId { get; set; }
        public int ProductId { get; set; }
    }
}
