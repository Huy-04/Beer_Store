# Phase 2: Shop Module - Implementation Plan

> **Session**: 2026-01-21 (Updated: 2026-01-23)  
> **Status**: Approved, ready to implement

---

## ⚠️ Naming Convention Reminder

> **Module ≠ Entity** - See `AGENTS.md` for details

| Concept | Name | Example |
|---------|------|--------|
| **Module** | `Shop` | Folder: `Entities/Shop/`, UoW: `IShopUnitOfWork` |
| **Entity** | `Store` | File: `Store.cs`, Repo: `IRStoreRepository` |

---

## Architecture

```
User ──owns──► Store ──has──► StoreAddress (1:N)
  │              │
  │              └──► [Product Module - Phase 3]
  │                        └──► [Inventory Module - Phase 4]
  │
  └──► UserAddress (User's addresses - separate from Store)

Domain.Core/ValueObjects/Address/  ◄── Shared VOs (Province, District, Ward, Street)
         ▲                    ▲
         │                    │
   UserAddress (Auth)    StoreAddress (Shop)
```

> **Related Modules**: See `plan-productmodule.md` and `plan-inventorymodule.md`

---

## Store Entity

```csharp
public class Store : AggregateRoot
{
    public Guid OwnerId { get; }               // User owns this store (no FK - microservice ready)
    public StoreName Name { get; }
    public Slug Slug { get; }
    public Img Logo { get; }                   // From Domain.Core
    public Description Description { get; }   // From Domain.Core
    public StoreType StoreType { get; }       // OfficialStore | NormalStore
    public StoreStatus StoreStatus { get; }   // Pending | Approved | Rejected | Suspended | Banned
    
    // Computed
    public bool IsOfficial => StoreType == StoreType.OfficialStore 
                           && StoreStatus == StoreStatus.Approved;
    
    // Audit
    public Guid CreatedBy { get; }
    public Guid UpdatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    
    // State Machine Methods
    public void Approve();      // Pending → Approved
    public void Reject();       // Pending → Rejected
    public void Suspend();      // Approved → Suspended
    public void Reactivate();   // Suspended → Approved
    public void Resubmit();     // Rejected → Pending
    public void Ban();          // Any → Banned (permanent)
}
```

---

## StoreAddress Entity

```csharp
public class StoreAddress : AggregateRoot
{
    public Guid StoreId { get; }
    
    // Shared ValueObjects from Domain.Core
    public Phone Phone { get; }
    public FullName ContactName { get; }
    public Province Province { get; }
    public District District { get; }
    public Ward Ward { get; }
    public Street Street { get; }
    
    // Store-specific
    public StoreAddressType Type { get; }      // Pickup | Return
    public IsDefault IsDefault { get; }
    
    // Audit
    public Guid CreatedBy { get; }
    public Guid UpdatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
}

public enum StoreAddressType 
{ 
    Pickup = 0,  // Điểm lấy hàng (ship từ đây)
    Return = 1   // Địa chỉ trả hàng
}
```

> **Note**: `Warehouse` removed - Inventory managed in separate module without multi-warehouse for MVP

---

## Shared ValueObjects (Domain.Core) ✅ DONE

Đã có sẵn trong `Domain.Core/ValueObjects/Address/`:

| ValueObject | Used By |
|-------------|---------|
| `Province` | UserAddress, StoreAddress |
| `District` | UserAddress, StoreAddress |
| `Ward` | UserAddress, StoreAddress |
| `Street` | UserAddress, StoreAddress |
| `Phone` | UserAddress, StoreAddress |
| `FullName` | UserAddress, StoreAddress |
| `IsDefault` | UserAddress, StoreAddress |

> **DDD Pattern**: Shared Kernel - share ValueObjects, không share Entity

---

## Enums

```csharp
public enum StoreType 
{ 
    OfficialStore = 1,  // Chính hãng, platform xác nhận
    NormalStore = 2     // Shop thường
}

public enum StoreStatus 
{ 
    Pending = 1,        // Chờ duyệt
    Approved = 2,       // Đã duyệt
    Rejected = 3,       // Từ chối (có thể đăng ký lại)
    Suspended = 4,      // Tạm khóa (vi phạm)
    Banned = 5          // Cấm vĩnh viễn
}
```

---

## Value Objects (Store-specific)

| Name | Validation | Scope |
|------|------------|-------|
| `StoreName` | max 100, not empty | - |
| `Slug` | lowercase, alphanumeric + hyphen | **Global unique** (DB unique index) |

---

## Registration Flow & State Machine

```
User Register Store (chọn Type) → Status = Pending
                                      │
                              Admin Review
                                      │
                    ┌─────────────────┼─────────────────┐
                    ▼                 ▼                 ▼
               Approved           Rejected          (vi phạm)
                  │              (đăng ký lại)      Suspended
                  │                   │                 │
                  ▼                   ▼                 ▼
              Suspended ◄──────── Pending ◄──────── Approved
              (vi phạm)          (resubmit)        (reactivate)
                  │
                  ▼
               Banned (permanent - từ bất kỳ status nào)
```

### State Transition Rules

| From | To | Action |
|------|----|--------|
| `Pending` | `Approved` | Admin approve |
| `Pending` | `Rejected` | Admin reject |
| `Rejected` | `Pending` | Owner resubmit |
| `Approved` | `Suspended` | Admin suspend (vi phạm) |
| `Suspended` | `Approved` | Admin reactivate |
| `Any` | `Banned` | Admin ban (permanent) |

---

## Folder Structure

```
Shared/Domain.Core/
└── ValueObjects/
    └── Address/                    # ✅ DONE - Shared VOs
        ├── Province.cs
        ├── District.cs
        ├── Ward.cs
        ├── Street.cs
        ├── Phone.cs
        ├── FullName.cs
        └── IsDefault.cs

BeerStore.Domain/
├── Entities/Shop/                  # Module folder
│   ├── Store.cs                     # Entity file
│   └── StoreAddress.cs
├── ValueObjects/Shop/
│   ├── StoreName.cs
│   └── Slug.cs
├── Enums/Shop/
│   ├── StoreType.cs
│   ├── StoreStatus.cs
│   ├── StoreAddressType.cs
│   └── Messages/StoreField.cs
└── IRepository/Shop/               # Module folder
    ├── Read/
    │   ├── IRStoreRepository.cs     # Entity name in interface
    │   └── IRStoreAddressRepository.cs
    └── Write/
        ├── IWStoreRepository.cs
        └── IWStoreAddressRepository.cs

BeerStore.Application/
├── Modules/Shop/
│   ├── Stores/                      # Entity folder
│   │   ├── Commands/ (RegisterStore, UpdateStore, ApproveStore, RejectStore, SuspendStore, BanStore)
│   │   └── Queries/ (GetStoreBySlug, GetStoreById, GetMyStore, GetAllStores)
│   └── StoreAddresses/
│       ├── Commands/ (CreateStoreAddress, UpdateStoreAddress, RemoveStoreAddress)
│       └── Queries/ (GetStoreAddresses, GetStoreAddressById)
├── DTOs/Shop/
│   ├── Store/ (Requests/, Responses/)
│   └── StoreAddress/ (Requests/, Responses/)
└── Interface/IUnitOfWork/Shop/
    └── IShopUnitOfWork.cs           # Module name for UoW

BeerStore.Infrastructure/
├── Repository/Shop/                # Module folder
│   ├── Read/ (RStoreRepository, RStoreAddressRepository)
│   └── Write/ (WStoreRepository, WStoreAddressRepository)
├── UnitOfWork/ShopUnitOfWork.cs     # Module name
├── Persistence/
│   ├── Db/ShopDbContext.cs          # Module name
│   └── EntityConfigurations/Shop/
│       ├── StoreConfiguration.cs    # Entity name
│       ├── StoreAddressConfiguration.cs
│       └── Converter/ShopConverter.cs
└── DependencyInjection/ShopDependencyInjection.cs

BeerStore.Api/
└── Controllers/Shop/
    ├── StoreController.cs           # Entity name
    ├── StoreAddressController.cs
    └── StoreManagementController.cs  # Admin actions
```

---

## Permissions

> **Format**: `{Entity}.{Operation}.{Scope}` - See `authorization.md`

| Permission | Description |
|------------|-------------|
| `Store.Read.All` | Xem tất cả stores (Admin) |
| `Store.Read.Self` | Xem store của mình |
| `Store.Create.All` | Đăng ký store mới |
| `Store.Update.Self` | Cập nhật store của mình |
| `Store.Approve.All` | Admin approve store |
| `Store.Reject.All` | Admin reject store |
| `Store.Suspend.All` | Admin suspend store |
| `Store.Ban.All` | Admin ban store (permanent) |
| `StoreAddress.Read.Self` | Xem địa chỉ store của mình |
| `StoreAddress.Create.Self` | Tạo địa chỉ store của mình |
| `StoreAddress.Update.Self` | Cập nhật địa chỉ store của mình |
| `StoreAddress.Remove.Self` | Xóa địa chỉ store của mình |

---

## Key Decisions

1. **Store as Entity name** - `Shop` là Module, `Store` là Entity (theo AGENTS.md)
2. **StoreAddress entity riêng** - Aggregate Root riêng biệt, tương tự UserAddress (scalability, independence)
3. **Shared ValueObjects** - Province, District, Ward, Street, Phone, FullName, IsDefault đã có trong Domain.Core
4. **Store 1:N StoreAddress** - Hỗ trợ multi-pickup points
5. **StoreAddressType enum** - Pickup, Return (Business removed for simplicity)
6. **StoreType** - OfficialStore (chính hãng) vs NormalStore (shop thường)
7. **Registration flow** - Pending → Admin review → Approved/Rejected với State Machine
8. **Banned status** - Thêm trạng thái Ban vĩnh viễn
9. **OwnerId - No FK** - Chỉ lưu Guid, không có navigation property, sẵn sàng cho microservice
10. **Slug Global Unique** - Unique index trên DB, mỗi store có slug duy nhất toàn hệ thống
11. **Inventory separate** - Stock management trong Inventory Module riêng, không trong Shop

---

## Checklist

### Store Entity ✅ DONE
- [x] Store entity + Value Objects (StoreName, Slug)
- [x] StoreType, StoreStatus enums
- [x] State machine methods (Approve, Reject, Suspend, Reactivate, Resubmit, Ban)
- [x] IRStoreRepository, IWStoreRepository
- [x] StoreConfiguration + Slug unique index

### StoreAddress Entity ✅ DONE
- [x] StoreAddress entity
- [x] StoreAddressType enum
- [x] IRStoreAddressRepository, IWStoreAddressRepository
- [x] StoreAddressConfiguration

### Infrastructure ✅ DONE
- [x] ShopDbContext with DbSet<Store>, DbSet<StoreAddress>
- [x] ShopUnitOfWork
- [x] ShopConverter (ValueObject converters)
- [x] DI Registration

### Application Layer (TODO)
- [x] IShopUnitOfWork
- [ ] IShopAuthorizationService
- [ ] Store Commands/Queries
- [ ] StoreAddress Commands/Queries
- [ ] DTOs (Request/Response)
- [ ] Mapping extensions

### API Layer (TODO)
- [ ] Permissions seeding
- [ ] StoreController
- [ ] StoreAddressController
- [ ] StoreManagementController (Admin actions)

---

## 📦 Application Layer Plan

### Authorization Service

```csharp
// Interface/Services/Authorization/IShopAuthorizationService.cs
public interface IShopAuthorizationService
{
    // Store
    void EnsureCanReadAllStores();
    void EnsureCanReadStore(Guid storeId);
    Task EnsureCanReadOwnStore(Guid storeId);       // Check ownership
    void EnsureCanCreateStore();
    Task EnsureCanUpdateStore(Guid storeId);        // Owner only
    void EnsureCanApproveStore();                   // Admin
    void EnsureCanRejectStore();                    // Admin
    void EnsureCanSuspendStore();                   // Admin
    void EnsureCanBanStore();                       // Admin
    void EnsureCanReactivateStore();                // Admin
    
    // StoreAddress
    Task EnsureCanReadStoreAddresses(Guid storeId);
    Task EnsureCanCreateStoreAddress(Guid storeId);
    Task EnsureCanUpdateStoreAddress(Guid addressId);
    Task EnsureCanRemoveStoreAddress(Guid addressId);
}
```

---

### Store Commands

#### 1. RegisterStoreCommand (User đăng ký Store mới)

```csharp
// Modules/Shop/Stores/Commands/RegisterStore/RegisterStoreCommand.cs
public record RegisterStoreCommand(
    Guid CreatedBy, 
    Guid UpdatedBy, 
    RegisterStoreRequest Request) : IRequest<StoreResponse>;

// DTOs/Shop/Store/Requests/RegisterStoreRequest.cs
public record RegisterStoreRequest(
    string Name,
    string Slug,
    string? Logo,           // URL
    string? Description,
    int StoreType);         // 1=OfficialStore, 2=NormalStore
```

**Handler Flow:**
1. `_authService.EnsureCanCreateStore()`
2. Check if user already has a store (1 user = 1 store for MVP)
3. Check Slug unique
4. Create Store with Status = Pending
5. Return StoreResponse

---

#### 2. UpdateStoreCommand (Owner cập nhật Store)

```csharp
public record UpdateStoreCommand(
    Guid UpdatedBy,
    Guid StoreId,
    UpdateStoreRequest Request) : IRequest<StoreResponse>;

public record UpdateStoreRequest(
    string Name,
    string? Logo,
    string? Description);
// Note: Slug không đổi được sau khi tạo
```

**Handler Flow:**
1. `await _authService.EnsureCanUpdateStore(storeId)` - Check ownership
2. Get store by ID
3. Apply changes
4. Return StoreResponse

---

#### 3. Admin Commands (ApproveStore, RejectStore, SuspendStore, BanStore, ReactivateStore)

```csharp
public record ApproveStoreCommand(Guid StoreId, Guid UpdatedBy) : IRequest<StoreResponse>;
public record RejectStoreCommand(Guid StoreId, Guid UpdatedBy, string? Reason) : IRequest<StoreResponse>;
public record SuspendStoreCommand(Guid StoreId, Guid UpdatedBy, string Reason) : IRequest<StoreResponse>;
public record BanStoreCommand(Guid StoreId, Guid UpdatedBy, string Reason) : IRequest<StoreResponse>;
public record ReactivateStoreCommand(Guid StoreId, Guid UpdatedBy) : IRequest<StoreResponse>;
```

**Handler Flow:**
1. `_authService.EnsureCanApproveStore()` (Admin only)
2. Get store by ID
3. Call entity state machine method (`store.Approve()`)
4. Return StoreResponse

---

#### 4. ResubmitStoreCommand (Owner resubmit sau khi bị Rejected)

```csharp
public record ResubmitStoreCommand(
    Guid StoreId,
    Guid UpdatedBy,
    ResubmitStoreRequest Request) : IRequest<StoreResponse>;

public record ResubmitStoreRequest(
    string Name,
    string? Logo,
    string? Description);
```

---

### Store Queries

```csharp
// Get by ID (check permission)
public record GetStoreByIdQuery(Guid CurrentUserId, Guid StoreId) : IRequest<StoreResponse>;

// Get by Slug (public)
public record GetStoreBySlugQuery(string Slug) : IRequest<StoreResponse>;

// Get my store (current user's store)
public record GetMyStoreQuery(Guid OwnerId) : IRequest<StoreResponse?>;

// Get all stores (Admin)
public record GetAllStoresQuery(
    int Page = 1, 
    int PageSize = 20,
    StoreStatus? Status = null) : IRequest<PagedResult<StoreResponse>>;

// Get stores for public listing (only Approved)
public record GetPublicStoresQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null) : IRequest<PagedResult<StorePublicResponse>>;
```

---

### StoreAddress Commands

```csharp
// Create
public record CreateStoreAddressCommand(
    Guid CreatedBy,
    Guid UpdatedBy,
    Guid StoreId,
    CreateStoreAddressRequest Request) : IRequest<StoreAddressResponse>;

public record CreateStoreAddressRequest(
    string Phone,
    string ContactName,
    string Province,
    string District,
    string Ward,
    string Street,
    int Type,           // 0=Pickup, 1=Return
    bool IsDefault);

// Update
public record UpdateStoreAddressCommand(
    Guid UpdatedBy,
    Guid AddressId,
    UpdateStoreAddressRequest Request) : IRequest<StoreAddressResponse>;

// Remove
public record RemoveStoreAddressCommand(Guid AddressId) : IRequest<bool>;

// Set Default
public record SetDefaultStoreAddressCommand(
    Guid StoreId,
    Guid AddressId,
    int Type) : IRequest<bool>;
```

---

### StoreAddress Queries

```csharp
public record GetStoreAddressesByStoreQuery(Guid StoreId) : IRequest<IEnumerable<StoreAddressResponse>>;
public record GetStoreAddressByIdQuery(Guid AddressId) : IRequest<StoreAddressResponse>;
```

---

### DTOs

#### Store DTOs

```csharp
// Responses/StoreResponse.cs
public record StoreResponse(
    Guid Id,
    Guid OwnerId,
    string Name,
    string Slug,
    string? Logo,
    string? Description,
    StoreType StoreType,
    StoreStatus StoreStatus,
    bool IsOfficial,
    Guid CreatedBy,
    Guid UpdatedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

// Responses/StorePublicResponse.cs (cho public listing)
public record StorePublicResponse(
    Guid Id,
    string Name,
    string Slug,
    string? Logo,
    string? Description,
    bool IsOfficial);
```

#### StoreAddress DTOs

```csharp
public record StoreAddressResponse(
    Guid Id,
    Guid StoreId,
    string Phone,
    string ContactName,
    string Province,
    string District,
    string Ward,
    string Street,
    StoreAddressType Type,
    bool IsDefault,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
```

---

### Mapping Extensions

```csharp
// Mapping/Shop/StoreMap/RequestToStore.cs
public static class RequestToStore
{
    public static Store ToStore(this RegisterStoreRequest request, Guid ownerId, Guid createdBy, Guid updatedBy)
    {
        return Store.Create(
            ownerId,
            StoreName.Create(request.Name),
            Slug.Create(request.Slug),
            (StoreType)request.StoreType,
            request.Logo != null ? Img.Create(request.Logo) : Img.Create("default-logo.png"),
            request.Description != null ? Description.Create(request.Description) : Description.Create("No description"),
            createdBy,
            updatedBy);
    }
}

// Mapping/Shop/StoreMap/StoreToResponse.cs
public static class StoreToResponse
{
    public static StoreResponse ToStoreResponse(this Store store)
    {
        return new StoreResponse(
            store.Id,
            store.OwnerId,
            store.Name.Value,
            store.Slug.Value,
            store.Logo?.Value,
            store.Description?.Value,
            store.StoreType,
            store.StoreStatus,
            store.IsOfficial,
            store.CreatedBy,
            store.UpdatedBy,
            store.CreatedAt,
            store.UpdatedAt);
    }
    
    public static StorePublicResponse ToStorePublicResponse(this Store store)
    {
        return new StorePublicResponse(
            store.Id,
            store.Name.Value,
            store.Slug.Value,
            store.Logo?.Value,
            store.Description?.Value,
            store.IsOfficial);
    }
}
```

---

### Folder Structure (Application Layer)

```
BeerStore.Application/
├── Modules/Shop/
│   ├── Stores/
│   │   ├── Commands/
│   │   │   ├── RegisterStore/
│   │   │   │   ├── RegisterStoreCommand.cs
│   │   │   │   └── RegisterStoreCHandler.cs
│   │   │   ├── UpdateStore/
│   │   │   ├── ApproveStore/
│   │   │   ├── RejectStore/
│   │   │   ├── SuspendStore/
│   │   │   ├── BanStore/
│   │   │   ├── ReactivateStore/
│   │   │   └── ResubmitStore/
│   │   └── Queries/
│   │       ├── GetStoreById/
│   │       ├── GetStoreBySlug/
│   │       ├── GetMyStore/
│   │       ├── GetAllStores/
│   │       └── GetPublicStores/
│   └── StoreAddresses/
│       ├── Commands/
│       │   ├── CreateStoreAddress/
│       │   ├── UpdateStoreAddress/
│       │   ├── RemoveStoreAddress/
│       │   └── SetDefaultStoreAddress/
│       └── Queries/
│           ├── GetStoreAddressesByStore/
│           └── GetStoreAddressById/
├── DTOs/Shop/
│   ├── Store/
│   │   ├── Requests/
│   │   │   ├── RegisterStoreRequest.cs
│   │   │   ├── UpdateStoreRequest.cs
│   │   │   └── ResubmitStoreRequest.cs
│   │   └── Responses/
│   │       ├── StoreResponse.cs
│   │       └── StorePublicResponse.cs
│   └── StoreAddress/
│       ├── Requests/
│       │   ├── CreateStoreAddressRequest.cs
│       │   └── UpdateStoreAddressRequest.cs
│       └── Responses/
│           └── StoreAddressResponse.cs
├── Interface/Services/Authorization/
│   └── IShopAuthorizationService.cs
└── Mapping/Shop/
    ├── StoreMap/
    │   ├── RequestToStore.cs
    │   └── StoreToResponse.cs
    └── StoreAddressMap/
        ├── RequestToStoreAddress.cs
        └── StoreAddressToResponse.cs
```

---

### Implementation Order

1. **DTOs First** - Requests & Responses
2. **Mapping Extensions** - RequestToEntity, EntityToResponse
3. **Authorization Service Interface** - IShopAuthorizationService
4. **Store Commands** - RegisterStore → UpdateStore → Admin commands
5. **Store Queries** - GetById → GetBySlug → GetMyStore → GetAll
6. **StoreAddress Commands** - Create → Update → Remove → SetDefault
7. **StoreAddress Queries** - GetByStore → GetById

---

### IShopUnitOfWork Update (Naming Fix)

```csharp
// Hiện tại đang dùng naming không consistent:
// RShopRepository, WShopRepository (Shop = Module name)

// Nên đổi thành Entity name:
public interface IShopUnitOfWork : IUnitOfWorkGeneric
{
    // Store
    IRStoreRepository RStoreRepository { get; }
    IWStoreRepository WStoreRepository { get; }

    // StoreAddress
    IRStoreAddressRepository RStoreAddressRepository { get; }
    IWStoreAddressRepository WStoreAddressRepository { get; }
}
```

> **Note**: Cần fix `IShopUnitOfWork` và `ShopUnitOfWork` để dùng Entity name cho property

---

## 🌐 API Layer Plan

### API Endpoints

#### Store (Seller)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/stores` | Register new store | ✅ |
| GET | `/api/stores/my` | Get my store | ✅ |
| PUT | `/api/stores/{id}` | Update my store | ✅ |
| POST | `/api/stores/{id}/resubmit` | Resubmit after rejected | ✅ |

#### Store (Public)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/stores/{slug}` | Get store by slug | ❌ |
| GET | `/api/stores` | List approved stores | ❌ |

#### Store (Admin)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/admin/stores` | List all stores | ✅ Admin |
| GET | `/api/admin/stores/{id}` | Get store by ID | ✅ Admin |
| POST | `/api/admin/stores/{id}/approve` | Approve store | ✅ Admin |
| POST | `/api/admin/stores/{id}/reject` | Reject store | ✅ Admin |
| POST | `/api/admin/stores/{id}/suspend` | Suspend store | ✅ Admin |
| POST | `/api/admin/stores/{id}/reactivate` | Reactivate store | ✅ Admin |
| POST | `/api/admin/stores/{id}/ban` | Ban store | ✅ Admin |

#### StoreAddress (Seller)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/stores/{storeId}/addresses` | List my store addresses | ✅ |
| GET | `/api/stores/{storeId}/addresses/{id}` | Get address by ID | ✅ |
| POST | `/api/stores/{storeId}/addresses` | Create address | ✅ |
| PUT | `/api/stores/{storeId}/addresses/{id}` | Update address | ✅ |
| DELETE | `/api/stores/{storeId}/addresses/{id}` | Remove address | ✅ |
| POST | `/api/stores/{storeId}/addresses/{id}/set-default` | Set as default | ✅ |

---

### Controllers

#### StoreController (Mixed: Public + Protected)

```csharp
[ApiController]
[Route("api/stores")]
public class StoreController : BaseApiController
{
    private readonly IMediator _mediator;

    // === PUBLIC ===
    
    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<StorePublicResponse>> GetBySlug(
        [FromRoute] string slug, CancellationToken token)
    {
        var result = await _mediator.Send(new GetStoreBySlugQuery(slug), token);
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<StorePublicResponse>>> GetPublic(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken token = default)
    {
        var result = await _mediator.Send(
            new GetPublicStoresQuery(page, pageSize, search), token);
        return Ok(result);
    }

    // === PROTECTED (Seller) ===

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<StoreResponse>> Register(
        [FromBody] RegisterStoreRequest request, CancellationToken token)
    {
        var result = await _mediator.Send(
            new RegisterStoreCommand(CurrentUserId, CurrentUserId, request), token);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<StoreResponse>> GetMy(CancellationToken token)
    {
        var result = await _mediator.Send(new GetMyStoreQuery(CurrentUserId), token);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<StoreResponse>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateStoreRequest request,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new UpdateStoreCommand(CurrentUserId, id, request), token);
        return Ok(result);
    }

    [HttpPost("{id:guid}/resubmit")]
    [Authorize]
    public async Task<ActionResult<StoreResponse>> Resubmit(
        [FromRoute] Guid id,
        [FromBody] ResubmitStoreRequest request,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new ResubmitStoreCommand(id, CurrentUserId, request), token);
        return Ok(result);
    }
}
```

---

#### StoreManagementController (Admin Only)

```csharp
[ApiController]
[Route("api/admin/stores")]
[Authorize]  // + Admin role/permission check in handlers
public class StoreManagementController : BaseApiController
{
    private readonly IMediator _mediator;

    [HttpGet]
    public async Task<ActionResult<PagedResult<StoreResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] StoreStatus? status = null,
        CancellationToken token = default)
    {
        var result = await _mediator.Send(
            new GetAllStoresQuery(page, pageSize, status), token);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StoreResponse>> GetById(
        [FromRoute] Guid id, CancellationToken token)
    {
        var result = await _mediator.Send(
            new GetStoreByIdQuery(CurrentUserId, id), token);
        return Ok(result);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<StoreResponse>> Approve(
        [FromRoute] Guid id, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ApproveStoreCommand(id, CurrentUserId), token);
        return Ok(result);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<StoreResponse>> Reject(
        [FromRoute] Guid id,
        [FromBody] RejectStoreRequest request,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new RejectStoreCommand(id, CurrentUserId, request.Reason), token);
        return Ok(result);
    }

    [HttpPost("{id:guid}/suspend")]
    public async Task<ActionResult<StoreResponse>> Suspend(
        [FromRoute] Guid id,
        [FromBody] SuspendStoreRequest request,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new SuspendStoreCommand(id, CurrentUserId, request.Reason), token);
        return Ok(result);
    }

    [HttpPost("{id:guid}/reactivate")]
    public async Task<ActionResult<StoreResponse>> Reactivate(
        [FromRoute] Guid id, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ReactivateStoreCommand(id, CurrentUserId), token);
        return Ok(result);
    }

    [HttpPost("{id:guid}/ban")]
    public async Task<ActionResult<StoreResponse>> Ban(
        [FromRoute] Guid id,
        [FromBody] BanStoreRequest request,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new BanStoreCommand(id, CurrentUserId, request.Reason), token);
        return Ok(result);
    }
}
```

---

#### StoreAddressController (Seller)

```csharp
[ApiController]
[Route("api/stores/{storeId:guid}/addresses")]
[Authorize]
public class StoreAddressController : BaseApiController
{
    private readonly IMediator _mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StoreAddressResponse>>> GetAll(
        [FromRoute] Guid storeId, CancellationToken token)
    {
        var result = await _mediator.Send(
            new GetStoreAddressesByStoreQuery(storeId), token);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StoreAddressResponse>> GetById(
        [FromRoute] Guid storeId,
        [FromRoute] Guid id,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new GetStoreAddressByIdQuery(id), token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<StoreAddressResponse>> Create(
        [FromRoute] Guid storeId,
        [FromBody] CreateStoreAddressRequest request,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new CreateStoreAddressCommand(CurrentUserId, CurrentUserId, storeId, request), token);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StoreAddressResponse>> Update(
        [FromRoute] Guid storeId,
        [FromRoute] Guid id,
        [FromBody] UpdateStoreAddressRequest request,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new UpdateStoreAddressCommand(CurrentUserId, id, request), token);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Remove(
        [FromRoute] Guid storeId,
        [FromRoute] Guid id,
        CancellationToken token)
    {
        await _mediator.Send(new RemoveStoreAddressCommand(id), token);
        return NoContent();
    }

    [HttpPost("{id:guid}/set-default")]
    public async Task<ActionResult> SetDefault(
        [FromRoute] Guid storeId,
        [FromRoute] Guid id,
        [FromBody] SetDefaultRequest request,
        CancellationToken token)
    {
        await _mediator.Send(
            new SetDefaultStoreAddressCommand(storeId, id, request.Type), token);
        return NoContent();
    }
}
```

---

### Additional Request DTOs (for API)

```csharp
// For Admin actions with reason
public record RejectStoreRequest(string? Reason);
public record SuspendStoreRequest(string Reason);
public record BanStoreRequest(string Reason);

// For SetDefault
public record SetDefaultRequest(int Type);  // 0=Pickup, 1=Return
```

---

### API Layer Folder Structure

```
BeerStore.Api/
└── Controllers/Shop/
    ├── StoreController.cs           # Public + Seller endpoints
    ├── StoreManagementController.cs # Admin endpoints
    └── StoreAddressController.cs    # Seller address management
```

---

### Implementation Order (Full)

1. **Infrastructure Fix** - Fix IShopUnitOfWork property names
2. **DTOs** - Requests & Responses
3. **Mapping** - RequestToEntity, EntityToResponse
4. **Authorization Service** - IShopAuthorizationService + Implementation
5. **Store Commands** - RegisterStore → UpdateStore → Admin commands
6. **Store Queries** - GetById → GetBySlug → GetMyStore → GetAll → GetPublic
7. **StoreAddress Commands** - Create → Update → Remove → SetDefault
8. **StoreAddress Queries** - GetByStore → GetById
9. **Controllers** - StoreController → StoreManagementController → StoreAddressController
10. **Permissions Seeding** - Add Store permissions to DB

---

## Related Modules

| Module | Phase | Relationship |
|--------|-------|-------------|
| **Product** | Phase 3 | Product belongs to Store (StoreId) |
| **Inventory** | Phase 4 | Inventory links Store + ProductVariant |
| **Order** | Phase 5 | Order references Store |

> See: `plan-productmodule.md`, `plan-inventorymodule.md`
