# BeerStore - Module Architecture Plan

> **Mục tiêu**: Thiết kế Modular Monolith để dễ dàng tách thành Microservices trong tương lai.

---

## 📁 Cấu trúc Modules (Bounded Contexts)

```
BeerStore.Application/
└── Modules/
    ├── Auth/                    # ✅ Identity & Access (đã có)
    │   ├── Authentication/      # Login, Register, , Logout
    │   ├── Users/               # User CRUD (Admin)
    │   ├── Roles/               # Role management
    │   ├── RefreshTokens/       # RefreshToken management
    │   └── Permissions/         # Permission management
    │
    ├── Catalog/                 # 📦 Product Catalog
    │   ├── Products/            # CRUD Products (Beer)
    │   ├── Categories/          # Beer categories (Lager, Ale, Stout...)
    │   ├── Brands/              # Beer brands
    │   └── Inventory/           # Stock management
    │
    ├── Ordering/                # 🛒 Order Management
    │   ├── Orders/              # Create, Update, Cancel Order
    │   ├── OrderItems/          # Line items
    │   └── OrderStatus/         # Status tracking (Pending → Confirmed → Shipped → Delivered)
    │
    ├── Basket/                  # 🧺 Shopping Cart
    │   ├── Carts/               # Cart management
    │   └── CartItems/           # Add/Remove/Update items
    │
    ├── Payment/                 # 💳 Payment Processing
    │   ├── Transactions/        # Payment records
    │   ├── Methods/             # Payment methods (COD, MoMo, ZaloPay, Card...)
    │   └── Refunds/             # Refund management
    │
    ├── Billing/                 # 🧾 Invoice & Receipt
    │   ├── Invoices/            # Generate invoices
    │   └── Bills/               # Bill records
    │
    ├── Shipping/                # 🚚 Delivery Management
    │   ├── Deliveries/          # Delivery tracking
    │   └── Carriers/            # Shipping partners (GHN, GHTK, J&T...)
    │
    └── Notification/            # 🔔 Notification Service
        ├── Emails/              # Email notifications
        ├── SMS/                 # SMS notifications
        └── Push/                # Push notifications
```

---

## 🎯 Bounded Context → Microservice Mapping

| Bounded Context | Mô tả | Microservice |
|-----------------|-------|--------------|
| **Auth** | Xác thực, phân quyền, quản lý user | `identity-service` |
| **Catalog** | Sản phẩm, danh mục, kho hàng | `catalog-service` |
| **Ordering** | Đơn hàng, trạng thái | `ordering-service` |
| **Basket** | Giỏ hàng | `basket-service` |
| **Payment** | Thanh toán | `payment-service` |
| **Billing** | Hóa đơn | `billing-service` |
| **Shipping** | Giao hàng | `shipping-service` |
| **Notification** | Thông báo | `notification-service` |

---

## 📂 Cấu trúc chi tiết mỗi Module

```
Modules/Catalog/
├── Products/
│   ├── Commands/
│   │   ├── CreateProduct/
│   │   │   ├── CreateProductCommand.cs
│   │   │   ├── CreateProductCHandler.cs
│   │   │   └── CreateProductValidator.cs
│   │   ├── UpdateProduct/
│   │   └── DeleteProduct/
│   └── Queries/
│       ├── GetProductById/
│       ├── GetProducts/
│       └── SearchProducts/
├── Categories/
│   └── ...
└── Inventory/
    └── ...
```


---

## 📋 Domain Entities dự kiến

### Catalog Module
- `Product` (Beer)
- `Category`
- `Brand`
- `InventoryItem`

### Ordering Module
- `Order`
- `OrderItem`
- `OrderStatus` (enum/value object)

### Basket Module
- `Cart`
- `CartItem`

### Payment Module
- `Transaction`
- `PaymentMethod`
- `Refund`

### Billing Module
- `Invoice`
- `Bill`

### Shipping Module
- `Delivery`
- `Carrier`

---


## 🚀 Roadmap Implementation

### Phase 1: Core (Đã hoàn thành ✅)
- [x] Auth Module (User, Role, Permission, RefreshToken)
- [x] Authentication (Login, Register, Refresh)

### Phase 2: Product
- [ ] Catalog Module
  - [ ] Products CRUD
  - [ ] Categories CRUD
  - [ ] Brands CRUD
  - [ ] Inventory

### Phase 3: Shopping
- [ ] Basket Module
- [ ] Ordering Module

### Phase 4: Payment & Billing
- [ ] Payment Module
- [ ] Billing Module

### Phase 5: Shipping & Notification
- [ ] Shipping Module
- [ ] Notification Module



new setup User Secrets

new Retry Policy

---

## 🔮 Future Improvements (từ Code Review)

### Security (Khi deploy Production)
- [ ] Setup User Secrets cho local development
- [ ] Move JWT SecretKey sang Azure Key Vault (production)
- [ ] Role-based Authorization (`[Authorize(Roles = "Admin")]`)
- [ ] Rate Limiting cho auth endpoints
- [ ] Restrict CORS policy theo environment
- [ ] Account lockout (track failed login attempts)

### Infrastructure (Khi cần)
- [ ] Retry Policy cho database operations (khi deploy cloud)
- [ ] Caching Layer (khi có performance issues)

### API (Tùy chọn)
- [ ] API Versioning (khi có external clients)
- [ ] POST Create trả về 201 thay vì 200
- [ ] Pagination cho list endpoints

### Database
- [ ] Run migration: `dotnet ef migrations add AddUserIndexes`