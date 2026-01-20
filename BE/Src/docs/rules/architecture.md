---
trigger: always_on
glob: 
description: Architecture documentation for Beer Store
---

# Architecture - Beer Store

## Clean Architecture Layers

```
┌─────────────────────────────────────┐
│           BeerStore.Api             │  ← Controllers, Middleware
├─────────────────────────────────────┤
│       BeerStore.Application         │  ← Commands, Queries, DTOs, Mapping
├─────────────────────────────────────┤
│          BeerStore.Domain           │  ← Entities, Value Objects, IRepository
├─────────────────────────────────────┤
│      BeerStore.Infrastructure       │  ← Repositories, DbContext, UoW
└─────────────────────────────────────┘
              ▲           ▲
          *.Core (Shared Layer)
```

## Shared Layer (*.Core)

| Project | Provides |
|---------|----------|
| `Domain.Core` | Entity, AggregateRoot, ValueObject, RuleValidator, IReadRepositoryGeneric, IWriteRepositoryGeneric |
| `Application.Core` | IUnitOfWorkGeneric, IJwtSettings |
| `Infrastructure.Core` | ReadRepositoryGeneric, WriteRepositoryGeneric, UnitOfWorkGeneric |
| `Api.Core` | BaseApiController, ExceptionMiddleware |

## MediatR Pipeline

```
Request → Handler → Response
```

## Repository Pattern

```csharp
// Generic interfaces (Domain.Core)
IReadRepositoryGeneric<TEntity>   // GetAll, GetById, Find, Any, ExistsById
IWriteRepositoryGeneric<TEntity>  // Add, Update, Remove

// Module-specific interfaces (BeerStore.Domain)
IR{Entity}Repository : IReadRepositoryGeneric<{Entity}>  // Custom read methods
IW{Entity}Repository : IWriteRepositoryGeneric<{Entity}> // Custom write methods

// Implementations (BeerStore.Infrastructure)
R{Entity}Repository : ReadRepositoryGeneric<{Entity}>, IR{Entity}Repository
```

## UnitOfWork Pattern

```csharp
// Generic (Application.Core)
IUnitOfWorkGeneric
    SaveChangesAsync()
    BeginTransactionAsync()
    CommitTransactionAsync()
    RollbackTransactionAsync()

// Module-specific (BeerStore.Application)
IAuthUnitOfWork : IUnitOfWorkGeneric
    RUserRepository      // Read
    WUserRepository      // Write
    RRoleRepository
    WRoleRepository
    ... (all repos for module)
```

## Domain Validation Pattern

```csharp
// Validation trong ValueObject.Create()
RuleValidator.CheckRules<TField>(rules);
// Throws MultiRuleException if any rule fails

// Handler throw BusinessRuleException cho business logic
throw new BusinessRuleException<UserField>(
    ErrorCategory.Conflict,
    UserField.Email,
    ErrorCode.EmailAlreadyExists,
    parameters);
```

## Value Objects

Located in `BeerStore.Domain/ValueObjects/{Module}/`:
- Inherit from `ValueObject` hoặc `StringBase`
- Validation trong static `Create()` method
- Examples: `Email`, `Password`, `UserName`, `Phone`, `RoleName`
