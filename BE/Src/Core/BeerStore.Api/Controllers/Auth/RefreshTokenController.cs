using Api.Core;
using BeerStore.Application.DTOs.Auth.RefreshToken.Requests;
using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeAllUserRefreshTokens;
using BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeByUserAndDevice;
using BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeRefreshToken;
using BeerStore.Application.Modules.Auth.RefreshTokens.Queries.GetActiveSessionsByUserId;
using BeerStore.Application.Modules.Auth.RefreshTokens.Queries.GetAllActiveSession;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RefreshTokenController : BaseApiController
    {
        private readonly IMediator _mediator;

        public RefreshTokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/RefreshToken
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllActiveSessionQuery(), token);
            return Ok(result);
        }

        // GET: api/RefreshToken/user/{id}
        [HttpGet("user/{id:guid}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetByUserId
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetActiveSessionsByUserIdQuery(id), token);
            return Ok(result);
        }

        // POST: api/RefreshToken/revoke/{tokenRaw}
        [HttpPost("revoke/{tokenRaw}")]
        public async Task<ActionResult> Revoke(
            [FromRoute] string tokenRaw,
            CancellationToken token)
        {
            await _mediator.Send(new RevokeRefreshTokenCommand(CurrentUserId, tokenRaw), token);
            return NoContent();
        }

        // POST: api/RefreshToken/revoke-all/{id}
        [HttpPost("revoke-all/user/{id:guid}")]
        public async Task<ActionResult> RevokeAll(
            [FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RevokeAllUserRefreshTokensCommand(id, CurrentUserId), token);
            return NoContent();
        }

        // POST: api/RefreshToken/revoke-device
        [HttpPost("revoke-device")]
        public async Task<ActionResult> RevokeUserDevice(
            [FromBody] RevokeUserDeviceRequest request,
            CancellationToken token)
        {
            await _mediator.Send(new RevokeByUserAndDeviceCommand(CurrentUserId, request.UserId, request.DeviceId), token);
            return NoContent();
        }
    }
}