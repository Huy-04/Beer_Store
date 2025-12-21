using BeerStore.Application.DTOs.Auth.Role.Requests;
using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Modules.Auth.Role.Commands.CreateRole;
using BeerStore.Application.Modules.Auth.Role.Commands.RemoveRole;
using BeerStore.Application.Modules.Auth.Role.Commands.UpdateRole;
using BeerStore.Application.Modules.Auth.Role.Queries.GetAllRole;
using BeerStore.Application.Modules.Auth.Role.Queries.GetRoleById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : BaseApiController
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllRoleQuery(), token);
            return Ok(result);
        }

        // Get: api/Role/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoleResponse>> GetById([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetRoleByIdQuery(id), token);
            return Ok(result);
        }

        // Post: api/Role
        [HttpPost]
        public async Task<ActionResult<RoleResponse>> Create(
            [FromBody] RoleRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateRoleCommand(CurrentUserId, CurrentUserId, request), token);
            return Ok(result);
        }

        // Put: api/Role/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RoleResponse>> Update(
            [FromRoute] Guid id, [FromBody] RoleRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateRoleCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        // Delete: api/Role/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveRoleCommand(id), token);
            return NoContent();
        }
    }
}