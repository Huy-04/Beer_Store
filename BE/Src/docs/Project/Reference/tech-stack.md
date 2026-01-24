# Tech Stack

> Technologies used in Beer Store project

---

## 🛠️ Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 9.0 | Runtime |
| C# | 13 | Language |
| Entity Framework Core | 9.x | ORM (Code First) |
| SQL Server | Latest | Database |
| MediatR | Latest | CQRS Pattern |

---

## 🔐 Security

| Technology | Purpose |
|------------|---------|
| JWT Bearer | Authentication tokens |
| BCrypt.NET | Password hashing |
| Permission-based | Authorization (Handler-level) |

---

## 📦 Key Packages

### API Layer
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Swashbuckle.AspNetCore` (Swagger)
- `Serilog.AspNetCore`

### Application Layer
- `MediatR`
- `FluentValidation` ❌ (NOT used - use RuleValidator)

### Domain Layer
- No external packages (pure C#)
- Uses `Domain.Core` shared library

### Infrastructure Layer
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`

---

## 🏗️ Architecture Patterns

| Pattern | Implementation |
|---------|---------------|
| Clean Architecture | 4-layer separation |
| CQRS | MediatR Commands/Queries |
| Repository | Read/Write separation (IR/IW) |
| Unit of Work | Per-module UoW |
| Domain-Driven Design | Entities, Value Objects, Aggregates |
| RuleValidator | Domain validation (not FluentValidation) |

---

## 📝 Logging

| Technology | Purpose |
|------------|---------|
| Serilog | Structured logging |
| `UseSharedSerilog()` | Shared configuration |

---

## 🚫 NOT Used (Important)

| Technology | Alternative |
|------------|-------------|
| ❌ AutoMapper | Manual mapping extensions |
| ❌ FluentValidation | RuleValidator in Domain |
| ❌ Generic exceptions | BusinessRuleException<TField> |

---

## 🛠️ Development Commands

```bash
# Build solution
dotnet build BE/Src/BeerStore.sln

# Run API
dotnet run --project BE/Src/Core/BeerStore.Api

# Add migration
dotnet ef migrations add <Name> \
  --project BE/Src/Core/BeerStore.Infrastructure \
  --startup-project BE/Src/Core/BeerStore.Api

# Update database
dotnet ef database update \
  --project BE/Src/Core/BeerStore.Infrastructure \
  --startup-project BE/Src/Core/BeerStore.Api

# Run tests
dotnet test BE/Src/BeerStore.sln
```
