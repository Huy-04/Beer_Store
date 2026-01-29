using Api.Core;
using BeerStore.Application.DTOs.Auth.Authentication.Requests.Login;
using BeerStore.Application.DTOs.Auth.Authentication.Requests.RefreshAccessToken;
using BeerStore.Application.DTOs.Auth.Authentication.Responses.Login;
using BeerStore.Application.DTOs.Auth.Authentication.Responses.Register;
using BeerStore.Application.DTOs.Auth.User.Requests;
using BeerStore.Application.Modules.Auth.Authentication.Command.Login;
using BeerStore.Application.Modules.Auth.Authentication.Command.Logout;
using BeerStore.Application.Modules.Auth.Authentication.Command.RefreshAccessToken;
using BeerStore.Application.Modules.Auth.Authentication.Command.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(
            [FromBody] LoginRequest request,
            [FromHeader(Name = "X-Device-Id")] string deviceId,
            [FromHeader(Name = "X-Device-Name")] string deviceName,
            CancellationToken token)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _mediator.Send(new LoginCommand(deviceId, deviceName, ipAddress, request), token);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(
            [FromBody] CreateUserRequest request,
            CancellationToken token)
        {   
            var result = await _mediator.Send(new RegisterCommand(request), token);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout(
            [FromHeader(Name = "X-Device-Id")] string deviceId,
            CancellationToken token)
        {
            await _mediator.Send(new LogoutCommand(CurrentUserId, deviceId), token);
            return NoContent();
        }

        [HttpPost("refresh-accesstoken")]
        public async Task<ActionResult<LoginResponse>> RefreshToken(
            [FromBody] RefreshAccessTokenRequest request,
            [FromHeader(Name = "X-Device-Id")] string deviceId,
            CancellationToken token)
        {
            var result = await _mediator.Send(new RefreshAccessTokenCommand(request.RefreshToken, deviceId), token);
            return Ok(result);
        }
    }
}