using Api.Core;
using BeerStore.Application.DTOs.Shop.StoreAddress.Requests;
using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Commands.CreateStoreAddress;
using BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Commands.RemoveStoreAddress;
using BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Commands.UpdateStoreAddress;
using BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Queries.GetStoreAddressById;
using BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Queries.GetStoreAddressesByStore;
using BeerStore.Domain.Constants.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Shop.Junction
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StoreAddressesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public StoreAddressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries

        // GET: api/StoreAddress/store/{storeId}
        [HttpGet("store/{storeId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StoreAddressResponse>>> GetByStore(
            [FromRoute] Guid storeId, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStoreAddressesByStoreQuery(storeId), token);
            return Ok(result);
        }

        // GET: api/StoreAddress/{id}
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<StoreAddressResponse>> GetById([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStoreAddressByIdQuery(id), token);
            if (result == null) return NotFound();
            return Ok(result);
        }

        #endregion Queries

        #region Commands

        // POST: api/StoreAddress/store/{storeId}
        [HttpPost("store/{storeId:guid}")]
        public async Task<ActionResult<StoreAddressResponse>> Create(
            [FromRoute] Guid storeId,
            [FromBody] CreateStoreAddressRequest request,
            CancellationToken token)
        {
            var result = await _mediator.Send(new CreateStoreAddressCommand(storeId, CurrentUserId, request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/StoreAddress/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<StoreAddressResponse>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateStoreAddressRequest request,
            CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateStoreAddressCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        // DELETE: api/StoreAddress/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveStoreAddressCommand(id, CurrentUserId), token);
            return NoContent();
        }

        #endregion Commands
    }
}
