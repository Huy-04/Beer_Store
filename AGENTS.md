# Beer Store Project

> **Agent Guide** - Đọc file này để hiểu project

## Tech Stack

- **.NET 9** - C# 13
- **Entity Framework Core** - Code First
- **MediatR** - CQRS pattern
- **RuleValidator** - Domain validation (trong Domain.Core)
- **BCrypt.NET** - Password hashing
- **JWT Bearer** - Authentication
- **Permission-based Authorization** - Handler-level authorization
- **SQL Server** - Database

---

## Project Structure

```
BE/Src/
├── Core/
│   ├── BeerStore.Api/           # Controllers, Program.cs
│   ├── BeerStore.Application/   # Commands, Queries, DTOs
│   ├── BeerStore.Domain/        # Entities, Value Objects
│   └── BeerStore.Infrastructure/# Repositories, DbContext, Authorization
└── Shared/
    ├── Api.Core/                # Base controllers, Middleware
    ├── Application.Core/        # IUnitOfWorkGeneric, DTOs
    ├── Domain.Core/             # Base entities, RuleValidator
    └── Infrastructure.Core/     # Base repositories
```

---

## Key Files

| Purpose | Location |
|---------|----------|
| Entities | `BeerStore.Domain/Entities/` |
| Value Objects | `BeerStore.Domain/ValueObjects/` |
| Commands | `BeerStore.Application/Modules/{Module}/{Feature}/Commands/` |
| Queries | `BeerStore.Application/Modules/{Module}/{Feature}/Queries/` |
| Repositories | `BeerStore.Infrastructure/Repository/` |
| Authorization | `BeerStore.Application/Interface/Services/Authorization/` |

---

## Rules Reference

Đọc thêm chi tiết tại:
- `@.agent/rules/architecture.md` - Kiến trúc chi tiết
- `@.agent/rules/conventions.md` - Coding conventions
- `@.agent/rules/modules.md` - Module structure
- `@.agent/rules/shared.md` - Shared layer patterns
- `@.agent/rules/authorization.md` - Permission-based authorization

---

## Dos & Don'ts

✅ **DO:**
- Tạo plan trước mọi thay đổi
- Sử dụng Value Objects cho domain concepts
- Validate bằng RuleValidator trong Domain layer
- Gọi `_authService.EnsureCan...()` đầu tiên trong handler

❌ **DON'T:**
- Không commit appsettings với secrets
- Không đặt logic trong Controllers
- Không bỏ qua authorization check trong handlers

