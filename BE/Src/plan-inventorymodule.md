# Phase 4: Inventory Module - Implementation Plan

> **Session**: 2026-01-23  
> **Status**: Draft, pending approval  
> **Depends on**: Shop Module (Phase 2), Product Module (Phase 3)

---

## ⚠️ Naming Convention Reminder

> **Module ≠ Entity** - See `AGENTS.md`

| Concept | Name | Example |
|---------|------|--------|
| **Module** | `Inventory` | Folder: `Entities/Inventory/`, UoW: `IInventoryUnitOfWork` |
| **Entity** | `Inventory`, `StockMovement` | File: `Inventory.cs` |

---

## Architecture

```
Store ──────────────────────────────────────┐
  │                                        │
  └──► Product ──► ProductVariant          │
                        │                  │
                        ▼                  ▼
                   Inventory (StoreId + VariantId)
                        │
                        └──► StockMovement (audit trail)
```

---

## Why Separate Module?

| Reason | Explanation |
|--------|-------------|
| **Single Responsibility** | Product = info, Inventory = stock |
| **High Frequency Updates** | Every order updates inventory |
| **Complex Business Logic** | Reserved, Available, Movements |
| **Reporting** | Inventory reports separate from product reports |
| **Microservice Ready** | Can split to separate service later |
| **Differentiation** | Feature Shopee doesn't have |

---

## Inventory Entity

```csharp
public class Inventory : AggregateRoot
{
    public Guid StoreId { get; }                  // Which store owns this
    public Guid ProductVariantId { get; }         // Which variant
    
    // Stock levels
    public Quantity Available { get; private set; }   // Can be sold
    public Quantity Reserved { get; private set; }    // In cart/pending order
    
    // Computed
    public Quantity Total => Available + Reserved;
    public bool IsInStock => Available > 0;
    public bool IsLowStock => LowStockThreshold.HasValue 
                           && Available <= LowStockThreshold;
    
    // Settings
    public Quantity? LowStockThreshold { get; }   // Alert when below
    public bool TrackInventory { get; }           // false = unlimited stock
    
    // Audit
    public Guid CreatedBy { get; }
    public Guid UpdatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    
    // Methods
    public void AddStock(Quantity qty, MovementType type, string? reason, Guid userId);
    public void RemoveStock(Quantity qty, MovementType type, string? reason, Guid userId);
    public void Reserve(Quantity qty, string referenceNo, Guid userId);
    public void ReleaseReserve(Quantity qty, string referenceNo, Guid userId);
    public void CommitReserve(Quantity qty, string referenceNo, Guid userId);
    public void Adjust(Quantity newTotal, string reason, Guid userId);
}
```

---

## StockMovement Entity

```csharp
public class StockMovement : Entity
{
    public Guid InventoryId { get; }
    
    public MovementType Type { get; }
    public MovementDirection Direction { get; }   // In or Out
    public Quantity Quantity { get; }             // Always positive
    public Quantity BalanceBefore { get; }
    public Quantity BalanceAfter { get; }
    
    public string? Reason { get; }                // User-provided reason
    public string? ReferenceNo { get; }           // OrderId, PO number, etc.
    
    public Guid CreatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
}
```

---

## Enums

```csharp
public enum MovementType
{
    // === INBOUND (+) ===
    InitialStock,       // Tồn kho ban đầu
    PurchaseIn,         // Nhập hàng từ nhà cung cấp
    ReturnIn,           // Khách trả hàng
    AdjustmentIn,       // Điều chỉnh tăng (kiểm kê thừa)
    
    // === OUTBOUND (-) ===
    SaleOut,            // Bán hàng (commit từ reserve)
    DamagedOut,         // Hàng hỏng
    ExpiredOut,         // Hết hạn sử dụng
    LostOut,            // Mất mát, thất thoát
    AdjustmentOut,      // Điều chỉnh giảm (kiểm kê thiếu)
    
    // === RESERVE (internal) ===
    Reserved,           // Đặt trước (vào giỏ hàng)
    Released,           // Hủy đặt trước (bỏ giỏ hàng, cancel order)
    Committed           // Xác nhận bán (order confirmed)
}

public enum MovementDirection
{
    In,     // Tăng stock
    Out     // Giảm stock
}
```

---

## Value Objects

### Check Domain.Core First!
| Name | Validation | Notes |
|------|------------|-------|
| `Quantity` | >= 0, integer | Check `Domain.Core/ValueObjects/Base/QuantityBase.cs` first |

---

## Folder Structure

```
BeerStore.Domain/
├── Entities/Inventory/
│   ├── Inventory.cs
│   └── StockMovement.cs
├── Enums/
│   ├── MovementType.cs
│   └── MovementDirection.cs
└── IRepository/Inventory/
    ├── Read/
    │   ├── IRInventoryRepository.cs
    │   └── IRStockMovementRepository.cs
    └── Write/
        ├── IWInventoryRepository.cs
        └── IWStockMovementRepository.cs

BeerStore.Application/
├── Modules/Inventory/
│   ├── Inventories/
│   │   ├── Commands/
│   │   │   ├── CreateInventory/          # Khi tạo variant
│   │   │   ├── AddStock/                 # Nhập hàng
│   │   │   ├── RemoveStock/              # Xuất hàng (manual)
│   │   │   ├── AdjustStock/              # Kiểm kê điều chỉnh
│   │   │   ├── ReserveStock/             # Đặt trước (add to cart)
│   │   │   ├── ReleaseReserve/           # Hủy đặt trước
│   │   │   ├── CommitReserve/            # Xác nhận bán
│   │   │   └── UpdateSettings/           # LowStockThreshold, TrackInventory
│   │   └── Queries/
│   │       ├── GetInventoryByVariant/
│   │       ├── GetInventoriesByShop/
│   │       ├── GetLowStockItems/
│   │       └── GetOutOfStockItems/
│   └── StockMovements/
│       └── Queries/
│           ├── GetMovementsByInventory/
│           ├── GetMovementsByShop/
│           └── GetInventoryReport/       # Tổng hợp nhập/xuất
├── DTOs/Inventory/
│   ├── Inventory/ (Requests/, Responses/)
│   └── StockMovement/ (Responses/)
└── Interface/IUnitOfWork/Inventory/
    └── IInventoryUnitOfWork.cs

BeerStore.Infrastructure/
├── Repository/Inventory/
│   ├── Read/
│   │   ├── RInventoryRepository.cs
│   │   └── RStockMovementRepository.cs
│   └── Write/
│       ├── WInventoryRepository.cs
│       └── WStockMovementRepository.cs
├── UnitOfWork/InventoryUnitOfWork.cs
└── Persistence/Configuration/Inventory/
    ├── InventoryConfiguration.cs
    └── StockMovementConfiguration.cs

BeerStore.Api/
└── Controllers/Inventory/
    ├── InventoryController.cs
    └── StockMovementController.cs
```

---

## Permissions

| Permission | Description |
|------------|-------------|
| `Inventory.Read.All` | Xem tất cả inventory (Admin) |
| `Inventory.Read.Self` | Xem inventory của shop mình |
| `Inventory.Update.Self` | Cập nhật stock của shop mình |
| `Inventory.Adjust.Self` | Kiểm kê điều chỉnh |
| `StockMovement.Read.Self` | Xem lịch sử xuất nhập |
| `StockMovement.Read.All` | Xem tất cả movements (Admin) |

---

## API Endpoints

### Inventory Management (Seller)
```
GET    /api/stores/{storeId}/inventory                    # List all inventory
GET    /api/stores/{storeId}/inventory/low-stock          # Low stock items
GET    /api/stores/{storeId}/inventory/out-of-stock       # Out of stock items
GET    /api/stores/{storeId}/inventory/{variantId}        # Get by variant

POST   /api/stores/{storeId}/inventory/{variantId}/add    # Add stock (purchase in)
POST   /api/stores/{storeId}/inventory/{variantId}/remove # Remove stock (damaged, etc.)
POST   /api/stores/{storeId}/inventory/{variantId}/adjust # Adjust after count
PUT    /api/stores/{storeId}/inventory/{variantId}/settings # Update threshold
```

### Stock Movements (Seller)
```
GET    /api/stores/{storeId}/inventory/{variantId}/movements  # History
GET    /api/stores/{storeId}/inventory/report                 # Summary report
```

### Internal APIs (for Order module)
```
POST   /api/internal/inventory/reserve          # Reserve when add to cart
POST   /api/internal/inventory/release          # Release when remove from cart
POST   /api/internal/inventory/commit           # Commit when order confirmed
```

---

## Request/Response DTOs

### AddStock Request
```json
{
    "quantity": 100,
    "type": "PurchaseIn",
    "reason": "Nhập hàng từ NCC ABC",
    "referenceNo": "PO-2026-001"
}
```

### AdjustStock Request
```json
{
    "actualQuantity": 95,
    "reason": "Kiểm kê tháng 01/2026"
}
// System calculates: current=100, actual=95 → AdjustmentOut qty=5
```

### Inventory Response
```json
{
    "variantId": "...",
    "variantName": "Heineken 330ml",
    "productName": "Bia Heineken",
    "available": 95,
    "reserved": 5,
    "total": 100,
    "isLowStock": false,
    "lowStockThreshold": 20,
    "trackInventory": true,
    "lastUpdated": "2026-01-23T10:00:00Z"
}
```

### StockMovement Response
```json
{
    "id": "...",
    "type": "PurchaseIn",
    "direction": "In",
    "quantity": 100,
    "balanceBefore": 0,
    "balanceAfter": 100,
    "reason": "Nhập hàng từ NCC ABC",
    "referenceNo": "PO-2026-001",
    "createdBy": "seller@example.com",
    "createdAt": "2026-01-23T09:00:00Z"
}
```

---

## Reserve Flow (Cart → Order)

```
1. Add to Cart
   └── POST /internal/inventory/reserve
       └── Available: 100 → 98, Reserved: 0 → 2

2. Remove from Cart / Cancel
   └── POST /internal/inventory/release  
       └── Available: 98 → 100, Reserved: 2 → 0

3. Order Confirmed
   └── POST /internal/inventory/commit
       └── Reserved: 2 → 0 (stock permanently reduced)
       └── Create SaleOut movement
```

---

## Database Indexes

```sql
-- Inventory
CREATE UNIQUE INDEX IX_Inventory_StoreId_VariantId 
    ON Inventories(StoreId, ProductVariantId);
    
CREATE INDEX IX_Inventory_StoreId_Available 
    ON Inventories(StoreId, Available);  -- For low stock query

-- StockMovement
CREATE INDEX IX_Movement_InventoryId_CreatedAt 
    ON StockMovements(InventoryId, CreatedAt DESC);
    
CREATE INDEX IX_Movement_StoreId_CreatedAt 
    ON StockMovements(StoreId, CreatedAt DESC);  -- For reports
```

---

## Events

```csharp
public record StockLowEvent(Guid InventoryId, Guid StoreId, Quantity Available);
public record StockOutEvent(Guid InventoryId, Guid StoreId);
public record StockReplenishedEvent(Guid InventoryId, Guid StoreId, Quantity NewAvailable);
```

> Events can trigger notifications to seller

---

## Integration Points

| Module | Integration |
|--------|-------------|
| **Product** | When Variant created → Create Inventory record |
| **Product** | When Variant deleted → Soft delete Inventory |
| **Cart** | Add to cart → Reserve stock |
| **Cart** | Remove from cart → Release reserve |
| **Order** | Order confirmed → Commit reserve |
| **Order** | Order cancelled → Release reserve |
| **Order** | Return accepted → Add stock (ReturnIn) |

---

## Key Decisions

1. **Inventory per Variant** - Not per Product (granular control)
2. **Single inventory per Store-Variant** - No multi-warehouse for MVP
3. **Reserve mechanism** - Prevent overselling
4. **Movement audit trail** - Every change is tracked
5. **Soft operations** - No hard delete, use adjustments
6. **TrackInventory flag** - Support unlimited stock products
7. **Internal APIs** - Cart/Order modules call via internal endpoints
8. **StoreId (not ShopId)** - Entity naming follows Store convention

---

## Checklist

### Domain Layer
- [ ] Inventory entity with methods
- [ ] StockMovement entity
- [ ] MovementType, MovementDirection enums
- [ ] Domain events (StockLow, StockOut, etc.)
- [ ] Repository interfaces

### Infrastructure Layer
- [ ] EF Configurations with indexes
- [ ] Repository implementations
- [ ] InventoryUnitOfWork
- [ ] DI Registration

### Application Layer
- [ ] IInventoryUnitOfWork interface
- [ ] IInventoryAuthorizationService
- [ ] AddStock command
- [ ] RemoveStock command
- [ ] AdjustStock command
- [ ] UpdateInventorySettings command
- [ ] ReserveStock command (internal)
- [ ] ReleaseReserve command (internal)
- [ ] CommitReserve command (internal)
- [ ] Query handlers
- [ ] DTOs
- [ ] Mapping extensions

### API Layer
- [ ] Permissions seeding
- [ ] InventoryController
- [ ] StockMovementController
- [ ] InternalInventoryController

### Integration
- [ ] Hook: Variant created → Create Inventory
- [ ] Hook: Cart add → Reserve
- [ ] Hook: Order confirm → Commit

---

## 📦 Application Layer Plan

### Authorization Service

```csharp
public interface IInventoryAuthorizationService
{
    // Inventory
    void EnsureCanReadAllInventory();                     // Admin
    Task EnsureCanReadStoreInventory(Guid storeId);       // Store owner
    Task EnsureCanUpdateInventory(Guid inventoryId);      // Store owner
    Task EnsureCanAdjustInventory(Guid inventoryId);      // Store owner
    
    // StockMovement
    void EnsureCanReadAllMovements();                     // Admin
    Task EnsureCanReadStoreMovements(Guid storeId);       // Store owner
    
    // Internal (service-to-service)
    void EnsureIsInternalService();                       // For Cart/Order integration
}
```

---

### Inventory Commands

```csharp
// Create (auto when variant created)
public record CreateInventoryCommand(
    Guid StoreId,
    Guid ProductVariantId,
    int InitialStock,
    int? LowStockThreshold,
    bool TrackInventory,
    Guid CreatedBy) : IRequest<InventoryResponse>;

// Add Stock (purchase, return, etc.)
public record AddStockCommand(
    Guid InventoryId,
    AddStockRequest Request,
    Guid UpdatedBy) : IRequest<InventoryResponse>;

public record AddStockRequest(
    int Quantity,
    MovementType Type,          // PurchaseIn, ReturnIn, AdjustmentIn
    string? Reason,
    string? ReferenceNo);

// Remove Stock (damaged, expired, lost, etc.)
public record RemoveStockCommand(
    Guid InventoryId,
    RemoveStockRequest Request,
    Guid UpdatedBy) : IRequest<InventoryResponse>;

public record RemoveStockRequest(
    int Quantity,
    MovementType Type,          // DamagedOut, ExpiredOut, LostOut, AdjustmentOut
    string? Reason,
    string? ReferenceNo);

// Adjust Stock (kiểm kê)
public record AdjustStockCommand(
    Guid InventoryId,
    AdjustStockRequest Request,
    Guid UpdatedBy) : IRequest<InventoryResponse>;

public record AdjustStockRequest(
    int ActualQuantity,         // System calculates difference
    string Reason);

// Update Settings
public record UpdateInventorySettingsCommand(
    Guid InventoryId,
    UpdateInventorySettingsRequest Request,
    Guid UpdatedBy) : IRequest<InventoryResponse>;

public record UpdateInventorySettingsRequest(
    int? LowStockThreshold,
    bool TrackInventory);
```

---

### Internal Commands (Cart/Order)

```csharp
// Reserve stock when add to cart
public record ReserveStockCommand(
    Guid InventoryId,
    int Quantity,
    string ReferenceNo,         // CartId or OrderId
    Guid UserId) : IRequest<bool>;

// Release reserve when remove from cart or cancel order
public record ReleaseReserveCommand(
    Guid InventoryId,
    int Quantity,
    string ReferenceNo,
    Guid UserId) : IRequest<bool>;

// Commit reserve when order confirmed
public record CommitReserveCommand(
    Guid InventoryId,
    int Quantity,
    string ReferenceNo,         // OrderId
    Guid UserId) : IRequest<bool>;

// Bulk operations for cart with multiple items
public record BulkReserveStockCommand(
    List<ReserveStockItem> Items,
    string ReferenceNo,
    Guid UserId) : IRequest<BulkOperationResult>;

public record ReserveStockItem(Guid InventoryId, int Quantity);

public record BulkOperationResult(
    bool Success,
    List<FailedItem>? FailedItems);
```

---

### Inventory Queries

```csharp
// By Variant
public record GetInventoryByVariantQuery(
    Guid StoreId,
    Guid VariantId) : IRequest<InventoryResponse>;

// List by Store
public record GetInventoriesByStoreQuery(
    Guid StoreId,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<InventoryResponse>>;

// Low Stock Items
public record GetLowStockItemsQuery(
    Guid StoreId,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<InventoryResponse>>;

// Out of Stock Items
public record GetOutOfStockItemsQuery(
    Guid StoreId,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<InventoryResponse>>;

// Admin: All inventories
public record GetAllInventoriesQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? StoreId = null) : IRequest<PagedResult<InventoryResponse>>;
```

---

### StockMovement Queries

```csharp
// By Inventory
public record GetMovementsByInventoryQuery(
    Guid InventoryId,
    int Page = 1,
    int PageSize = 50,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null) : IRequest<PagedResult<StockMovementResponse>>;

// By Store
public record GetMovementsByStoreQuery(
    Guid StoreId,
    int Page = 1,
    int PageSize = 50,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    MovementType? Type = null) : IRequest<PagedResult<StockMovementResponse>>;

// Inventory Report
public record GetInventoryReportQuery(
    Guid StoreId,
    DateTimeOffset From,
    DateTimeOffset To) : IRequest<InventoryReportResponse>;
```

---

### DTOs

```csharp
// Inventory Response
public record InventoryResponse(
    Guid Id,
    Guid StoreId,
    Guid ProductVariantId,
    string VariantSku,
    string? VariantName,
    string ProductName,
    int Available,
    int Reserved,
    int Total,
    bool IsInStock,
    bool IsLowStock,
    int? LowStockThreshold,
    bool TrackInventory,
    DateTimeOffset UpdatedAt);

// StockMovement Response
public record StockMovementResponse(
    Guid Id,
    Guid InventoryId,
    MovementType Type,
    MovementDirection Direction,
    int Quantity,
    int BalanceBefore,
    int BalanceAfter,
    string? Reason,
    string? ReferenceNo,
    Guid CreatedBy,
    string CreatedByEmail,
    DateTimeOffset CreatedAt);

// Inventory Report Response
public record InventoryReportResponse(
    Guid StoreId,
    DateTimeOffset From,
    DateTimeOffset To,
    int TotalProducts,
    int TotalInStock,
    int TotalOutOfStock,
    int TotalLowStock,
    int TotalStockIn,
    int TotalStockOut,
    List<MovementSummary> MovementsByType);

public record MovementSummary(
    MovementType Type,
    int TotalQuantity,
    int TransactionCount);
```

---

## 🌐 API Layer Plan

### API Endpoints (Updated)

#### Inventory (Seller)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/stores/{storeId}/inventory` | List all inventory | ✅ |
| GET | `/api/stores/{storeId}/inventory/low-stock` | Low stock items | ✅ |
| GET | `/api/stores/{storeId}/inventory/out-of-stock` | Out of stock | ✅ |
| GET | `/api/stores/{storeId}/inventory/{variantId}` | Get by variant | ✅ |
| POST | `/api/stores/{storeId}/inventory/{variantId}/add` | Add stock | ✅ |
| POST | `/api/stores/{storeId}/inventory/{variantId}/remove` | Remove stock | ✅ |
| POST | `/api/stores/{storeId}/inventory/{variantId}/adjust` | Adjust stock | ✅ |
| PUT | `/api/stores/{storeId}/inventory/{variantId}/settings` | Update settings | ✅ |

#### Stock Movements (Seller)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/stores/{storeId}/inventory/{variantId}/movements` | Movement history | ✅ |
| GET | `/api/stores/{storeId}/inventory/movements` | All movements | ✅ |
| GET | `/api/stores/{storeId}/inventory/report` | Summary report | ✅ |

#### Internal APIs (Service-to-Service)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/internal/inventory/reserve` | Reserve stock | 🔑 API Key |
| POST | `/api/internal/inventory/release` | Release reserve | 🔑 API Key |
| POST | `/api/internal/inventory/commit` | Commit reserve | 🔑 API Key |
| POST | `/api/internal/inventory/bulk-reserve` | Bulk reserve | 🔑 API Key |

---

### Controllers

```csharp
// InventoryController - Seller
[ApiController]
[Route("api/stores/{storeId:guid}/inventory")]
[Authorize]
public class InventoryController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<InventoryResponse>>> GetAll(
        [FromRoute] Guid storeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken token = default);

    [HttpGet("low-stock")]
    public async Task<ActionResult<PagedResult<InventoryResponse>>> GetLowStock(...);

    [HttpGet("out-of-stock")]
    public async Task<ActionResult<PagedResult<InventoryResponse>>> GetOutOfStock(...);

    [HttpGet("{variantId:guid}")]
    public async Task<ActionResult<InventoryResponse>> GetByVariant(
        [FromRoute] Guid storeId,
        [FromRoute] Guid variantId,
        CancellationToken token);

    [HttpPost("{variantId:guid}/add")]
    public async Task<ActionResult<InventoryResponse>> AddStock(
        [FromRoute] Guid storeId,
        [FromRoute] Guid variantId,
        [FromBody] AddStockRequest request,
        CancellationToken token);

    [HttpPost("{variantId:guid}/remove")]
    public async Task<ActionResult<InventoryResponse>> RemoveStock(...);

    [HttpPost("{variantId:guid}/adjust")]
    public async Task<ActionResult<InventoryResponse>> AdjustStock(...);

    [HttpPut("{variantId:guid}/settings")]
    public async Task<ActionResult<InventoryResponse>> UpdateSettings(...);
}
```

---

```csharp
// StockMovementController - Seller
[ApiController]
[Route("api/stores/{storeId:guid}/inventory")]
[Authorize]
public class StockMovementController : BaseApiController
{
    [HttpGet("{variantId:guid}/movements")]
    public async Task<ActionResult<PagedResult<StockMovementResponse>>> GetByVariant(
        [FromRoute] Guid storeId,
        [FromRoute] Guid variantId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] DateTimeOffset? from = null,
        [FromQuery] DateTimeOffset? to = null,
        CancellationToken token = default);

    [HttpGet("movements")]
    public async Task<ActionResult<PagedResult<StockMovementResponse>>> GetAll(
        [FromRoute] Guid storeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] MovementType? type = null,
        CancellationToken token = default);

    [HttpGet("report")]
    public async Task<ActionResult<InventoryReportResponse>> GetReport(
        [FromRoute] Guid storeId,
        [FromQuery] DateTimeOffset from,
        [FromQuery] DateTimeOffset to,
        CancellationToken token);
}
```

---

```csharp
// InternalInventoryController - Service-to-Service
[ApiController]
[Route("api/internal/inventory")]
public class InternalInventoryController : BaseApiController
{
    // API Key authentication or internal network only
    
    [HttpPost("reserve")]
    public async Task<ActionResult<bool>> Reserve(
        [FromBody] ReserveStockRequest request,
        CancellationToken token);

    [HttpPost("release")]
    public async Task<ActionResult<bool>> Release(
        [FromBody] ReleaseReserveRequest request,
        CancellationToken token);

    [HttpPost("commit")]
    public async Task<ActionResult<bool>> Commit(
        [FromBody] CommitReserveRequest request,
        CancellationToken token);

    [HttpPost("bulk-reserve")]
    public async Task<ActionResult<BulkOperationResult>> BulkReserve(
        [FromBody] BulkReserveRequest request,
        CancellationToken token);
}

// Internal Request DTOs
public record ReserveStockRequest(Guid InventoryId, int Quantity, string ReferenceNo, Guid UserId);
public record ReleaseReserveRequest(Guid InventoryId, int Quantity, string ReferenceNo, Guid UserId);
public record CommitReserveRequest(Guid InventoryId, int Quantity, string ReferenceNo, Guid UserId);
public record BulkReserveRequest(List<ReserveStockItem> Items, string ReferenceNo, Guid UserId);
```

---

### Implementation Order

1. **Domain Layer** - Entities, Value Objects, Enums, Events, IRepository
2. **Infrastructure** - Configurations, Repositories, UnitOfWork, DI
3. **DTOs** - Requests & Responses
4. **Mapping** - RequestToEntity, EntityToResponse
5. **Authorization** - IInventoryAuthorizationService
6. **Inventory Commands** - Create → AddStock → RemoveStock → Adjust → Settings
7. **Internal Commands** - Reserve → Release → Commit → BulkReserve
8. **Queries** - ByVariant → ByStore → LowStock → OutOfStock → Movements → Report
9. **Controllers** - InventoryController → StockMovementController → InternalInventoryController
10. **Integration Hooks** - Variant created → Create Inventory
11. **Permissions Seeding**

---

## Comparison: Shopee vs Beer Store

| Feature | Shopee | Beer Store |
|---------|--------|------------|
| Update stock | ✅ Manual | ✅ Manual |
| Auto deduct on sale | ✅ Yes | ✅ Yes |
| Reserve mechanism | ✅ Yes | ✅ Yes |
| Movement history | ❌ No | ✅ **Yes** |
| Reason tracking | ❌ No | ✅ **Yes** |
| Stock adjustment | ❌ Just overwrite | ✅ **Audit trail** |
| Inventory report | ❌ Basic | ✅ **Detailed** |
| Low stock alert | ✅ Basic | ✅ Configurable |

---

## Future Enhancements (Post-MVP)

- [ ] Multi-warehouse support
- [ ] Batch/Lot tracking (expiry dates)
- [ ] Automatic reorder point
- [ ] Stock forecast
- [ ] Bulk import/export
- [ ] Integration with external ERP
