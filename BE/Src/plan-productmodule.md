# Phase 3: Product Module - Implementation Plan

> **Session**: 2026-01-23  
> **Status**: Draft, pending approval  
> **Depends on**: Shop Module (Phase 2)

---

## ⚠️ Naming Convention Reminder

> **Module ≠ Entity** - See `AGENTS.md`

| Concept | Name | Example |
|---------|------|--------|
| **Module** | `Product` | Folder: `Entities/Product/`, UoW: `IProductUnitOfWork` |
| **Entity** | `Product`, `ProductVariant`, `Category`, `Brand` | File: `Product.cs` |

---

## Architecture

```
Store ──has──► Product ──has──► ProductVariant
                │                    │
                │                    └──► Inventory (separate module)
                │
                └──► Category (N:N)
                └──► Brand (N:1)
                └──► ProductImage (1:N)
```

---

## Product Entity

```csharp
public class Product : AggregateRoot
{
    public Guid StoreId { get; }                // Belongs to which Store
    public Guid? BrandId { get; }               // Optional brand
    
    public ProductName Name { get; }
    public Slug Slug { get; }                   // Unique within Store
    public Description? Description { get; }   // From Domain.Core
    public ProductStatus Status { get; }
    
    // SEO
    public MetaTitle? MetaTitle { get; }
    public MetaDescription? MetaDescription { get; }
    
    // Pricing (base, variants may override)
    public Money BasePrice { get; }             // From Domain.Core
    public Money? CompareAtPrice { get; }       // Original price (for discount display)
    
    // Settings
    public bool HasVariants { get; }            // true = use variants, false = single product
    public bool IsDigital { get; }              // Digital product (no shipping)
    
    // Audit
    public Guid CreatedBy { get; }
    public Guid UpdatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    public DateTimeOffset? PublishedAt { get; }
}
```

---

## ProductVariant Entity

```csharp
public class ProductVariant : Entity
{
    public Guid ProductId { get; }
    
    public Sku Sku { get; }                     // Unique within Store
    public VariantName? Name { get; }           // "Đỏ - Size M", null if no variants
    
    // Pricing (overrides Product if set) - Use Money from Domain.Core
    public Money Price { get; }
    public Money? CompareAtPrice { get; }
    
    // Physical
    public Weight? Weight { get; }              // For shipping calculation
    
    // Options (e.g., Color: Red, Size: M)
    public VariantOption? Option1 { get; }      // { Name: "Color", Value: "Red" }
    public VariantOption? Option2 { get; }      // { Name: "Size", Value: "M" }
    public VariantOption? Option3 { get; }      // Max 3 options like Shopee
    
    // Status
    public bool IsActive { get; }
    
    // Audit
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
}
```

---

## Category Entity

```csharp
public class Category : AggregateRoot
{
    public Guid? ParentId { get; }              // Null = root category
    public CategoryName Name { get; }
    public Slug Slug { get; }                   // Global unique
    public Description? Description { get; }   // From Domain.Core
    public Img? Icon { get; }                   // From Domain.Core
    public int SortOrder { get; }
    public bool IsActive { get; }
    
    // Computed
    public int Level { get; }                   // 0 = root, 1 = child, etc.
    public string Path { get; }                 // "electronics/phones/smartphones"
}

// Junction table
public class ProductCategory : Entity
{
    public Guid ProductId { get; }
    public Guid CategoryId { get; }
    public bool IsPrimary { get; }              // Main category for display
}
```

---

## Brand Entity

```csharp
public class Brand : AggregateRoot
{
    public BrandName Name { get; }
    public Slug Slug { get; }                   // Global unique
    public Description? Description { get; }   // From Domain.Core
    public Img? Logo { get; }                   // From Domain.Core
    public bool IsActive { get; }
    
    // Verification
    public bool IsVerified { get; }             // Platform verified brand
}
```

---

## ProductImage Entity

```csharp
public class ProductImage : Entity
{
    public Guid ProductId { get; }
    public Guid? VariantId { get; }             // Null = product level, set = variant specific
    
    public Img Url { get; }                     // From Domain.Core
    public AltText? Alt { get; }
    public int SortOrder { get; }
    public bool IsPrimary { get; }              // Main image
}
```

---

## Enums

```csharp
public enum ProductStatus
{
    Draft,          // Đang soạn, chưa publish
    Active,         // Đang bán
    Inactive,       // Tạm ẩn (seller tự ẩn)
    OutOfStock,     // Hết hàng (auto từ Inventory)
    Deleted         // Soft delete
}
```

---

## Value Objects

### From Domain.Core (✅ Reuse)
| Name | Location | Purpose |
|------|----------|--------|
| `Money` | `Domain.Core/ValueObjects/Money.cs` | Pricing with Currency |
| `Description` | `Domain.Core/ValueObjects/Description.cs` | Product description |
| `Img` | `Domain.Core/ValueObjects/Img.cs` | Image URL |
| `Slug` | Already in Shop module | URL-friendly name |

### Product-specific (Create new)
| Name | Validation | Scope |
|------|------------|-------|
| `ProductName` | 1-200 chars, not empty | - |
| `Sku` | alphanumeric, max 50 | Unique within Store |
| `VariantName` | max 100 | - |
| `VariantOption` | { Name: string, Value: string } | - |
| `Weight` | > 0, grams | - |
| `CategoryName` | 1-100 chars | - |
| `BrandName` | 1-100 chars | - |
| `MetaTitle` | max 60 chars | SEO |
| `MetaDescription` | max 160 chars | SEO |
| `AltText` | max 125 chars | Accessibility |

---

## Folder Structure

```
BeerStore.Domain/
├── Entities/Product/
│   ├── Product.cs
│   ├── ProductVariant.cs
│   ├── ProductImage.cs
│   ├── ProductCategory.cs
│   ├── Category.cs
│   └── Brand.cs
├── ValueObjects/Product/
│   ├── ProductName.cs
│   ├── Sku.cs
│   ├── VariantName.cs
│   ├── VariantOption.cs
│   ├── CategoryName.cs
│   ├── BrandName.cs
│   ├── MetaTitle.cs
│   ├── MetaDescription.cs
│   ├── AltText.cs
│   └── Weight.cs
├── Enums/
│   └── ProductStatus.cs
└── IRepository/Product/
    ├── Read/
    │   ├── IRProductRepository.cs
    │   ├── IRProductVariantRepository.cs
    │   ├── IRCategoryRepository.cs
    │   └── IRBrandRepository.cs
    └── Write/
        ├── IWProductRepository.cs
        ├── IWProductVariantRepository.cs
        ├── IWCategoryRepository.cs
        └── IWBrandRepository.cs

BeerStore.Application/
├── Modules/Product/
│   ├── Products/
│   │   ├── Commands/
│   │   │   ├── CreateProduct/
│   │   │   ├── UpdateProduct/
│   │   │   ├── DeleteProduct/
│   │   │   ├── PublishProduct/
│   │   │   └── UnpublishProduct/
│   │   └── Queries/
│   │       ├── GetProductById/
│   │       ├── GetProductBySlug/
│   │       ├── GetProductsByShop/
│   │       ├── SearchProducts/
│   │       └── GetProductsForAdmin/
│   ├── Variants/
│   │   ├── Commands/ (Create, Update, Delete)
│   │   └── Queries/ (GetByProduct)
│   ├── Categories/
│   │   ├── Commands/ (Create, Update, Delete, Reorder)
│   │   └── Queries/ (GetTree, GetById, GetBySlug)
│   └── Brands/
│       ├── Commands/ (Create, Update, Delete, Verify)
│       └── Queries/ (GetAll, GetById, GetBySlug)
├── DTOs/Product/
│   ├── Product/ (Requests/, Responses/)
│   ├── Variant/ (Requests/, Responses/)
│   ├── Category/ (Requests/, Responses/)
│   └── Brand/ (Requests/, Responses/)
└── Interface/IUnitOfWork/Product/
    └── IProductUnitOfWork.cs

BeerStore.Infrastructure/
├── Repository/Product/
│   ├── Read/
│   └── Write/
├── UnitOfWork/ProductUnitOfWork.cs
└── Persistence/Configuration/Product/
    ├── ProductConfiguration.cs
    ├── ProductVariantConfiguration.cs
    ├── ProductImageConfiguration.cs
    ├── ProductCategoryConfiguration.cs
    ├── CategoryConfiguration.cs
    └── BrandConfiguration.cs

BeerStore.Api/
└── Controllers/Product/
    ├── ProductController.cs
    ├── VariantController.cs
    ├── CategoryController.cs
    └── BrandController.cs
```

---

## Permissions

| Permission | Description |
|------------|-------------|
| `Product.Read.All` | Xem tất cả products (Admin) |
| `Product.Read.Self` | Xem products của shop mình |
| `Product.Read.Public` | Xem products đang Active (Public) |
| `Product.Create.Self` | Tạo product cho shop mình |
| `Product.Update.Self` | Cập nhật product của mình |
| `Product.Delete.Self` | Xóa product của mình |
| `Product.Publish.Self` | Publish/Unpublish product |
| `Category.Read.All` | Xem categories |
| `Category.Manage.All` | CRUD categories (Admin) |
| `Brand.Read.All` | Xem brands |
| `Brand.Manage.All` | CRUD brands (Admin) |
| `Brand.Verify.All` | Verify brand (Admin) |

---

## API Endpoints

### Product (Seller)
```
POST   /api/shops/{shopId}/products           # Create product
GET    /api/shops/{shopId}/products           # List my products
GET    /api/shops/{shopId}/products/{id}      # Get product detail
PUT    /api/shops/{shopId}/products/{id}      # Update product
DELETE /api/shops/{shopId}/products/{id}      # Delete product
POST   /api/shops/{shopId}/products/{id}/publish
POST   /api/shops/{shopId}/products/{id}/unpublish
```

### Product (Public)
```
GET    /api/products                          # Search/filter products
GET    /api/products/{slug}                   # Get by slug (public view)
GET    /api/stores/{storeSlug}/products       # Products by store
```

### Variant
```
POST   /api/products/{productId}/variants
GET    /api/products/{productId}/variants
PUT    /api/products/{productId}/variants/{id}
DELETE /api/products/{productId}/variants/{id}
```

### Category (Admin)
```
GET    /api/categories                        # Get tree
GET    /api/categories/{slug}                 # Get by slug
POST   /api/admin/categories
PUT    /api/admin/categories/{id}
DELETE /api/admin/categories/{id}
```

### Brand (Admin)
```
GET    /api/brands
GET    /api/brands/{slug}
POST   /api/admin/brands
PUT    /api/admin/brands/{id}
DELETE /api/admin/brands/{id}
POST   /api/admin/brands/{id}/verify
```

---

## Key Decisions

1. **Product belongs to Store** - `StoreId` reference, no FK (microservice ready)
2. **Variant as Entity** - Not AggregateRoot, managed through Product
3. **Slug unique within Store** - Not global, allows same slug in different stores
4. **SKU unique within Store** - Same logic as Slug
5. **Max 3 variant options** - Like Shopee (Color, Size, etc.)
6. **Category as tree** - ParentId for hierarchy
7. **Brand optional** - Product may not have brand
8. **Inventory separate** - Stock managed in Inventory Module
9. **Money from Domain.Core** - Reuse Money ValueObject with Currency support
10. **Img from Domain.Core** - Reuse Img ValueObject

---

## Database Indexes

```sql
-- Product
CREATE UNIQUE INDEX IX_Product_StoreId_Slug ON Products(StoreId, Slug);
CREATE INDEX IX_Product_Status ON Products(Status);
CREATE INDEX IX_Product_CreatedAt ON Products(CreatedAt DESC);

-- Variant
CREATE UNIQUE INDEX IX_Variant_StoreId_Sku ON ProductVariants(StoreId, Sku);

-- Category
CREATE UNIQUE INDEX IX_Category_Slug ON Categories(Slug);
CREATE INDEX IX_Category_ParentId ON Categories(ParentId);

-- Brand
CREATE UNIQUE INDEX IX_Brand_Slug ON Brands(Slug);
```

---

## Checklist

### Domain Layer
- [ ] Product entity + Value Objects
- [ ] ProductVariant entity
- [ ] ProductImage entity
- [ ] ProductCategory junction
- [ ] Category entity (tree structure)
- [ ] Brand entity
- [ ] ProductStatus enum
- [ ] Repository interfaces (Read/Write)

### Infrastructure Layer
- [ ] EF Configurations with indexes
- [ ] Repository implementations
- [ ] ProductUnitOfWork
- [ ] DI Registration

### Application Layer
- [ ] IProductUnitOfWork interface
- [ ] IProductAuthorizationService
- [ ] Product Commands (Create, Update, Delete, Publish)
- [ ] Product Queries (ById, BySlug, ByStore, Search)
- [ ] Variant Commands/Queries
- [ ] Category Commands/Queries
- [ ] Brand Commands/Queries
- [ ] DTOs (Request/Response)
- [ ] Mapping extensions

### API Layer
- [ ] Permissions seeding
- [ ] ProductController
- [ ] VariantController
- [ ] CategoryController
- [ ] BrandController

### Testing
- [ ] Unit tests for Value Objects
- [ ] Unit tests for Entity methods
- [ ] Integration tests for repositories

---

## 📦 Application Layer Plan

### Authorization Service

```csharp
public interface IProductAuthorizationService
{
    // Product
    void EnsureCanReadAllProducts();                    // Admin
    Task EnsureCanReadOwnProducts(Guid storeId);        // Store owner
    void EnsureCanCreateProduct();
    Task EnsureCanUpdateProduct(Guid productId);        // Store owner
    Task EnsureCanDeleteProduct(Guid productId);        // Store owner
    Task EnsureCanPublishProduct(Guid productId);       // Store owner
    
    // Category
    void EnsureCanManageCategories();                   // Admin
    
    // Brand
    void EnsureCanManageBrands();                       // Admin
    void EnsureCanVerifyBrand();                        // Admin
}
```

---

### Product Commands

```csharp
// Create
public record CreateProductCommand(
    Guid CreatedBy,
    Guid UpdatedBy,
    Guid StoreId,
    CreateProductRequest Request) : IRequest<ProductResponse>;

public record CreateProductRequest(
    string Name,
    string Slug,
    string? Description,
    Guid? BrandId,
    List<Guid> CategoryIds,
    decimal BasePrice,
    string Currency,            // VND, USD
    decimal? CompareAtPrice,
    bool HasVariants,
    bool IsDigital,
    string? MetaTitle,
    string? MetaDescription,
    List<CreateProductImageRequest>? Images,
    List<CreateVariantRequest>? Variants);  // If HasVariants=true

// Update
public record UpdateProductCommand(
    Guid UpdatedBy,
    Guid ProductId,
    UpdateProductRequest Request) : IRequest<ProductResponse>;

// Delete (soft)
public record DeleteProductCommand(Guid ProductId) : IRequest<bool>;

// Publish/Unpublish
public record PublishProductCommand(Guid ProductId, Guid UpdatedBy) : IRequest<ProductResponse>;
public record UnpublishProductCommand(Guid ProductId, Guid UpdatedBy) : IRequest<ProductResponse>;
```

---

### Product Queries

```csharp
// Seller
public record GetProductByIdQuery(Guid CurrentUserId, Guid ProductId) : IRequest<ProductResponse>;
public record GetProductsByStoreQuery(
    Guid StoreId,
    int Page = 1,
    int PageSize = 20,
    ProductStatus? Status = null) : IRequest<PagedResult<ProductResponse>>;

// Public
public record GetProductBySlugQuery(string Slug) : IRequest<ProductPublicResponse>;
public record SearchProductsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    Guid? CategoryId = null,
    Guid? BrandId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? SortBy = null) : IRequest<PagedResult<ProductPublicResponse>>;

// Admin
public record GetAllProductsQuery(
    int Page = 1,
    int PageSize = 20,
    ProductStatus? Status = null) : IRequest<PagedResult<ProductResponse>>;
```

---

### Variant Commands/Queries

```csharp
// Commands
public record CreateVariantCommand(
    Guid ProductId,
    Guid CreatedBy,
    CreateVariantRequest Request) : IRequest<VariantResponse>;

public record UpdateVariantCommand(
    Guid VariantId,
    Guid UpdatedBy,
    UpdateVariantRequest Request) : IRequest<VariantResponse>;

public record DeleteVariantCommand(Guid VariantId) : IRequest<bool>;

// Queries
public record GetVariantsByProductQuery(Guid ProductId) : IRequest<IEnumerable<VariantResponse>>;

// Request DTOs
public record CreateVariantRequest(
    string Sku,
    string? Name,
    decimal Price,
    string Currency,
    decimal? CompareAtPrice,
    decimal? Weight,
    VariantOptionDto? Option1,
    VariantOptionDto? Option2,
    VariantOptionDto? Option3);

public record VariantOptionDto(string Name, string Value);
```

---

### Category Commands/Queries (Admin)

```csharp
// Commands
public record CreateCategoryCommand(CreateCategoryRequest Request) : IRequest<CategoryResponse>;
public record UpdateCategoryCommand(Guid CategoryId, UpdateCategoryRequest Request) : IRequest<CategoryResponse>;
public record DeleteCategoryCommand(Guid CategoryId) : IRequest<bool>;
public record ReorderCategoriesCommand(List<CategoryOrderDto> Orders) : IRequest<bool>;

// Queries
public record GetCategoryTreeQuery() : IRequest<IEnumerable<CategoryTreeResponse>>;
public record GetCategoryByIdQuery(Guid CategoryId) : IRequest<CategoryResponse>;
public record GetCategoryBySlugQuery(string Slug) : IRequest<CategoryResponse>;

// Request DTOs
public record CreateCategoryRequest(
    Guid? ParentId,
    string Name,
    string Slug,
    string? Description,
    string? Icon,
    int SortOrder);
```

---

### Brand Commands/Queries (Admin)

```csharp
// Commands
public record CreateBrandCommand(CreateBrandRequest Request) : IRequest<BrandResponse>;
public record UpdateBrandCommand(Guid BrandId, UpdateBrandRequest Request) : IRequest<BrandResponse>;
public record DeleteBrandCommand(Guid BrandId) : IRequest<bool>;
public record VerifyBrandCommand(Guid BrandId) : IRequest<BrandResponse>;

// Queries
public record GetAllBrandsQuery(int Page = 1, int PageSize = 50) : IRequest<PagedResult<BrandResponse>>;
public record GetBrandByIdQuery(Guid BrandId) : IRequest<BrandResponse>;
public record GetBrandBySlugQuery(string Slug) : IRequest<BrandResponse>;

// Request DTOs
public record CreateBrandRequest(
    string Name,
    string Slug,
    string? Description,
    string? Logo);
```

---

### DTOs

```csharp
// Product Responses
public record ProductResponse(
    Guid Id,
    Guid StoreId,
    Guid? BrandId,
    string Name,
    string Slug,
    string? Description,
    ProductStatus Status,
    decimal BasePrice,
    string Currency,
    decimal? CompareAtPrice,
    bool HasVariants,
    bool IsDigital,
    string? MetaTitle,
    string? MetaDescription,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? PublishedAt,
    List<VariantResponse>? Variants,
    List<ProductImageResponse>? Images,
    List<CategoryResponse>? Categories,
    BrandResponse? Brand);

public record ProductPublicResponse(
    Guid Id,
    string StoreName,
    string StoreSlug,
    string Name,
    string Slug,
    string? Description,
    decimal BasePrice,
    string Currency,
    decimal? CompareAtPrice,
    bool HasVariants,
    string? PrimaryImageUrl,
    BrandResponse? Brand);

// Variant Response
public record VariantResponse(
    Guid Id,
    string Sku,
    string? Name,
    decimal Price,
    string Currency,
    decimal? CompareAtPrice,
    decimal? Weight,
    bool IsActive,
    VariantOptionDto? Option1,
    VariantOptionDto? Option2,
    VariantOptionDto? Option3);

// Category Response
public record CategoryResponse(
    Guid Id,
    Guid? ParentId,
    string Name,
    string Slug,
    string? Description,
    string? Icon,
    int SortOrder,
    int Level,
    string Path,
    bool IsActive);

public record CategoryTreeResponse(
    Guid Id,
    string Name,
    string Slug,
    int SortOrder,
    List<CategoryTreeResponse> Children);

// Brand Response
public record BrandResponse(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string? Logo,
    bool IsVerified,
    bool IsActive);
```

---

## 🌐 API Layer Plan

### API Endpoints (Updated)

#### Product (Seller)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/stores/{storeId}/products` | Create product | ✅ |
| GET | `/api/stores/{storeId}/products` | List my products | ✅ |
| GET | `/api/stores/{storeId}/products/{id}` | Get product detail | ✅ |
| PUT | `/api/stores/{storeId}/products/{id}` | Update product | ✅ |
| DELETE | `/api/stores/{storeId}/products/{id}` | Delete product | ✅ |
| POST | `/api/stores/{storeId}/products/{id}/publish` | Publish | ✅ |
| POST | `/api/stores/{storeId}/products/{id}/unpublish` | Unpublish | ✅ |

#### Product (Public)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/products` | Search products | ❌ |
| GET | `/api/products/{slug}` | Get by slug | ❌ |
| GET | `/api/stores/{storeSlug}/products` | Products by store | ❌ |

#### Variant (Seller)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/products/{productId}/variants` | List variants | ✅ |
| POST | `/api/products/{productId}/variants` | Create variant | ✅ |
| PUT | `/api/products/{productId}/variants/{id}` | Update variant | ✅ |
| DELETE | `/api/products/{productId}/variants/{id}` | Delete variant | ✅ |

#### Category (Public + Admin)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/categories` | Get tree | ❌ |
| GET | `/api/categories/{slug}` | Get by slug | ❌ |
| POST | `/api/admin/categories` | Create | ✅ Admin |
| PUT | `/api/admin/categories/{id}` | Update | ✅ Admin |
| DELETE | `/api/admin/categories/{id}` | Delete | ✅ Admin |

#### Brand (Public + Admin)
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/brands` | List brands | ❌ |
| GET | `/api/brands/{slug}` | Get by slug | ❌ |
| POST | `/api/admin/brands` | Create | ✅ Admin |
| PUT | `/api/admin/brands/{id}` | Update | ✅ Admin |
| DELETE | `/api/admin/brands/{id}` | Delete | ✅ Admin |
| POST | `/api/admin/brands/{id}/verify` | Verify | ✅ Admin |

---

### Controllers

```csharp
// ProductController - Seller + Public
[ApiController]
[Route("api")]
public class ProductController : BaseApiController
{
    // === SELLER (Protected) ===
    [HttpPost("stores/{storeId:guid}/products")]
    [Authorize]
    public async Task<ActionResult<ProductResponse>> Create(...);

    [HttpGet("stores/{storeId:guid}/products")]
    [Authorize]
    public async Task<ActionResult<PagedResult<ProductResponse>>> GetByStore(...);

    // === PUBLIC ===
    [HttpGet("products")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<ProductPublicResponse>>> Search(...);

    [HttpGet("products/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductPublicResponse>> GetBySlug(...);
}

// VariantController - Seller only
[ApiController]
[Route("api/products/{productId:guid}/variants")]
[Authorize]
public class VariantController : BaseApiController { }

// CategoryController - Public + Admin
[ApiController]
[Route("api")]
public class CategoryController : BaseApiController
{
    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoryTreeResponse>>> GetTree(...);

    [HttpPost("admin/categories")]
    [Authorize]
    public async Task<ActionResult<CategoryResponse>> Create(...);
}

// BrandController - Public + Admin
[ApiController]
[Route("api")]
public class BrandController : BaseApiController { }
```

---

### Implementation Order

1. **Domain Layer** - Entities, Value Objects, Enums, IRepository
2. **Infrastructure** - Configurations, Repositories, UnitOfWork, DI
3. **DTOs** - Requests & Responses
4. **Mapping** - RequestToEntity, EntityToResponse
5. **Authorization** - IProductAuthorizationService
6. **Commands** - Product → Variant → Category → Brand
7. **Queries** - Product → Variant → Category → Brand
8. **Controllers** - ProductController → VariantController → CategoryController → BrandController
9. **Permissions Seeding**

---

## Notes

- **Inventory NOT in this module** - See `plan-inventorymodule.md`
- **Search/Filter** - Consider Elasticsearch for production scale
- **Images** - Store URL only, actual upload handled by separate service
