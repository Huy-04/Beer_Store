# Application Layer Patterns

> Commands, Queries, Handlers, DTOs, Mapping

---

## Folder Structure

```
BeerStore.Application/
├── Modules/
│   └── Auth/
│       ├── Users/
│       │   ├── Commands/
│       │   │   └── CreateUser/
│       │   │       ├── CreateUserCommand.cs
│       │   │       └── CreateUserCHandler.cs
│       │   └── Queries/
│       │       └── GetUserById/
│       └── Junction/
├── DTOs/
│   └── Auth/User/
│       ├── Requests/CreateUserRequest.cs
│       └── Responses/UserResponse.cs
├── Interface/
│   ├── IUnitOfWork/Auth/IAuthUnitOfWork.cs
│   └── Services/Authorization/IAuthAuthorizationService.cs
└── Mapping/
    └── Auth/UserMap/
        ├── RequestToUser.cs
        └── UserToResponse.cs
```

---

## CQRS Rules

| Type | Transaction | Repository | Return |
|------|-------------|------------|--------|
| Command | ✅ BEGIN/COMMIT/ROLLBACK | W...Repository + R...Repository | Response DTO |
| Query | ❌ No transaction | R...Repository ONLY | Response DTO |

---

## Command Definition

```csharp
public record CreateUserCommand(Guid CreatedBy, Guid UpdatedBy, CreateUserRequest Request) : IRequest<UserResponse>;
```

## Query Definition

```csharp
public record GetUserByIdQuery(Guid IdUser) : IRequest<UserResponse>;
public record GetAllUserQuery : IRequest<IEnumerable<UserResponse>>;
```

---

## Command Handler Template

```csharp
public class CreateUserCHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IAuthUnitOfWork _auow;
    private readonly IAuthAuthorizationService _authService;

    public async Task<UserResponse> Handle(CreateUserCommand cmd, CancellationToken token)
    {
        // 1. Authorization FIRST
        _authService.EnsureCanCreateUser();

        // 2. Transaction
        await _auow.BeginTransactionAsync(token);
        try
        {
            // 3. Map & Execute
            var user = cmd.Request.ToUser(cmd.CreatedBy, cmd.UpdatedBy);
            
            // 4. Check conflicts (Read repo)
            if (await _auow.RUserRepository.ExistsByEmailAsync(user.Email, token))
                throw new BusinessRuleException<UserField>(ErrorCategory.Conflict, UserField.Email, ErrorCode.EmailAlreadyExists);

            // 5. Write
            await _auow.WUserRepository.AddAsync(user, token);
            await _auow.CommitTransactionAsync(token);
            
            return user.ToUserResponse();
        }
        catch
        {
            await _auow.RollbackTransactionAsync(token);
            throw;
        }
    }
}
```

---

## Query Handler Template

```csharp
public class GetUserByIdQHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IAuthUnitOfWork _auow;
    private readonly IAuthAuthorizationService _authService;

    public async Task<UserResponse> Handle(GetUserByIdQuery query, CancellationToken token)
    {
        // 1. Authorization
        _authService.EnsureCanReadUser(query.IdUser);

        // 2. Query (no transaction)
        var user = await _auow.RUserRepository.GetByIdAsync(query.IdUser, token);
        if (user == null)
            throw new BusinessRuleException<UserField>(ErrorCategory.NotFound, UserField.IdUser, ErrorCode.IdNotFound);

        return user.ToUserResponse();
    }
}
```

---

## DTOs (record types)

```csharp
// Request
public record CreateUserRequest(string Email, string FullName, string Password);

// Response
public record UserResponse(Guid IdUser, string Email, StatusEnum UserStatus, DateTimeOffset CreatedAt);
```

---

## Mapping (NO AutoMapper)

```csharp
// RequestToUser.cs
public static User ToUser(this CreateUserRequest request, Guid createdBy, Guid updatedBy)
{
    return User.Create(
        Email.Create(request.Email),
        FullName.Create(request.FullName),
        Password.Create(request.Password),
        createdBy, updatedBy);
}

// UserToResponse.cs
public static UserResponse ToUserResponse(this User user)
{
    return new(user.Id, user.Email.Value, user.UserStatus.Value, user.CreatedAt);
}
```

---

## Unit of Work Interface

```csharp
public interface IAuthUnitOfWork : IUnitOfWorkGeneric
{
    IRUserRepository RUserRepository { get; }
    IWUserRepository WUserRepository { get; }
    IRRoleRepository RRoleRepository { get; }
    IWRoleRepository WRoleRepository { get; }
}
```

---

## Authorization Service Interface

```csharp
public interface IAuthAuthorizationService
{
    void EnsureCanReadAllUsers();
    void EnsureCanReadUser(Guid targetUserId);
    void EnsureCanCreateUser();
    void EnsureCanUpdateUser(Guid targetUserId);
    void EnsureCanRemoveUser();
}
```
