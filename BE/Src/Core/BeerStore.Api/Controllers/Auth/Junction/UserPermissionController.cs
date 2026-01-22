using Api.Core;
using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Requests;
using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.AddUserPermission;
using BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.UpdateUserPermission;
using BeerStore.Application.Modules.Auth.Junction.UserPermission.Queries.GetUserPermissionsByUserId;
using BeerStore.Application.Modules.Auth.Junction.UserPermission.Queries.GetAllUserPermissions;
using BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.RemoveUserPermission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth.Junction
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserPermissionController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserPermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/UserPermission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPermissionResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllUserPermissionsQuery(), token);
            return Ok(result);
        }

        // Get: api/UserPermission/user/{userId}
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<UserPermissionResponse>>> GetByUserId(
            [FromRoute] Guid userId, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserPermissionsByUserIdQuery(userId), token);
            return Ok(result);
        }

        // Post: api/UserPermission
        [HttpPost]
        public async Task<ActionResult<UserPermissionResponse>> Add(
            [FromBody] AddUserPermissionRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(
                new AddUserPermissionCommand(request.UserId, request.PermissionId, request.Status), token);
            return CreatedAtAction(nameof(GetByUserId), new { userId = request.UserId }, result);
        }

        // Put: api/UserPermission/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserPermissionResponse>> Update(
            [FromRoute] Guid id, [FromBody] UpdateUserPermissionRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateUserPermissionCommand(id, request.Status), token);
            return Ok(result);
        }

        // Delete: api/UserPermission/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveUserPermissionCommand(id), token);
            return NoContent();
        }
    }
}
