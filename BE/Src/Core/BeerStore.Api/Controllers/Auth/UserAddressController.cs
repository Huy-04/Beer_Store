using Api.Core;
using BeerStore.Application.DTOs.Auth.UserAddress.Requests;
using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Application.Modules.Auth.UserAddresses.Commands.CreateUserAddress;
using BeerStore.Application.Modules.Auth.UserAddresses.Commands.RemoveUserAddress;
using BeerStore.Application.Modules.Auth.UserAddresses.Commands.UpdateUserAddress;
using BeerStore.Application.Modules.Auth.UserAddresses.Queries.GetUserAddressById;
using BeerStore.Application.Modules.Auth.UserAddresses.Queries.GetUserAddressByPhone;
using BeerStore.Application.Modules.Auth.UserAddresses.Queries.GetAllUserAddress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserAddressController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserAddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/UserAddress
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAddressResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllUserAddressQuery(), token);
            return Ok(result);
        }

        // Get: api/UserAddress/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserAddressResponse>> GetById([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserAddressByIdQuery(id), token);
            return Ok(result);
        }

        // Get: api/UserAddress/phone/{phone}
        [HttpGet("phone/{phone}")]
        public async Task<ActionResult<IEnumerable<UserAddressResponse>>> GetByPhone([FromRoute] string phone, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserAddressByPhoneQuery(phone), token);
            return Ok(result);
        }

        // Post: api/UserAddress
        [HttpPost]
        public async Task<ActionResult<UserAddressResponse>> Create(
            [FromBody] UserAddressRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateUserAddressCommand(CurrentUserId, CurrentUserId, CurrentUserId, request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdUserAddress }, result);
        }

        // Put: api/UserAddress/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserAddressResponse>> Update(
            [FromRoute] Guid id, [FromBody] UserAddressRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateUserAddressCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        // Delete: api/UserAddress/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveUserAddressCommand(id), token);
            return NoContent();
        }
    }
}
