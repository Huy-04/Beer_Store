using Api.Core;
using BeerStore.Application.DTOs.Auth.User.Requests;
using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Modules.Auth.Users.Commands.CreateUser;
using BeerStore.Application.Modules.Auth.Users.Commands.RemoveUser;
using BeerStore.Application.Modules.Auth.Users.Commands.UpdateUser;
using BeerStore.Application.Modules.Auth.Users.Queries.GetAllUser;
using BeerStore.Application.Modules.Auth.Users.Queries.GetByUserByEmailStatus;
using BeerStore.Application.Modules.Auth.Users.Queries.GetUserByEmail;
using BeerStore.Application.Modules.Auth.Users.Queries.GetUserById;
using BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhone;
using BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhoneStatus;
using BeerStore.Application.Modules.Auth.Users.Queries.GetUserByUserName;
using BeerStore.Application.Modules.Auth.Users.Queries.GetUserByUserStatus;
using Domain.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllUserQuery(), token);
            return Ok(result);
        }

        // Get: api/User/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetById([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id), token);
            return Ok(result);
        }

        // Get: api/User/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserResponse>> GetByEmail([FromRoute] string email, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserByEmailQuery(email), token);
            return Ok(result);
        }

        // Get: api/User/username/{username}
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserResponse>> GetByUserName([FromRoute] string username, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserByUserNameQuery(username), token);
            return Ok(result);
        }

        // Get: api/User/phone/{phone}
        [HttpGet("phone/{phone}")]
        public async Task<ActionResult<UserResponse>> GetByPhone([FromRoute] string phone, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserByPhoneQuery(phone), token);
            return Ok(result);
        }

        // Get: api/User/email-status/{status}
        [HttpGet("email-status/{status}")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetByEmailStatus(
            [FromRoute] StatusEnum status, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserByEmailStatusQuery(status), token);
            return Ok(result);
        }

        // Get: api/User/phone-status/{status}
        [HttpGet("phone-status/{status}")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetByPhoneStatus(
            [FromRoute] StatusEnum status, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserByPhoneStatusQuery(status), token);
            return Ok(result);
        }

        // Get: api/User/user-status/{status}
        [HttpGet("user-status/{status}")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetByUserStatus(
            [FromRoute] StatusEnum status, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserByUserStatusQuery(status), token);
            return Ok(result);
        }

        // Post: api/User
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(
            [FromBody] CreateUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateUserCommand(CurrentUserId, CurrentUserId, request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdUser }, result);
        }

        // Put: api/User/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserResponse>> Update(
            [FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        // Delete: api/User/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            await _mediator.Send(new RemoveUserCommand(id), token);
            return NoContent();
        }
    }
}