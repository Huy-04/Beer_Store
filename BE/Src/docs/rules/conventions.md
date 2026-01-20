---
trigger: always_on
glob: 
description: Coding conventions for Beer Store
---

# Coding Conventions

## File Structure

```
Modules/{Module}/{Feature}/
├── Commands/
│   └── Create{Entity}/
│       ├── Create{Entity}Command.cs
│       └── Create{Entity}CHandler.cs    # C = Command
└── Queries/
    └── Get{Entity}/
        ├── Get{Entity}Query.cs
        └── Get{Entity}QHandler.cs       # Q = Query
```

## Naming

| Type | Pattern | Example |
|------|---------|---------|
| Entity | PascalCase | `User`, `Role` |
| Value Object | PascalCase | `Email`, `Password` |
| Command | `{Action}{Entity}Command` | `CreateUserCommand` |
| Query | `Get{Entity}Query` | `GetUserByIdQuery` |
| Command Handler | `{Name}CHandler` | `CreateUserCHandler` |
| Query Handler | `{Name}QHandler` | `GetUserByIdQHandler` |
| Read Repo Interface | `IR{Entity}Repository` | `IRUserRepository` |
| Write Repo Interface | `IW{Entity}Repository` | `IWUserRepository` |
| Read Repo Impl | `R{Entity}Repository` | `RUserRepository` |
| Module UnitOfWork | `I{Module}UnitOfWork` | `IAuthUnitOfWork` |
| Request DTO | `{Action}{Entity}Request` | `CreateUserRequest` |
| Response DTO | `{Entity}Response` | `UserResponse` |

## Entity Pattern

```csharp
public class User : AggregateRoot
{
    // Properties với private setters
    public Email Email { get; private set; }
    
    // Private constructor
    private User() { }
    
    // Factory method
    public static User Create(Email email, ...) { }
    
    // Update methods
    public void UpdateEmail(Email email)
    {
        if (Email == email) return;
        Email = email;
        Touch();
    }
    
    // Touch for UpdatedAt
    public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
```

## Domain Validation (ValueObject)

```csharp
public class Email : StringBase
{
    private Email(string value) : base(value) { }

    public static Email Create(string value)
    {
        RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
        {
            new StringMaxLength<UserField>(value, 100, UserField.Email),
            new StringNotEmpty<UserField>(value, UserField.Email),
            new StringMinimum<UserField>(value, 5, UserField.Email),
        });
        return new Email(value);
    }
}
```

## Handler Pattern

```csharp
public class CreateUserCHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IAuthUnitOfWork _auow;
    private readonly IAuthAuthorizationService _authService;
    
    public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken token)
    {
        // 1. Authorization check (đầu tiên)
        _authService.EnsureCanCreateUser();
        
        await _auow.BeginTransactionAsync(token);
        try
        {
            // 2. Business logic checks
            if (await _auow.RUserRepository.ExistsByEmailAsync(user.Email, token))
                throw new BusinessRuleException<UserField>(...);
            
            // 3. Write operations
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

## Mapping (Extension Methods)

```csharp
// Request → Entity
public static User ToUser(this CreateUserRequest request, ...) { }

// Entity → Response
public static UserResponse ToUserResponse(this User user) { }
```
