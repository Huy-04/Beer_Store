---
name: dotnet-patterns
description: C#/.NET patterns for Clean Architecture, CQRS, EF Core, and Domain-Driven Design. Beer Store project specific.
allowed-tools: Read, Write, Edit, Glob, Grep
---

# .NET Patterns

> Patterns for C#/.NET Clean Architecture with CQRS (MediatR) and DDD.

## 📁 Sub-files

| File | Content |
|------|---------|
| [api.md](api.md) | Controllers, Program.cs, Middleware |
| [application.md](application.md) | Commands, Queries, Handlers, DTOs, Mapping |
| [domain.md](domain.md) | Entities, Value Objects, IRepository, Enums |
| [infrastructure.md](infrastructure.md) | Repositories, DbContext, EF Config, Services, DI |

---

## 1. Project Structure

### Solution Layout

```
BE/Src/
├── Core/
│   ├── BeerStore.Api/              # Controllers, Program.cs
│   ├── BeerStore.Application/      # Commands, Queries, DTOs
│   ├── BeerStore.Domain/           # Entities, Value Objects
│   └── BeerStore.Infrastructure/   # Repositories, DbContext
└── Shared/
    ├── Api.Core/                   # BaseApiController, Middleware
    ├── Application.Core/           # IUnitOfWorkGeneric
    ├── Domain.Core/                # Entity, RuleValidator, Rules
    └── Infrastructure.Core/        # Generic Repositories
```

### Module Structure (per layer)

```
{Layer}/Modules/{Module}/{Entity}/
├── Commands/
│   ├── Create{Entity}/
│   │   ├── Create{Entity}Command.cs
│   │   └── Create{Entity}CHandler.cs
│   └── Update{Entity}/
└── Queries/
    └── Get{Entity}ById/
        ├── Get{Entity}ByIdQuery.cs
        └── Get{Entity}ByIdQHandler.cs
```

---

## 2. Core Components - CHECK BEFORE CREATING

> ⚠️ **ALWAYS check Core before creating new components**

### Domain.Core/ValueObjects/

| Component | Namespace | Use for |
|-----------|-----------|--------|
| Province, District, Ward, Street | `.Address` | Location |
| Phone, FullName | `.Address` | Contact info |
| IsDefault | `.Address` | Status flags |
| Description, Img, Money | root | Common fields |

### Domain.Core/Base/

| Class | Purpose |
|-------|--------|
| Entity, AggregateRoot, ValueObject | Base classes |
| StringBase, EnumBase, QuantityBase | ValueObject bases |

### Domain.Core/Rule/

| Rule | Purpose |
|------|--------|
| StringNotEmpty, StringMaxLength, StringMinimum, StringPattern | String validation |
| NotNegativeRule, MaxValueLimitRule | Number validation |
| EnumValidateRule | Enum validation |

### Infrastructure.Core/PropertyConverters/

| Converter | Contains |
|-----------|----------|
| AddressConverter | Province, District, Ward, Street, Phone, FullName, IsDefault |
| CommonConverterExtension | Extension methods |

### Decision: Core or Module?

| If... | Put in... |
|-------|----------|
| Used by ≥2 modules | `*.Core` |
| Module-specific only | `BeerStore.*` |

---

## 3. Layer Patterns

### Domain Layer

| Component | Base Class | Purpose |
|-----------|------------|---------|
| Entity | `Entity` | Has ID, lifecycle |
| Aggregate Root | `AggregateRoot` | Transaction boundary |
| Value Object | `ValueObject` | Immutable, no ID |
| String Value Object | `StringBase` | String wrapper |

**Entity Rules:**
- Private setters
- Factory method `Create()`
- Business methods for state changes
- ORM constructor `private Entity() { }`

### Application Layer

| Component | Pattern | Purpose |
|-----------|---------|---------|
| Command | `IRequest<TResponse>` | Write operation |
| Query | `IRequest<TResponse>` | Read operation |
| Command Handler | `CHandler` suffix | Execute command |
| Query Handler | `QHandler` suffix | Execute query |

**Handler Structure:**
1. Authorization FIRST
2. Transaction (Commands only)
3. Business logic
4. Return DTO

### Infrastructure Layer

| Component | Pattern | Purpose |
|-----------|---------|---------|
| Read Repository | `R{Entity}Repository` | Read operations |
| Write Repository | `W{Entity}Repository` | Write operations |
| UnitOfWork | `{Module}UnitOfWork` | Transaction + Repos |
| Configuration | `{Entity}Configuration` | EF mapping |

---

## 4. Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Command | `{Action}{Entity}Command` | `CreateUserCommand` |
| Query | `Get{Entity}Query` | `GetUserByIdQuery` |
| Command Handler | `{Name}CHandler` | `CreateUserCHandler` |
| Query Handler | `{Name}QHandler` | `GetUserByIdQHandler` |
| Read Interface | `IR{Entity}Repository` | `IRUserRepository` |
| Write Interface | `IW{Entity}Repository` | `IWUserRepository` |
| Read Impl | `R{Entity}Repository` | `RUserRepository` |
| Write Impl | `W{Entity}Repository` | `WUserRepository` |
| UnitOfWork | `I{Module}UnitOfWork` | `IAuthUnitOfWork` |
| Request DTO | `{Action}{Entity}Request` | `CreateUserRequest` |
| Response DTO | `{Entity}Response` | `UserResponse` |

---

## 5. Validation with RuleValidator

### Usage in Value Object

```csharp
public static Email Create(string value)
{
    RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
    {
        new StringNotEmpty<UserField>(value, UserField.Email),
        new StringMaxLength<UserField>(value, 100, UserField.Email),
        new StringMatchesRegex<UserField>(value, EmailRegex, UserField.Email),
    });
    return new Email(value);
}
```

### Available Rules (Domain.Core)

| Rule | Purpose |
|------|---------|
| `StringNotEmpty` | Required string |
| `StringMaxLength` | Max length |
| `StringMinLength` | Min length |
| `StringMatchesRegex` | Regex pattern |
| `GuidNotEmpty` | Required Guid |
| `NumberGreaterThan` | Numeric minimum |

---

## 6. Authorization Pattern

### Permission Format

`{Entity}.{Action}.{Scope}`

| Scope | Meaning |
|-------|---------|
| `All` | Any resource |
| `Self` | Own resources only |

### In Handler

```csharp
// Check with scope (All or Self)
_authService.EnsureCanReadUser(targetUserId);

// Check without scope (Admin only)
_authService.EnsureCanCreateRole();
```

### Implementation

```csharp
public void EnsureCanReadUser(Guid targetUserId)
{
    if (HasPermission("User.Read.All")) return;
    if (HasPermission("User.Read.Self") && targetUserId == _currentUserId) return;
    throw new ForbiddenAccessException();
}
```

---

## 7. Transaction Pattern

### Command Handler

```csharp
await _uow.BeginTransactionAsync(token);
try
{
    // Business logic here
    await _uow.W{Entity}Repository.AddAsync(entity, token);
    await _uow.CommitTransactionAsync(token);
    return result;
}
catch
{
    await _uow.RollbackTransactionAsync(token);
    throw;
}
```

### Query Handler (NO Transaction)

```csharp
// Queries don't need transactions
var entity = await _uow.R{Entity}Repository.GetByIdAsync(id, token);
return entity.ToResponse();
```

---

## 8. Mapping Pattern

### Manual Extension Methods

```csharp
// Request → Entity
public static User ToUser(this CreateUserRequest request, Guid createdBy, Guid updatedBy)
{
    return User.Create(
        Email.Create(request.Email),
        Password.Create(request.Password),
        createdBy,
        updatedBy
    );
}

// Entity → Response
public static UserResponse ToUserResponse(this User user)
{
    return new UserResponse(
        user.Id,
        user.Email.Value,
        user.CreatedAt
    );
}
```

**⚠️ NO AutoMapper** - Use manual mapping for explicit control.

---

## 9. EF Core Configuration

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        
        // Value Object conversion
        builder.Property(x => x.Email)
            .HasConversion(
                v => v.Value,
                v => Email.Create(v))
            .HasMaxLength(100)
            .IsRequired();
    }
}
```

---

## 10. DI Registration Pattern

```csharp
// In DependencyInjection/{Module}ServiceRegistration.cs
public static class AuthServiceRegistration
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        // UnitOfWork
        services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
        
        // Repositories
        services.AddScoped<IRUserRepository, RUserRepository>();
        services.AddScoped<IWUserRepository, WUserRepository>();
        
        // Services
        services.AddScoped<IAuthAuthorizationService, AuthAuthorizationService>();
        
        return services;
    }
}
```

---

## 11. Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : BaseApiController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        [FromBody] CreateUserRequest request, 
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new CreateUserCommand(CurrentUserId, CurrentUserId, request), 
            token);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(
        Guid id, 
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new GetUserByIdQuery(CurrentUserId, id), 
            token);
        return Ok(result);
    }
}
```

---

## ✅ Checklist

Before submitting code:

- [ ] Authorization check is FIRST in handler
- [ ] Commands use transactions
- [ ] Validation uses `RuleValidator`
- [ ] Mapping is manual (no AutoMapper)
- [ ] `CancellationToken` passed everywhere
- [ ] Naming follows conventions
- [ ] No business logic in Controllers
- [ ] Build passes: `dotnet build`
