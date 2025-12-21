using BeerStore.Application.DTOs.Auth.User.Requests;
using BeerStore.Application.DTOs.Auth.User.Requests.Login;
using BeerStore.Application.DTOs.Auth.User.Responses.Login;
using BeerStore.Application.DTOs.Auth.User.Responses.Register;
using BeerStore.Application.Modules.Auth.User.Commands.Login;
using BeerStore.Application.Modules.Auth.User.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Post: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login
            ([FromBody] LoginRequest request,
            CancellationToken token)
        {
            var result = await _mediator.Send(new LoginCommand(request), token);
            return Ok(result);
        }

        // Post: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register
            ([FromBody] CreateUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new RegisterCommand(request), token);
            return Ok(result);
        }
    }
}