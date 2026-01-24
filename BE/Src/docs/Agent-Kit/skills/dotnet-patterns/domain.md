# Domain Layer Patterns

> Entities, Value Objects, IRepository, Enums

---

> ⚠️ **Before creating ValueObject**: Check `Domain.Core/ValueObjects/` first.
> See [SKILL.md - Core Components](SKILL.md#2-core-components---check-before-creating)

---

## Folder Structure

```
BeerStore.Domain/
├── Entities/
│   └── Auth/
│       ├── User.cs
│       └── Junction/UserRole.cs
├── ValueObjects/
│   └── Auth/User/
│       ├── Email.cs
│       └── Status/UserStatus.cs
├── Enums/
│   └── Auth/Messages/UserField.cs
└── IRepository/
    └── Auth/
        ├── Read/IRUserRepository.cs
        └── Write/IWUserRepository.cs
```

---

## Entity Rules

| Rule | Description |
|------|-------------|
| Inheritance | `AggregateRoot` (root) or `Entity` |
| Properties | `private set` |
| Constructors | `private` parameterless (ORM) + `private` full |
| Factory | `static Create()` method |
| State changes | Business methods (`UpdateEmail()`, `ChangeStatus()`) |
| Audit | `CreatedBy`, `UpdatedBy`, `CreatedAt`, `UpdatedAt` |
| Collections | `IReadOnlyCollection<T>` |

---

## Entity Template

```csharp
public class User : AggregateRoot
{
    public Email Email { get; private set; }
    public FullName FullName { get; private set; }
    public UserStatus UserStatus { get; private set; }
    public Guid CreatedBy { get; private set; }
    public Guid UpdatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private User() { }  // ORM

    private User(Guid id, Email email, FullName fullName, Guid createdBy, Guid updatedBy) : base(id)
    {
        Email = email;
        FullName = fullName;
        UserStatus = UserStatus.Create(StatusEnum.Active);
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
        CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
    }

    public static User Create(Email email, FullName fullName, Guid createdBy, Guid updatedBy)
        => new(Guid.NewGuid(), email, fullName, createdBy, updatedBy);

    public void UpdateEmail(Email email) { Email = email; Touch(); }
    public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
```

---

## Value Object - StringBase

```csharp
public class Email : StringBase
{
    private Email(string value) : base(value) { }

    public static Email Create(string value)
    {
        RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
        {
            new StringNotEmpty<UserField>(value, UserField.Email),
            new StringMaxLength<UserField>(value, 100, UserField.Email),
        });
        return new Email(value);
    }
}
```

---

## Value Object - EnumBase (Status)

```csharp
public class UserStatus : EnumBase<StatusEnum>
{
    private UserStatus(StatusEnum value) : base(value) { }

    public static UserStatus Create(StatusEnum value)
    {
        RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
        {
            new EnumValidateRule<StatusEnum, UserField>(value, 
                Enum.GetValues<StatusEnum>().ToList(), UserField.UserStatus)
        });
        return new UserStatus(value);
    }
}
```

---

## Available Rules (Domain.Core)

| Category | Rule | Purpose |
|----------|------|---------|
| String | `StringNotEmpty<T>` | Required |
| | `StringMaxLength<T>` | Max length |
| | `StringMinimum<T>` | Min length |
| | `StringPattern<T>` | Regex |
| | `StringContainsDigit<T>` | Has digit |
| | `StringContainsUppercase<T>` | Has uppercase |
| Number | `NotNegativeRule<T, TField>` | >= 0 |
| | `MaxValueLimitRule<T, TField>` | <= max |
| Enum | `EnumValidateRule<TEnum, TField>` | Valid enum |

---

## Field Enum (for validation)

```csharp
public enum UserField
{
    IdUser, Email, Password, FullName, UserStatus
}
```

---

## Repository Interfaces

```csharp
// Read
public interface IRUserRepository : IReadRepositoryGeneric<User>
{
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken token = default);
    Task<User?> GetByEmailWithRolesAsync(Email email, CancellationToken token = default);
}

// Write
public interface IWUserRepository : IWriteRepositoryGeneric<User>
{
    // Usually empty - inherits Add, Update, Remove
}
```

---

## Junction Entity

```csharp
public class UserRole : Entity
{
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }

    private UserRole() { }

    private UserRole(Guid id, Guid userId, Guid roleId) : base(id)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public static UserRole Create(Guid userId, Guid roleId)
        => new(Guid.NewGuid(), userId, roleId);
}
```
