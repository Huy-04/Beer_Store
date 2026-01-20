---
trigger: always_on
glob: 
description: Shared layer documentation for Beer Store
---

# Shared Layer - Beer Store

Các project dùng chung cho toàn bộ solution, chứa base classes và generic implementations.

## Projects Overview

| Project | Purpose |
|---------|---------|
| `Domain.Core` | Base classes, RuleValidator, Generic repository interfaces |
| `Application.Core` | IUnitOfWorkGeneric, ISettings interfaces, Shared DTOs |
| `Infrastructure.Core` | Generic repository & UoW implementations |
| `Api.Core` | BaseApiController, ExceptionMiddleware, Logging |

---

## Domain.Core

### Base Classes

```csharp
// Entity - Base với Guid Id
public abstract class Entity
{
    public Guid Id { get; protected set; }
}

// AggregateRoot - Với Domain Events support
public abstract class AggregateRoot : Entity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    protected void AddDomainEvent(IDomainEvent domainEvent);
    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents();
}

// ValueObject - Với equality comparison
public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetAtomicValues();
}
```

### RuleValidator Pattern

```csharp
// Domain validation - throws MultiRuleException if any rule fails
RuleValidator.CheckRules<TField>(new IBusinessRule<TField>[]
{
    new StringMaxLength<TField>(value, 100, Field.Name),
    new StringNotEmpty<TField>(value, Field.Name),
    new NotNegativeRule<decimal, TField>(amount, Field.Amount),
});
```

**Available Rules:**
- `StringRule/`: NotEmpty, MinLength, MaxLength, Pattern
- `NumberRule/`: NotNegative, InRange
- `EnumRule/`: EnumValidate, EnumEqual

### Generic Repository Interfaces

```csharp
// Read operations
public interface IReadRepositoryGeneric<TEntity> where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token);
}

// Write operations
public interface IWriteRepositoryGeneric<TEntity> where TEntity : Entity
{
    Task AddAsync(TEntity entity, CancellationToken token);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
```

### Shared ValueObjects

- `Money` - Amount + Currency với arithmetic operators (+, -, *)
- `Description` - Text với validation
- `Img` - Image path/url
- `StringBase` - Base cho string ValueObjects
- `QuantityBase` - Base cho quantity ValueObjects

---

## Application.Core

### IUnitOfWorkGeneric

```csharp
public interface IUnitOfWorkGeneric
{
    Task<int> SaveChangesAsync(CancellationToken token);
    Task BeginTransactionAsync(CancellationToken token);
    Task CommitTransactionAsync(CancellationToken token);
    Task RollbackTransactionAsync(CancellationToken token);
}
```

### Interfaces
- `IJwtSettings` - JWT configuration contract

### Shared DTOs
- `MoneyRequest`, `MoneyResponse` - Money DTO pair
- `RequestToMoney`, `MoneyToResponse` - Mapping extensions

---

## Infrastructure.Core

### Generic Implementations

```csharp
// Read repository base
public class ReadRepositoryGeneric<TEntity> : IReadRepositoryGeneric<TEntity>
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _entities;
}

// Write repository base
public class WriteRepositoryGeneric<TEntity> : IWriteRepositoryGeneric<TEntity>

// UnitOfWork base
public class UnitOfWorkGeneric : IUnitOfWorkGeneric
```

### PropertyConverters
- `CommonConverterExtension` - EF Core value converters cho ValueObjects

---

## Api.Core

### BaseApiController

```csharp
public abstract class BaseApiController : ControllerBase
{
    protected Guid CurrentUserId { get; }      // From JWT claims
    protected string? CurrentUserEmail { get; }
    protected List<string> CurrentUserRoles { get; }
}
```

### ExceptionMiddleware

Handles:
- `BusinessRuleException<TField>` → 4xx với CustomProblemDetails
- `MultiRuleException` → Multiple validation errors
- Unhandled exceptions → 500

### Logging
- `SerilogExtensions` - Serilog configuration
- `ShortSourceContextEnricher` - Custom enricher
