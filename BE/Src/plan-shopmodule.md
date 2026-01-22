# Phase 2: Shop Module - Implementation Plan

> **Session**: 2026-01-21 (Updated: 2026-01-22)  
> **Status**: Approved, ready to implement

---

## Architecture

```
User ──owns──► Shop ──has──► ShopAddress (1:N)
  │              │
  │              └──has──► Product ──has──► Variant ──► Inventory
  │
  └──► Address (User's addresses - separate from Shop)

Domain.Core/ValueObjects/Address/  ◄── Shared VOs (Province, District, Ward, Street)
         ▲                    ▲
         │                    │
   Address (Auth)      ShopAddress (Shop)
```

---

## Shop Entity

```csharp
public class Shop : AggregateRoot
{
    public Guid OwnerId { get; }               // User owns this shop
    public ShopName Name { get; }
    public Slug Slug { get; }
    public ImageUrl? Logo { get; }
    public ImageUrl? Banner { get; }
    public Description? Description { get; }
    public ShopType Type { get; }              // OfficialStore | Reseller
    public ShopStatus Status { get; }          // Pending | Approved | Rejected | Suspended
    
    // Navigation
    public ICollection<ShopAddress> Addresses { get; }
    
    // Computed
    public bool IsOfficial => Type == ShopType.OfficialStore 
                           && Status == ShopStatus.Approved;
}
```

---

## ShopAddress Entity

```csharp
public class ShopAddress : Entity
{
    public Guid ShopId { get; }
    
    // Shared ValueObjects from Domain.Core
    public Phone Phone { get; }
    public FullName ContactName { get; }
    public Province Province { get; }
    public District District { get; }
    public Ward Ward { get; }
    public Street Street { get; }
    
    // Shop-specific
    public ShopAddressType Type { get; }       // Business | Warehouse | Pickup | Return
    public IsDefault IsDefault { get; }
    
    // Audit
    public Guid CreatedBy { get; }
    public Guid UpdatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
}

public enum ShopAddressType 
{ 
    Business,    // Địa chỉ đăng ký kinh doanh
    Warehouse,   // Kho hàng
    Pickup,      // Điểm lấy hàng
    Return       // Địa chỉ trả hàng
}
```

---

## Shared ValueObjects (Domain.Core)

Move từ `BeerStore.Domain/ValueObjects/Auth/Address/` → `Domain.Core/ValueObjects/Address/`

| ValueObject | Used By |
|-------------|---------|
| `Province` | Address, ShopAddress |
| `District` | Address, ShopAddress |
| `Ward` | Address, ShopAddress |
| `Street` | Address, ShopAddress |

> **DDD Pattern**: Shared Kernel - share ValueObjects, không share Entity

---

## Enums

```csharp
public enum ShopType 
{ 
    OfficialStore,  // Chính hãng, platform xác nhận
    Reseller        // Nhà phân phối, tự cam kết
}

public enum ShopStatus 
{ 
    Pending,        // Chờ duyệt
    Approved,       // Đã duyệt
    Rejected,       // Từ chối (có thể đăng ký lại)
    Suspended       // Tạm khóa (vi phạm)
}
```

---

## Value Objects (Shop-specific)

| Name | Validation |
|------|------------|
| `ShopName` | max 100, not empty |
| `Slug` | lowercase, alphanumeric + hyphen, unique |

---

## Registration Flow

```
User Register Shop (chọn Type) → Status = Pending
                                      │
                              Admin Review
                                      │
                    ┌─────────────────┼─────────────────┐
                    ▼                 ▼                 ▼
               Approved           Rejected          (vi phạm)
                                 (đăng ký lại)      Suspended
```

---

## Folder Structure

```
Shared/Domain.Core/
└── ValueObjects/
    └── Address/                    # 🆕 Shared VOs
        ├── Province.cs
        ├── District.cs
        ├── Ward.cs
        └── Street.cs

BeerStore.Domain/
├── Entities/Shop/
│   ├── Shop.cs
│   └── ShopAddress.cs              # 🆕 NEW
├── ValueObjects/Shop/
│   ├── ShopName.cs
│   ├── Slug.cs
│   └── Enums/ (ShopType, ShopStatus, ShopAddressType)
└── IRepository/Shop/
    ├── Read/
    │   ├── IRShopRepository.cs
    │   └── IRShopAddressRepository.cs
    └── Write/
        ├── IWShopRepository.cs
        └── IWShopAddressRepository.cs

BeerStore.Application/
├── Modules/Shop/
│   ├── Shops/
│   │   ├── Commands/ (RegisterShop, UpdateShop, ApproveShop, RejectShop, SuspendShop)
│   │   └── Queries/ (GetShopBySlug, GetShopById, GetMyShop, GetAllShops)
│   └── ShopAddresses/
│       ├── Commands/ (CreateShopAddress, UpdateShopAddress, RemoveShopAddress)
│       └── Queries/ (GetShopAddresses, GetShopAddressById)
├── DTOs/Shop/
│   ├── Shop/ (Requests/, Responses/)
│   └── ShopAddress/ (Requests/, Responses/)
└── Interface/IUnitOfWork/Shop/
    └── IShopUnitOfWork.cs

BeerStore.Infrastructure/
├── Repository/Shop/
│   ├── Read/ (RShopRepository, RShopAddressRepository)
│   └── Write/ (WShopRepository, WShopAddressRepository)
├── UnitOfWork/ShopUnitOfWork.cs
└── Persistence/Configuration/Shop/
    ├── ShopConfiguration.cs
    └── ShopAddressConfiguration.cs

BeerStore.Api/
└── Controllers/Shop/
    ├── ShopController.cs
    ├── ShopAddressController.cs
    └── ShopManagementController.cs
```

---

## Permissions

| Permission | Description |
|------------|-------------|
| `Shop.Read.All` | Xem tất cả shops |
| `Shop.Read.Self` | Xem shop của mình |
| `Shop.Create.All` | Đăng ký shop mới |
| `Shop.Update.Self` | Cập nhật shop của mình |
| `Shop.Approve.All` | Admin approve shop |
| `Shop.Reject.All` | Admin reject shop |
| `Shop.Suspend.All` | Admin suspend shop |
| `ShopAddress.Read.Self` | Xem địa chỉ shop của mình |
| `ShopAddress.Create.Self` | Tạo địa chỉ shop của mình |
| `ShopAddress.Update.Self` | Cập nhật địa chỉ shop của mình |
| `ShopAddress.Remove.Self` | Xóa địa chỉ shop của mình |

---

## Key Decisions

1. **ShopAddress entity riêng** - Không dùng Address của User (scalability, independence)
2. **Shared ValueObjects** - Province, District, Ward, Street move xuống Domain.Core (DDD Shared Kernel pattern)
3. **Shop 1:N ShopAddress** - Hỗ trợ multi-warehouse, multi-pickup points
4. **ShopAddressType enum** - Business, Warehouse, Pickup, Return
5. **ShopType** - OfficialStore (chính hãng) vs Reseller (nhà phân phối)
6. **Registration flow** - Pending → Admin review → Approved/Rejected

---

## Pre-requisite: Move Address ValueObjects to Core

Before implementing Shop module:

- [ ] Move `Province.cs` → `Domain.Core/ValueObjects/Address/`
- [ ] Move `District.cs` → `Domain.Core/ValueObjects/Address/`
- [ ] Move `Ward.cs` → `Domain.Core/ValueObjects/Address/`
- [ ] Move `Street.cs` → `Domain.Core/ValueObjects/Address/`
- [ ] Update `Address.cs` to use VOs from Core
- [ ] Update `AddressConfiguration.cs` converter imports

---

## Checklist

### Shop Entity
- [ ] Shop entity + Value Objects (ShopName, Slug)
- [ ] ShopType, ShopStatus enums
- [ ] IRShopRepository, IWShopRepository
- [ ] ShopConfiguration

### ShopAddress Entity
- [ ] ShopAddress entity
- [ ] ShopAddressType enum
- [ ] IRShopAddressRepository, IWShopAddressRepository
- [ ] ShopAddressConfiguration

### Application Layer
- [ ] IShopUnitOfWork
- [ ] RegisterShop command
- [ ] UpdateShop command
- [ ] ApproveShop, RejectShop, SuspendShop (Admin)
- [ ] GetShopBySlug, GetShopById, GetMyShop, GetAllShops queries
- [ ] ShopAddress CRUD commands/queries

### API Layer
- [ ] Permissions (Shop + ShopAddress)
- [ ] ShopController
- [ ] ShopAddressController
- [ ] ShopManagementController
