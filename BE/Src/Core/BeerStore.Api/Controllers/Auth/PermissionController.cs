using Api.Core;
using BeerStore.Application.DTOs.Auth.Permission.Requests;
using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Modules.Auth.Permissions.Commands.CreatePermission;
using BeerStore.Application.Modules.Auth.Permissions.Commands.RemovePermission;
using BeerStore.Application.Modules.Auth.Permissions.Commands.UpdatePermission;
using BeerStore.Application.Modules.Auth.Permissions.Queries.GetAllPermission;
using BeerStore.Application.Modules.Auth.Permissions.Queries.GetPermissionById;
using BeerStore.Application.Modules.Auth.Permissions.Queries.GetPermissionByModule;
using BeerStore.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionController : BaseApiController
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/Permission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllPermissionQuery(), token);
            return Ok(result);
        }

        // Get: api/Permission/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PermissionResponse>> GetById([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetPermissionByIdQuery(id), token);
            return Ok(result);
        }

        // Get: api/Permission/module/{module}
        [HttpGet("module/{module}")]
        public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetByModule(
            [FromRoute] ModuleEnum module, CancellationToken token)
        {
            var result = await _mediator.Send(new GetPermissionByModuleQuery(module), token);
            return Ok(result);
        }

        // Post: api/Permission
        [HttpPost]
        public async Task<ActionResult<PermissionResponse>> Create(
            [FromBody] PermissionRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreatePermissionCommand(CurrentUserId, CurrentUserId, request), token);
            return Ok(result);
        }

        // Put: api/Permission/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PermissionResponse>> Update(
            [FromRoute] Guid id, [FromBody] PermissionRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdatePermissionCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        // Delete: api/Permission/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemovePermissionCommand(id), token);
            return NoContent();
        }
    }
}
