# Auth Module

> Authentication & Authorization module

---

## 📦 Entities

| Entity | Description |
|--------|-------------|
| `User` | User account with email, password |
| `Role` | Role definitions |
| `Permission` | Permission definitions |
| `UserRole` | Junction: User ↔ Role |
| `RolePermission` | Junction: Role ↔ Permission |

---

## 🔗 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/login` | Login, returns JWT |
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/refresh` | Refresh token |
| GET | `/api/user` | Get all users |
| GET | `/api/user/{id}` | Get user by ID |
| POST | `/api/user` | Create user |
| PUT | `/api/user/{id}` | Update user |
| DELETE | `/api/user/{id}` | Remove user |
| GET | `/api/role` | Get all roles |
| POST | `/api/role` | Create role |

---

## 🔐 Permissions

| Permission | Description |
|------------|-------------|
| `User.Read.All` | Read any user |
| `User.Read.Self` | Read own user |
| `User.Create.All` | Create users |
| `User.Update.All` | Update any user |
| `User.Update.Self` | Update own user |
| `User.Delete.All` | Delete any user |
| `Role.Read.All` | Read roles |
| `Role.Create.All` | Create roles |
| `Role.Update.All` | Update roles |
| `Role.Delete.All` | Delete roles |

---

## 📁 File Locations

```
Domain/Entities/Auth/
├── User.cs
├── Role.cs
├── Permission.cs
├── UserRole.cs
└── RolePermission.cs

Domain/ValueObjects/Auth/
├── Email.cs
├── Password.cs
└── PermissionName.cs

Application/Modules/Auth/
├── User/
│   ├── Commands/
│   └── Queries/
├── Role/
└── Junction/

Infrastructure/Repository/Auth/
├── RUserRepository.cs
├── WUserRepository.cs
└── ...

Api/Controllers/Auth/
├── AuthController.cs
├── UserController.cs
└── RoleController.cs
```

---

## ✅ Status

- [x] User CRUD
- [x] Role CRUD
- [x] Permission system
- [x] JWT Authentication
- [x] Authorization service
