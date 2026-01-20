---
trigger: always_on
glob: 
description: Module structure for Beer Store
---

# Modules - Beer Store

## Bounded Contexts

| Module | Status | Description |
|--------|--------|-------------|
| **Auth** | ✅ Complete | Users, Roles, Permissions, RefreshTokens |
| **Catalog** | 🔜 Next | Products, Categories, Brands |
| **Ordering** | ⏳ Planned | Orders, OrderItems, Cart |
| **Inventory** | ⏳ Planned | Stock, Warehouses |
| **Payment** | ⏳ Planned | Payments, Transactions |

---

## Auth Module Structure

### Domain Layer

```
BeerStore.Domain/
├── Entities/Auth/
│   ├── User.cs
│   ├── Role.cs
│   ├── Permission.cs
│   ├── RefreshToken.cs
│   ├── Address.cs
│   └── Junction/
│       ├── UserRole.cs
│       └── RolePermission.cs
├── ValueObjects/Auth/
│   ├── User/
│   │   ├── Email.cs, Phone.cs, FullName.cs
│   │   ├── UserName.cs, Password.cs
│   │   └── Status/ (UserStatus, EmailStatus, PhoneStatus)
│   ├── Role/ (RoleName.cs)
│   ├── Permission/ (PermissionName.cs, PermissionCode.cs)
│   ├── RefreshToken/ (TokenHash.cs, DeviceId.cs, ...)
│   └── Address/ (Street.cs, City.cs, ...)
└── IRepository/Auth/
    ├── Read/  (IRUserRepository, IRRoleRepository, ...)
    └── Write/ (IWUserRepository, IWRoleRepository, ...)
```

### Application Layer

```
BeerStore.Application/
├── Modules/Auth/
│   ├── Users/
│   │   ├── Commands/ (CreateUser, UpdateUser, RemoveUser)
│   │   └── Queries/  (GetAllUser, GetUserById, GetUserByEmail, ...)
│   ├── Roles/
│   │   ├── Commands/ (CreateRole, UpdateRole, RemoveRole)
│   │   └── Queries/  (GetAllRole, GetRoleById, ...)
│   ├── Permissions/
│   ├── RefreshTokens/
│   ├── Addresses/
│   ├── Authentication/ (Login, Logout, RefreshToken)
│   └── Junction/ (UserRole, RolePermission)
├── DTOs/Auth/
│   ├── User/ (Requests/, Responses/)
│   ├── Role/, Permission/, ...
├── Mapping/Auth/
│   └── UserMap/, RoleMap/, ...
└── Interface/
    ├── IUnitOfWork/Auth/IAuthUnitOfWork.cs
    └── Services/ (IPasswordHasher, IJwtService)
```

### Infrastructure Layer

```
BeerStore.Infrastructure/
├── Repository/Auth/
│   ├── Read/  (RUserRepository, RRoleRepository, ...)
│   └── Write/ (WUserRepository, WRoleRepository, ...)
├── UnitOfWork/
│   └── AuthUnitOfWork.cs
├── Persistence/
│   ├── Db/AuthDbContext.cs
│   └── Configuration/ (UserConfiguration, RoleConfiguration, ...)
└── Services/
    ├── JwtService.cs
    └── PasswordHasher.cs
```

### API Layer

```
BeerStore.Api/
└── Controllers/Auth/
    ├── UserController.cs
    ├── RoleController.cs
    ├── PermissionController.cs
    ├── RefreshTokenController.cs
    ├── AddressController.cs
    ├── AuthenticationController.cs
    └── Junction/
        ├── UserRoleController.cs
        └── RolePermissionController.cs
```

---

## Adding New Module

Khi thêm module mới (e.g., Catalog):

1. **Domain**: Tạo `Entities/Catalog/`, `ValueObjects/Catalog/`, `IRepository/Catalog/`
2. **Application**: Tạo `Modules/Catalog/`, `DTOs/Catalog/`, `Interface/IUnitOfWork/Catalog/ICatalogUnitOfWork.cs`
3. **Infrastructure**: Tạo `Repository/Catalog/`, `UnitOfWork/CatalogUnitOfWork.cs`, `Persistence/Configuration/Catalog/`
4. **API**: Tạo `Controllers/Catalog/`
5. **DI**: Register trong `DependencyInjection.cs`
