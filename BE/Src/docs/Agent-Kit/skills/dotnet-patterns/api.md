# API Layer Patterns

> Controllers, Program.cs, Middleware

---

## Folder Structure

```
BeerStore.Api/
├── Controllers/
│   ├── Auth/                    # Module
│   │   ├── AuthenticationController.cs  # Public (login, register)
│   │   ├── UserController.cs
│   │   └── Junction/
│   │       └── UserRoleController.cs
│   └── Shop/
├── Program.cs
└── appsettings.json
```

---

## Controller Rules

| Rule | Description |
|------|-------------|
| Inherit | `BaseApiController` |
| Attributes | `[ApiController]`, `[Route("api/[controller]")]` |
| Auth | `[Authorize]` class-level (protected) or method-level |
| DI | Inject `IMediator` only |
| CancellationToken | ALWAYS accept `CancellationToken token` |
| Return | `ActionResult<TResponse>` or `ActionResult` |
| Route | Use `{id:guid}` for Guid params |
| Logic | **NO business logic** - only route to MediatR |

---

## Controller Template

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : BaseApiController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll(CancellationToken token)
    {
        var result = await _mediator.Send(new GetAllUserQuery(), token);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById([FromRoute] Guid id, CancellationToken token)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request, CancellationToken token)
    {
        var result = await _mediator.Send(new CreateUserCommand(CurrentUserId, CurrentUserId, request), token);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken token)
    {
        var result = await _mediator.Send(new UpdateUserCommand(id, CurrentUserId, request), token);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        await _mediator.Send(new RemoveUserCommand(id), token);
        return NoContent();
    }
}
```

---

## Public Controller (No [Authorize])

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : BaseApiController
{
    private readonly IMediator _mediator;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken token)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password), token);
        return Ok(result);
    }

    [Authorize]  // Method-level auth
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] LogoutRequest request, CancellationToken token)
    {
        await _mediator.Send(new LogoutCommand(CurrentUserId, request.RefreshToken), token);
        return NoContent();
    }
}
```

---

## Middleware Order (Program.cs)

```
Request → ExceptionMiddleware → CORS → Routing → Authentication → Authorization → Controllers → Response
```

```csharp
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```
