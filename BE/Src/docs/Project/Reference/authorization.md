---
trigger: always_on
glob: 
description: Authorization pattern for Beer Store
---

# Authorization Pattern

## Overview

Permission-based authorization implemented in Application layer handlers.

## Components

| Component | Location | Purpose |
|-----------|----------|---------|
| `ICurrentUserContext` | Shared/Application.Core | User info from JWT claims |
| `IAuthAuthorizationService` | Application/Interface/Services/Authorization | Authorization checks |
| `AuthenticationService` | Infrastructure/Services/Authorization | Implementation |
| `CustomClaimTypes.Permission` | Application.Core/Constants | JWT claim type |

## ⚠️ Golden Rules (Start Here)

1.  **Check FIRST:** Authorization check **MUST** be the first line in the Handler.
    ```csharp
    public async Task Handle(...) {
         _authService.EnsureCanReadUser(...); // ✅ Right here
         // ... then logic
    }
    ```
2.  **Explicit > Implicit:**
    *   `Read.All` allows reading ANY record.
    *   `Read.Self` allows reading **ONLY** your own record.
    *   Logic is usually: `if (Reset.All) return; if (Read.Self && isOwner) return; throw Forbidden;`

## Master Permission List

**Source of Truth:** Defined in `AuthAuthorizationService.cs`. Agents **MUST** use these exact strings.

### User
| Permission | Description |
|------------|-------------|
| `User.Read.All` | Read any user profile |
| `User.Read.Self` | Read own profile |
| `User.Create.All` | Create new users (Admin) |
| `User.Update.All` | Update any user |
| `User.Update.Self` | Update own profile |
| `User.Remove.All` | Delete user (Soft delete) |

### Role & Permission
| Permission | Description |
|------------|-------------|
| `Role.Read.All` | List roles |
| `Role.Create.All` | Create role |
| `Role.Update.All` | Update role |
| `Role.Remove.All` | Delete role |
| `Permission.Read.All` | List permissions |
| `Permission.Create.All` | Create permission |
| `Permission.Update.All` | Update permission |
| `Permission.Remove.All` | Delete permission |

### Address
| Permission | Description |
|------------|-------------|
| `Address.Read.All` | Read any address |
| `Address.Read.Self` | Read own address |
| `Address.Create.All` | Create address for anyone |
| `Address.Create.Self` | Create own address |
| `Address.Update.All` | Update any address |
| `Address.Update.Self` | Update own address |
| `Address.Remove.All` | Remove any address |
| `Address.Remove.Self` | Remove own address |

### RefreshToken
| Permission | Description |
|------------|-------------|
| `RefreshToken.Read.All` | Read all tokens |
| `RefreshToken.Read.Self` | Read own tokens |
| `RefreshToken.Create.All` | Create token manually |
| `RefreshToken.Create.Self` | Create own token (Login) |
| `RefreshToken.Revoke.All` | Revoke any token |
| `RefreshToken.Revoke.Self` | Revoke own token |

### Junctions (Admin Only)
- `UserRole.*`: Assign roles to users.
- `RolePermission.*`: Assign permissions to roles.

## Permission Naming Convention

```
{Entity}.{Operation}.{Scope}
```

| Part | Values | Example |
|------|--------|---------|
| Entity | User, Role, Permission, Address, RefreshToken | `User` |
| Operation | Read, Create, Update, Remove, Revoke | `Read` |
| Scope | All (global), Self (own resources) | `All` |

## Handler Implementation Pattern

```csharp
public class GetUserByIdQHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IAuthAuthorizationService _authService;
    
    public async Task<UserResponse> Handle(GetUserByIdQuery query, CancellationToken token)
    {
        // 1. Authorization check
        _authService.EnsureCanReadUser(query.UserId);
        
        // 2. Business logic
        var user = await _auow.RUserRepository.GetByIdAsync(query.UserId, token);
        return user.ToUserResponse();
    }
}
```

## JWT Token Structure

```json
{
  "sub": "user-id",
  "email": "user@example.com",
  "roles": ["Administrator"],
  "permissions": [
    "User.Read.All",
    "User.Create.All",
    "Role.Read.All"
  ]
}
```
