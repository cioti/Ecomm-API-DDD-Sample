using MediatR;
using System;

namespace Ecomm.Application.Commands
{
    public class CreateShoppingCartCommand : IRequest<Guid> { }
}
