# Naming Conventions

> Quy tắc đặt tên cho Beer Store project

---

## 📋 Summary Table

| Type | Pattern | Example |
|------|---------|---------|
| Entity | PascalCase | `User`, `Role`, `Store` |
| Value Object | PascalCase | `Email`, `Password` |
| Command | `{Action}{Entity}Command` | `CreateUserCommand` |
| Query | `Get{Entity}Query` | `GetUserByIdQuery` |
| Command Handler | `{Name}CHandler` | `CreateUserCHandler` |
| Query Handler | `{Name}QHandler` | `GetUserByIdQHandler` |
| Read Repo Interface | `IR{Entity}Repository` | `IRUserRepository` |
| Write Repo Interface | `IW{Entity}Repository` | `IWUserRepository` |
| Read Repo Impl | `R{Entity}Repository` | `RUserRepository` |
| Write Repo Impl | `W{Entity}Repository` | `WUserRepository` |
| Module UnitOfWork | `I{Module}UnitOfWork` | `IAuthUnitOfWork` |
| Request DTO | `{Action}{Entity}Request` | `CreateUserRequest` |
| Response DTO | `{Entity}Response` | `UserResponse` |
| Field Enum | `{Entity}Field` | `UserField` |
| Configuration | `{Entity}Configuration` | `UserConfiguration` |
| Controller | `{Entity}Controller` | `UserController` |

---

## 🏗️ Layer-specific

### Domain Layer

```csharp
// Entity
public class User : AggregateRoot { }
public class UserRole : Entity { }  // Junction

// Value Object
public class Email : StringBase { }
public class Password : ValueObject { }

// Field Enum
public enum UserField { IdUser, Email, Password, ... }

// Repository Interface
public interface IRUserRepository : IReadRepositoryGeneric<User> { }
public interface IWUserRepository : IWriteRepositoryGeneric<User> { }
```

### Application Layer

```csharp
// Command
public record CreateUserCommand(...) : IRequest<UserResponse>;
public record UpdateUserCommand(...) : IRequest<UserResponse>;
public record RemoveUserCommand(...) : IRequest<bool>;

// Query
public record GetUserByIdQuery(...) : IRequest<UserResponse>;
public record GetAllUserQuery(...) : IRequest<IEnumerable<UserResponse>>;

// Handler
public class CreateUserCHandler : IRequestHandler<CreateUserCommand, UserResponse> { }
public class GetUserByIdQHandler : IRequestHandler<GetUserByIdQuery, UserResponse> { }

// DTO
public record CreateUserRequest(string Email, string Password);
public record UserResponse(Guid Id, string Email, DateTimeOffset CreatedAt);

// Mapping
public static class UserMapping {
    public static User ToUser(this CreateUserRequest request, ...) { }
    public static UserResponse ToUserResponse(this User user) { }
}
```

### Infrastructure Layer

```csharp
// Repository
public class RUserRepository : ReadRepositoryGeneric<User>, IRUserRepository { }
public class WUserRepository : WriteRepositoryGeneric<User>, IWUserRepository { }

// UnitOfWork
public interface IAuthUnitOfWork : IUnitOfWorkGeneric {
    IRUserRepository RUserRepository { get; }
    IWUserRepository WUserRepository { get; }
}

// Configuration
public class UserConfiguration : IEntityTypeConfiguration<User> { }

// DI Registration
public static class AuthServiceRegistration { }
```

### API Layer

```csharp
// Controller
[ApiController]
[Route("api/[controller]")]
public class UserController : BaseApiController { }

// Special Controllers
public class AuthController : BaseApiController { }  // Public endpoints
```

---

## 📁 Folder Structure

```
{Layer}/
├── {Module}/                    # Auth, Shop, Catalog
│   ├── {Entity}/               # User, Role, Store
│   │   ├── Commands/           # CreateUser/, UpdateUser/
│   │   └── Queries/            # GetAllUser/, GetUserById/
│   └── Junction/               # UserRole/, RolePermission/
```

---

## ⚠️ Common Mistakes

| ❌ Wrong | ✅ Correct |
|----------|-----------|
| `UserCommandHandler` | `CreateUserCHandler` |
| `UserQueryHandler` | `GetUserByIdQHandler` |
| `IUserRepository` | `IRUserRepository` / `IWUserRepository` |
| `UserRepository` | `RUserRepository` / `WUserRepository` |
| `UserDTO` | `UserResponse` |
| `CreateUserDTO` | `CreateUserRequest` |
