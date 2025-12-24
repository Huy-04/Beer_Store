using BeerStore.Application.DTOs.Auth.Junction.UserRole.Requests;
using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using BeerStore.Application.Modules.Auth.Junction.UserRole.Commands.AddUserRole;
using BeerStore.Application.Modules.Auth.Junction.UserRole.Commands.RemoveUserRole;
using BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetAllUserRoles;
using BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetUserRolesByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserRoleController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/UserRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllUserRolesQuery(), token);
            return Ok(result);
        }

        // Get: api/UserRole/user/{userId}
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetByUserId(
            [FromRoute] Guid userId, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserRolesByUserIdQuery(userId), token);
            return Ok(result);
        }

        // Post: api/UserRole
        [HttpPost]
        public async Task<ActionResult<UserRoleResponse>> Add(
            [FromBody] AddUserRoleRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new AddUserRoleCommand(request.UserId, request.RoleId), token);
            return CreatedAtAction(nameof(GetByUserId), new { userId = request.UserId }, result);
        }

        // Delete: api/UserRole/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveUserRoleCommand(id), token);
            return NoContent();
        }
    }
}
