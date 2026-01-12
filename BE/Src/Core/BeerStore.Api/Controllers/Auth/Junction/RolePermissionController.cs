using Api.Core;
using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Requests;
using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using BeerStore.Application.Modules.Auth.Junction.RolePermission.Commands.AddRolePermission;
using BeerStore.Application.Modules.Auth.Junction.RolePermission.Commands.RemoveRolePermission;
using BeerStore.Application.Modules.Auth.Junction.RolePermission.Queries.GetAllRolePermissions;
using BeerStore.Application.Modules.Auth.Junction.RolePermission.Queries.GetPermissionsByRoleId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth.Junction
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolePermissionController : BaseApiController
    {
        private readonly IMediator _mediator;

        public RolePermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/RolePermission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolePermissionResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllRolePermissionsQuery(), token);
            return Ok(result);
        }

        // Get: api/RolePermission/role/{roleId}
        [HttpGet("role/{roleId:guid}")]
        public async Task<ActionResult<IEnumerable<RolePermissionResponse>>> GetByRoleId(
            [FromRoute] Guid roleId, CancellationToken token)
        {
            var result = await _mediator.Send(new GetPermissionsByRoleIdQuery(roleId), token);
            return Ok(result);
        }

        // Post: api/RolePermission
        [HttpPost]
        public async Task<ActionResult<RolePermissionResponse>> Add(
            [FromBody] AddRolePermissionRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new AddRolePermissionCommand(request.RoleId, request.PermissionId), token);
            return CreatedAtAction(nameof(GetByRoleId), new { roleId = request.RoleId }, result);
        }

        // Delete: api/RolePermission/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remove([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveRolePermissionCommand(id), token);
            return NoContent();
        }
    }
}
