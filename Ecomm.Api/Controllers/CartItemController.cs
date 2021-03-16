using Ecomm.Application.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Api.Controllers
{
    [Route("api/v1/cart/{cartId:guid}/items")]
    public class CartItemController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AddCartItemAsync(Guid cartId, AddCartItemCommand command, CancellationToken cancellationToken)
        {
            command.CartId = cartId;
            await Mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{productId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RemoveCartItemAsync(Guid cartId, int productId, CancellationToken cancellationToken)
        {
            var command = new RemoveCartItemCommand
            {
                CartId = cartId,
                ProductId = productId
            };
            await Mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
