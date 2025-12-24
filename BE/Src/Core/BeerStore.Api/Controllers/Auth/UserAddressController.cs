using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Requests;
using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using BeerStore.Application.Modules.Auth.Junction.UserAddress.Commands.AddUserAddress;
using BeerStore.Application.Modules.Auth.Junction.UserAddress.Commands.RemoveUserAddress;
using BeerStore.Application.Modules.Auth.Junction.UserAddress.Queries.GetAllUserAddresses;
using BeerStore.Application.Modules.Auth.Junction.UserAddress.Queries.GetUserAddressesByUserId;
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
            var result = await _mediator.Send(new GetAllUserAddressesQuery(), token);
            return Ok(result);
        }

        // Get: api/UserAddress/user/{userId}
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<UserAddressResponse>>> GetByUserId(
            [FromRoute] Guid userId, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserAddressesByUserIdQuery(userId), token);
            return Ok(result);
        }

        // Post: api/UserAddress
        [HttpPost]
        public async Task<ActionResult<UserAddressResponse>> Add(
            [FromBody] AddUserAddressRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new AddUserAddressCommand(request.UserId, request.AddressId), token);
            return CreatedAtAction(nameof(GetByUserId), new { userId = request.UserId }, result);
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
