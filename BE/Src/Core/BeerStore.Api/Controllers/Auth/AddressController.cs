using Api.Core;
using BeerStore.Application.DTOs.Auth.Address.Requests;
using BeerStore.Application.DTOs.Auth.Address.Responses;
using BeerStore.Application.Modules.Auth.Addresses.Commands.CreateAddress;
using BeerStore.Application.Modules.Auth.Addresses.Commands.RemoveAddress;
using BeerStore.Application.Modules.Auth.Addresses.Commands.UpdateAddress;
using BeerStore.Application.Modules.Auth.Addresses.Queries.GetAddressById;
using BeerStore.Application.Modules.Auth.Addresses.Queries.GetAddressByPhone;
using BeerStore.Application.Modules.Auth.Addresses.Queries.GetAllAddress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/Address
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllAddressQuery(), token);
            return Ok(result);
        }

        // Get: api/Address/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AddressResponse>> GetById([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetAddressByIdQuery(id), token);
            return Ok(result);
        }

        // Get: api/Address/phone/{phone}
        [HttpGet("phone/{phone}")]
        public async Task<ActionResult<IEnumerable<AddressResponse>>> GetByPhone([FromRoute] string phone, CancellationToken token)
        {
            var result = await _mediator.Send(new GetAddressByPhoneQuery(phone), token);
            return Ok(result);
        }

        // Post: api/Address
        [HttpPost]
        public async Task<ActionResult<AddressResponse>> Create(
            [FromBody] AddressRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateAddressCommand(CurrentUserId, CurrentUserId, CurrentUserId, request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdAddress }, result);
        }

        // Put: api/Address/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<AddressResponse>> Update(
            [FromRoute] Guid id, [FromBody] AddressRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateAddressCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        // Delete: api/Address/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveAddressCommand(id), token);
            return NoContent();
        }
    }
}
