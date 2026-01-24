# Beer Store - Agent Guide

> **Đọc file này trước khi làm bất kỳ task nào.**

---

## 🚨 GOLDEN RULES

### Workflow
1. **SKILL FIRST** - Đọc skill phù hợp từ `Agent-Kit/skills/` TRƯỚC khi code
2. **PLAN FIRST** - Tạo plan trước mọi thay đổi, chờ user approve
3. **VERIFY** - Build sau mỗi thay đổi: `dotnet build BE/Src/BeerStore.sln`
4. **REPORT** - Báo cáo kết quả sau khi hoàn thành task

### Code Standards
5. **Authorization FIRST** - `_authService.EnsureCan...()` là dòng đầu tiên trong Handler
6. **NO business logic in Controllers** - Controllers chỉ route đến MediatR
7. **Validate in Domain** - Dùng `RuleValidator`, không throw exceptions thủ công
8. **Transactions in Commands** - `BeginTransaction` → `Commit` / `Rollback`
9. **CORE FIRST** - Check `*.Core` trước khi tạo mới ValueObjects, Converters, Base classes

---

## 🎯 Task Flow

```
1. Nhận task từ user
         │
         ▼
2. Xác định layer/module liên quan
         │
         ▼
3. Đọc skill phù hợp (SKILL FIRST)
   ├── BE task → Agent-Kit/skills/dotnet-patterns/SKILL.md
   ├── API layer → dotnet-patterns/api.md
   ├── Application → dotnet-patterns/application.md
   ├── Domain → dotnet-patterns/domain.md
   └── Infrastructure → dotnet-patterns/infrastructure.md
         │
         ▼
4. Đọc module docs nếu cần
   └── Layer/modules/{module}/README.md
         │
         ▼
5. Tạo plan → Chờ approve → Execute → Verify → Report
```

---

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────┐
│           BeerStore.Api             │  ← Controllers, Program.cs
├─────────────────────────────────────┤
│       BeerStore.Application         │  ← Commands, Queries, DTOs, Mapping
├─────────────────────────────────────┤
│          BeerStore.Domain           │  ← Entities, Value Objects, IRepository
├─────────────────────────────────────┤
│      BeerStore.Infrastructure       │  ← Repositories, DbContext, Services
└─────────────────────────────────────┘
              ▲
          Shared/ (*.Core packages)
```

---

## ⚠️ Module vs Entity - QUAN TRỌNG

> **Module ≠ Entity** - Đây là 2 khái niệm khác nhau!

| Concept | Meaning | Example |
|---------|---------|---------|
| **Module** | Folder/namespace grouping | `Auth`, `Shop`, `Order` |
| **Entity** | Domain object thực tế | `User`, `Store`, `Product` |

### Mapping Module → Entities

| Module | Entities bên trong |
|--------|-------------------|
| `Auth` | `User`, `Role`, `UserRole`, `RefreshToken` |
| `Shop` | `Store`, `StoreAddress`, `UserStore` |

### Naming theo Module vs Entity

```
✅ ĐÚNG:
├── Infrastructure/Repository/Shop/          ← Module folder
│   ├── RStoreRepository.cs                  ← Entity name
│   └── RStoreAddressRepository.cs           ← Entity name
├── UnitOfWork/ShopUnitOfWork.cs             ← Module name
└── DependencyInjection/ShopDependencyInjection.cs  ← Module name

❌ SAI (đừng nhầm lẫn):
├── ShopRepository.cs      ← Sai! Shop là module, không phải entity
└── StoreUnitOfWork.cs     ← Sai! Store là entity, UoW theo module
```

### Quy tắc

| Component | Đặt tên theo |
|-----------|-------------|
| Folder structure | **Module** |
| UnitOfWork | **Module** (`IShopUnitOfWork`) |
| DbContext | **Module** (`ShopDbContext`) |
| DI class | **Module** (`ShopDependencyInjection`) |
| Repository | **Entity** (`RStoreRepository`) |
| Configuration | **Entity** (`StoreConfiguration`) |
| Controller | **Entity** (`StoreController`) |

---

## 🛠️ Quick Commands

```bash
# Build
dotnet build BE/Src/BeerStore.sln

# Run API
dotnet run --project BE/Src/Core/BeerStore.Api

# Migration
dotnet ef migrations add <Name> --project BE/Src/Core/BeerStore.Infrastructure --startup-project BE/Src/Core/BeerStore.Api
```

---

## ⚡ Quick Pattern - Command Handler

```csharp
public async Task<UserResponse> Handle(CreateUserCommand cmd, CancellationToken token)
{
    // 1. Authorization FIRST
    _authService.EnsureCanCreateUser();

    await _auow.BeginTransactionAsync(token);
    try
    {
        // 2. Business logic
        var user = cmd.Request.ToUser(cmd.CreatedBy, cmd.UpdatedBy);
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
```

---

## ✅ Checklist - New Feature

1. [ ] **Domain**: Entity + ValueObjects + IRepository interfaces
2. [ ] **Application**: Command/Query + Handler + DTOs + Mapping
3. [ ] **Infrastructure**: Repository impl + DbContext + Configuration + DI
4. [ ] **API**: Controller
5. [ ] **Authorization**: Permission checks in handlers

---

## 📚 Documentation

### Layer Patterns (Agent-Kit)
| Topic | File |
|-------|------|
| API & Controllers | `../Agent-Kit/skills/dotnet-patterns/api.md` |
| Commands & Queries | `../Agent-Kit/skills/dotnet-patterns/application.md` |
| Entities & ValueObjects | `../Agent-Kit/skills/dotnet-patterns/domain.md` |
| Repositories & DbContext | `../Agent-Kit/skills/dotnet-patterns/infrastructure.md` |

### Module Docs
| Module | File |
|--------|------|
| Auth | `Layer/modules/auth/README.md` |
| Shop | `Layer/modules/shop/README.md` |

### Reference
| Topic | File |
|-------|------|
| Naming Conventions | `Reference/naming.md` |
| Tech Stack | `Reference/tech-stack.md` |
| Authorization/Permissions | `Reference/authorization.md` |
| Shared Components | `Reference/shared.md` |

---

## ❌ Common Mistakes

| Don't | Do Instead |
|-------|------------|
| Logic in Controllers | Put in Handlers |
| `throw new Exception()` | `BusinessRuleException<TField>` |
| Skip authorization | `_authService.EnsureCan...()` first |
| Use FluentValidation | `RuleValidator` in Domain |
| Use AutoMapper | Manual mapping extensions |

