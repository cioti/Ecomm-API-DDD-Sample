using Ecomm.Application.Commands;
using Ecomm.Application.Common.DTOs;
using Ecomm.Application.Queries.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Api.Controllers
{
    public class CartController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> CreateAsync(CancellationToken cancellationToken)
        {
            var cartId = await Mediator.Send(new CreateShoppingCartCommand(), cancellationToken);
            return Created($"{Request.Path}/{cartId}", cartId);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ShoppingCartDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetShoppingCartByIdQuery { Id = id }, cancellationToken));
        }
    }
}
