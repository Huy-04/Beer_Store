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
| `ICurrentUserContext` | Application/Interface/Services | User info from JWT claims |
| `IAuthAuthorizationService` | Application/Interface/Services/Authorization | Authorization checks |
| `AuthAuthorizationService` | Infrastructure/Services/Authorization | Implementation |
| `CustomClaimTypes.Permission` | Application.Core/Constants | JWT claim type |

## Permission Naming Convention

```
{Entity}.{Operation}.{Scope}
```

| Part | Values | Example |
|------|--------|---------|
| Entity | User, Role, Permission, UserAddress, RefreshToken, UserRole, RolePermission, UserPermission | `User` |
| Operation | Read, Create, Update, Remove, Revoke | `Read` |
| Scope | All (global), Self (own resources) | `All` |

**Examples:**
- `User.Read.All` - Đọc tất cả users
- `User.Read.Self` - Chỉ đọc user của mình
- `RefreshToken.Revoke.Self` - Revoke token của mình

## Handler Authorization Pattern

```csharp
public class GetUserByIdQHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IAuthAuthorizationService _authService;
    
    public async Task<UserResponse> Handle(GetUserByIdQuery query, CancellationToken token)
    {
        // Authorization check đầu tiên
        _authService.EnsureCanReadUser(query.UserId);
        
        // Business logic
        var user = await _auow.RUserRepository.GetByIdAsync(query.UserId, token);
        return user.ToUserResponse();
    }
}
```

## Authorization Methods

| Method | Permission Required |
|--------|-------------------|
| `EnsureCanReadUser(userId)` | User.Read.All OR (User.Read.Self + owner) |
| `EnsureCanReadAllUsers()` | User.Read.All |
| `EnsureCanCreateUser()` | User.Create.All |
| `EnsureCanUpdateUser(userId)` | User.Update.All OR (User.Update.Self + owner) |
| `EnsureCanRemoveUser()` | User.Remove.All |
| `EnsureCanRemovePermission()` | Permission.Remove.All |
| `EnsureCanRemoveUserAddress(id)` | UserAddress.Remove.All OR (UserAddress.Remove.Self + owner) |
| `EnsureCanRevokeRefreshToken(userId)` | RefreshToken.Revoke.All OR (RefreshToken.Revoke.Self + owner) |
| `EnsureCanRemoveUserPermission()` | UserPermission.Remove.All |

## Handlers Without Authorization

Only handlers in `Authentication` folder:
- LoginCHandler
- LogoutCHandler  
- RegisterCHandler
- RefreshAccessTokenCHandler

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
