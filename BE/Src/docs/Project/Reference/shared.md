---
trigger: always_on
glob: 
description: Shared layer documentation for Beer Store. Contains reusable components for Domain, Application, Infrastructure, and API layers.
---

# Shared Layer - Beer Store

The `Shared` folder contains the "Kernel" of the application. **AGENTS MUST** use these components instead of reinventing the wheel.

## 1. Domain.Core (`BE/Src/Shared/Domain.Core`)

### 🧱 Base Classes (MUST Inherit)

All Domain objects must inherit from these base classes:

| Class | Purpose | Location |
|-------|---------|----------|
| `Entity` | Has `Guid Id`. Use for simple entities | `Base/Entity.cs` |
| `AggregateRoot` | Inherits Entity. Has `DomainEvents`. Use for root entities | `Base/AggregateRoot.cs` |
| `ValueObject` | Immutable. Implements equality | `Base/ValueObject.cs` |

### 📦 Common ValueObjects Catalog

**AGENTS: Reuse these instead of creating new ones.**

| ValueObject | Purpose | Location |
|-------------|---------|----------|
| `Money` | Money with Currency & Math (+, -, *) | `ValueObjects/Money.cs` |
| `StringBase` | Base for string VOs | `ValueObjects/Base/StringBase.cs` |
| `QuantityBase<TNumber, TSelf>` | Base for numeric VOs with math | `ValueObjects/Base/QuantityBase.cs` |
| `EnumBase<TEnum>` | Base for enum wrapper VOs | `ValueObjects/Base/EnumBase.cs` |
| `Description` | Validated text (5-255 chars) | `ValueObjects/Description.cs` |
| `Img` | Image URL/Path (5-255 chars) | `ValueObjects/Img.cs` |
| `Address.Street` | Street name | `ValueObjects/Address/Street.cs` |
| `Address.Ward` | Ward/Phường | `ValueObjects/Address/Ward.cs` |
| `Address.District` | District/Quận | `ValueObjects/Address/District.cs` |
| `Address.Province` | Province/City | `ValueObjects/Address/Province.cs` |

### 📋 Enums

| Enum | Purpose | Location |
|------|---------|----------|
| `ErrorCategory` | HTTP error categories (400, 401, 403, 404, 409, 500) | `Enums/ErrorCategory.cs` |
| `ErrorCode` | Specific error codes for validation/business | `Enums/ErrorCode.cs` |
| `StatusEnum` | Active/Inactive status | `Enums/StatusEnum.cs` |
| `CurrencyEnum` | VND, USD | `Enums/CurrencyEnum.cs` |

**Field Enums (for validation messages):** `Enums/Messages/`
- `CommonField` - Generic fields (Description, Img, etc.)
- `MoneyField` - Money-related fields (Amount, Currency)
- `AddressField` - Address-related fields
- `ParamField` - Parameter keys (MaxLength, Minimum, Value)

### ✅ Validation (`RuleValidator`)

**Do not** throw exceptions manually for validation. Use `RuleValidator`.

```csharp
using Domain.Core.Rule;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule.StringRule;

public static Email Create(string email)
{
    // Throws MultiRuleException if any rule fails
    RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
    {
        new StringNotEmpty<UserField>(email, UserField.Email),
        new StringMaxLength<UserField>(email, 100, UserField.Email),
        new StringPattern<UserField>(email, @"^[\w\.-]+@[\w\.-]+\.\w+$", UserField.Email)
    });

    return new Email(email);
}
```

**Available Rules:**

| Category | Rule | Purpose |
|----------|------|---------|
| **StringRule** | `StringNotEmpty<TField>` | Not null or whitespace |
| | `StringMaxLength<TField>` | Maximum length limit |
| | `StringMinimum<TField>` | Minimum length limit |
| | `StringPattern<TField>` | Regex pattern match |
| | `StringContainsDigit<TField>` | Must contain digit |
| | `StringContainsLowercase<TField>` | Must contain lowercase |
| | `StringContainsUppercase<TField>` | Must contain uppercase |
| **NumberRule** | `NotNegativeRule<T, TField>` | Value >= 0 |
| | `NotZeroRule<T, TField>` | Value != 0 |
| | `MaxValueLimitRule<T, TField>` | Value <= max |
| **EnumRule** | `EnumValidateRule<TEnum, TField>` | Enum in valid list |
| | `EnumEqualRule<TEnum, TField>` | Two enums must equal |

### 🔒 Interfaces

| Interface | Purpose | Location |
|-----------|---------|----------|
| `IBusinessRule<TField>` | Contract for validation rules | `Interface/Rule/IBusinessRule.cs` |
| `IDomainEvent` | Marker for domain events | `Interface/Event/IDomainEvent.cs` |
| `IReadRepositoryGeneric<TEntity>` | Read repository contract | `Interface/IRepository/IReadRepositoryGeneric.cs` |
| `IWriteRepositoryGeneric<TEntity>` | Write repository contract | `Interface/IRepository/IWriteRepositoryGeneric.cs` |

### 💾 Generic Repositories

Do not create basic CRUD methods in your specific repositories. Inherit from these interfaces.

**IReadRepositoryGeneric<TEntity>**
```csharp
Task<IEnumerable<TEntity>> GetAllAsync(token);
Task<TEntity?> GetByIdAsync(Guid id, token);
Task<bool> ExistsByIdAsync(Guid id, token);
Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> cond, token);
Task<bool> AnyAsync(Expression<Func<TEntity, bool>> cond, token);
```

**IWriteRepositoryGeneric<TEntity>**
```csharp
Task AddAsync(TEntity entity, token); // Only adds to ChangeTracker
void Update(TEntity entity);          // Only marks as Modified
void Remove(TEntity entity);          // Only marks as Deleted
```

### 🛡️ Helpers

| Helper | Purpose | Location |
|--------|---------|----------|
| `AuthorizationHelper.ThrowForbidden<TField>()` | Throw 403 Forbidden exception | `Helpers/AuthorizationHelper.cs` |

### ⚠️ Exceptions

| Exception | Purpose | Location |
|-----------|---------|----------|
| `BusinessRuleException<TField>` | Single business rule violation | `RuleException/BusinessRuleException.cs` |
| `MultiRuleException` | Multiple validation errors | `RuleException/MultiRuleException.cs` |

---

## 2. Application.Core (`BE/Src/Shared/Application.Core`)

### 🔧 Interfaces

| Interface | Purpose | Location |
|-----------|---------|----------|
| `IUnitOfWorkGeneric` | Transaction management | `Interface/IUnitOfWork/IUnitOfWorkGeneric.cs` |
| `IJwtSettings` | JWT configuration contract | `Interface/ISettings/IJwtSettings.cs` |
| `ICurrentUserContext` | Current user info (UserId, Email, Roles, Permissions) | `Interface/Services/ICurrentUserContext.cs` |

### ⚡ Unit of Work (`IUnitOfWorkGeneric`)

Used to manage transactions from the Command Handler.

```csharp
public interface IUnitOfWorkGeneric
{
    Task BeginTransactionAsync(token);
    Task CommitTransactionAsync(token);
    Task RollbackTransactionAsync(token);
    Task<int> SaveChangesAsync(token);
}

// Usage in Handler
await _uow.BeginTransactionAsync(token);
try {
    await _uow.WRepo.AddAsync(entity, token);
    await _uow.CommitTransactionAsync(token);
} catch {
    await _uow.RollbackTransactionAsync(token);
    throw;
}
```

### 📋 Constants

| Constant | Purpose | Location |
|----------|---------|----------|
| `CustomClaimTypes.Permission` | Claim type for permissions | `Constants/CustomClaimTypes.cs` |

### 📦 DTOs

| DTO | Purpose | Location |
|-----|---------|----------|
| `MoneyRequest` | Request DTO for Money | `DTOs/Requests/Money/MoneyRequest.cs` |
| `MoneyResponse` | Response DTO for Money | `DTOs/Responses/Money/MoneyResponse.cs` |

### 🗺️ Mapping Extensions

| Extension | Purpose | Location |
|-----------|---------|----------|
| `MoneyToResponse.ToMoneyResponse()` | Money → MoneyResponse | `Mapping/MoneyMapExtension/MoneyToResponse.cs` |
| `RequestToMoney.ToMoney()` | MoneyRequest → Money | `Mapping/MoneyMapExtension/RequestToMoney.cs` |

---

## 3. Api.Core (`BE/Src/Shared/Api.Core`)

### 🎮 Base Controller

All Controllers **MUST** inherit from `BaseApiController`.

```csharp
public class UserController : BaseApiController
{
    public void SomeMethod()
    {
        // Built-in properties from Token
        Guid userId = CurrentUserId;       // Throws UnauthorizedAccessException if not found
        string? email = CurrentUserEmail;
        List<string> roles = CurrentUserRoles;
    }
}
```

### 🚨 Exception Handling (`Middleware/`)

The `ExceptionMiddleware` automatically handles these exceptions:

| Exception | HTTP Status | Use Case |
|-----------|-------------|----------|
| `BusinessRuleException<Enum>` | Based on `ErrorCategory` | Single logic error |
| `MultiRuleException` | 400 Bad Request | Multiple validation errors |
| `Exception` (unhandled) | 500 Internal Server Error | Unexpected errors |

**Error Response Classes:**
- `CustomProblemDetails` - RFC 7807 problem details with errors list
- `CustomErrorDetail` - Individual error with Field, ErrorCode, Parameters

**Throwing Business Errors:**
```csharp
throw new BusinessRuleException<UserField>(
    ErrorCategory.Conflict,       // Category (determines HTTP status)
    UserField.Email,              // Focus Field
    ErrorCode.EmailAlreadyExists, // Error Code enum
    new Dictionary<object, object>() // Parameters
);
```

### 📝 Logging (`Logging/`)

| Component | Purpose | Location |
|-----------|---------|----------|
| `SerilogExtensions.UseSharedSerilog()` | Configure Serilog for host | `Logging/SerilogExtensions.cs` |
| `SerilogOptions` | Logging configuration options | `Logging/SerilogOptions.cs` |
| `ShortSourceContextEnricher` | Shorten source context in logs | `Logging/ShortSourceContextEnricher.cs` |

---

## 4. Infrastructure.Core (`BE/Src/Shared/Infrastructure.Core`)

### ⚙️ Generic Implementations

| Class | Implements | Purpose | Location |
|-------|------------|---------|----------|
| `ReadRepositoryGeneric<T>` | `IReadRepositoryGeneric<T>` | EF Core read operations with `AsNoTracking()` | `Repository/ReadRepositoryGeneric.cs` |
| `WriteRepositoryGeneric<T>` | `IWriteRepositoryGeneric<T>` | EF Core write operations | `Repository/WriteRepositoryGeneric.cs` |
| `UnitOfWorkGeneric` | `IUnitOfWorkGeneric` | EF Core transaction management | `UnitOfWork/UnitOfWorkGeneric.cs` |

### 👤 Services

| Service | Implements | Purpose | Location |
|---------|------------|---------|----------|
| `CurrentUserContext` | `ICurrentUserContext` | Get current user from HttpContext | `Services/CurrentUserContext.cs` |

### ⚙️ Settings

| Class | Implements | Purpose | Location |
|-------|------------|---------|----------|
| `JwtSettings` | `IJwtSettings` | JWT configuration implementation | `Settings/JwtSettings.cs` |

### 🛠 Property Converters

Use these in EF Core `IEntityTypeConfiguration` for Value Objects:

```csharp
using Infrastructure.Core.PropertyConverters;

// In OnModelCreating or EntityConfiguration
builder.Property(e => e.Description)
    .HasConversion(CommonConverterExtension.DescriptionConverter);

builder.Property(e => e.Image)
    .HasConversion(CommonConverterExtension.ImgConverter);
```

**Available Converters:**
| Converter | Converts | Location |
|-----------|----------|----------|
| `DescriptionConverter` | `Description` ↔ `string` | `PropertyConverters/CommonConverterExtension.cs` |
| `ImgConverter` | `Img` ↔ `string` | `PropertyConverters/CommonConverterExtension.cs` |
