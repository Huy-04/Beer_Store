# Beer Store Platform - Kiến Trúc Module

> **Ngày tạo**: 2026-01-23  
> **Trạng thái**: Đang lập kế hoạch  
> **Loại**: Nền tảng E-commerce giống Shopee  
> **Kiến trúc**: Modular Monolith → Sẵn sàng chuyển Microservices

---

## 🏗️ Tổng Quan Kiến Trúc

### Nguyên tắc thiết kế (Sẵn sàng cho Microservice)

> **Tham khảo**: Microsoft eShop, microservices.io patterns

1. **Module = Bounded Context = Future Service** (DDD)
   - Mỗi module là 1 bounded context (ngữ cảnh giới hạn) độc lập
   - Sau này có thể tách thành microservice riêng khi cần mở rộng quy mô (scale)
   - **Ví dụ**: Module Order có thể tách thành Order Service chạy độc lập
   
2. **Database per Module** (Mỗi module có DB riêng)
   - Mỗi module có DbContext riêng biệt
   - Không join trực tiếp giữa các module (tránh phụ thuộc chặt)
   - **Lý do**: Khi tách microservice, mỗi service sẽ có database riêng
   
3. **Communication by ID only** (Giao tiếp chỉ qua ID)
   - Module giao tiếp qua ID, không dùng Foreign Key cứng
   - Sẵn sàng cho async messaging (Event Bus - hệ thống sự kiện bất đồng bộ)
   - **Ví dụ**: Order lưu `UserId` nhưng không có navigation property tới User
   
4. **Shared Kernel = Common Libraries** (Nhân chung = Thư viện dùng chung)
   - `*.Core` packages = shared libraries dùng chung giữa các module
   - ValueObjects, Base classes có thể đóng gói thành NuGet package
   - **Ví dụ**: `Domain.Core` chứa `Money`, `Address`, `Email` ValueObjects

5. **Single Responsibility** (Đơn trách nhiệm)
   - Mỗi service/module chỉ làm đúng 1 việc
   - Thay đổi 1 nghiệp vụ chỉ ảnh hưởng 1 module
   - **Ví dụ**: Thay đổi cách tính phí ship chỉ cần sửa Shipping module

---

## 🎯 Phân Tách Service (theo eShop pattern)

### Phân Loại Subdomain (Miền con)

> **Subdomain** là các vùng nghiệp vụ khác nhau trong hệ thống. Phân loại đúng giúp ưu tiên đầu tư và quyết định build hay buy.

| Loại | Module | Giải thích |
|------|--------|------------|
| **Core** (Lõi) | Product, Order, Payment | **Nghiệp vụ chính** - Tạo ra lợi thế cạnh tranh, cần tự build và đầu tư nhiều nhất |
| **Supporting** (Hỗ trợ) | Shop, Inventory, Shipping, Review | **Hỗ trợ nghiệp vụ chính** - Quan trọng nhưng không tạo lợi thế cạnh tranh |
| **Generic** (Chung) | Auth, Notification, Chat | **Chức năng chung** - Có thể dùng third-party service (Firebase, SendGrid, Twilio) |

---

## 📦 Service Map (Microservice View)

```
┌────────────────────────────────────────────────────────────────┐
│                    🌐 API GATEWAY                              │
│      (Routing, Authentication, Rate Limiting, Logging)         │
└───────────┬────────────────────────────────────────────────────┘
            │
   ┌────────┼────────┬─────────────┬─────────────┐
   ▼        ▼        ▼             ▼             ▼
┌──────┐ ┌──────┐ ┌──────┐     ┌──────┐     ┌──────┐
│AUTH  │ │SHOP  │ │PROD  │     │CART  │     │NOTIF │
│      │ │      │ │      │     │      │     │      │
│[DB]  │ │[DB]  │ │[DB]  │     │[Redis]     │[DB]  │
└──────┘ └──────┘ └──────┘     └──────┘     └──────┘
   │        │        │             │
   │        └────────┼─────────────┘
   │                 ▼
   │        ┌──────────────────┐
   │        │ INVENTORY [DB]   │
   │        │ CATALOG [Cache]  │
   │        └──────────────────┘
   │                 │
   │        ┌────────┴────────┐
   │        ▼                 ▼
   │     ┌──────┐         ┌──────┐
   │     │ORDER │         │REVIEW│
   │     │[DB]  │         │[DB]  │
   │     └──────┘         └──────┘
   │        │
   │   ┌────┼────┐
   │   ▼    ▼    ▼
   │ ┌──────┬────────┬─────────┐
   │ │PAY   │SHIP    │VOUCHER  │
   │ │[DB]  │[DB]    │[DB]     │
   │ └──────┴────────┴─────────┘
   │
   └──────► EVENT BUS (RabbitMQ/Kafka/Azure Bus)
            ├─ OrderCreated
            ├─ StockReserved
            ├─ PaymentCompleted
            ├─ OrderConfirmed
            ├─ ShipmentCreated
            └─ ReviewRequested

```

---

## 📋 Service Details (Module = Future Service)

| Entity | Mô tả |
|--------|-------|
| `User` | Tài khoản người dùng (email, password, phone, avatar) |
| `Role` | Vai trò (Admin, Seller, Buyer, Staff) |
| `Permission` | Quyền hạn (CreateProduct, ViewOrder, ManageShop) |
| `UserRole` | Bảng liên kết: User ↔ Role (1 user có nhiều role) |
| `RolePermission` | Bảng liên kết: Role ↔ Permission (1 role có nhiều permission) |
| `UserAddress` | Địa chỉ giao hàng của user (có thể có nhiều địa chỉ) |

**Trách nhiệm**: 
- **Authentication** (Xác thực): Đăng nhập, đăng ký, quên mật khẩu, JWT token
- **Authorization** (Phân quyền): Kiểm tra quyền truy cập resource
- **User Profile**: Quản lý thông tin cá nhân, địa chỉ

---

### 2. SHOP Module 🔄 In Progress

| Entity | Description |
|--------|-------------|
| `Store` | Thông tin shop (OwnerId → User.Id) |
| `StoreAddress` | Địa chỉ shop (Business, Warehouse, Pickup, Return) |
| `UserStore` | Junction: Nhân viên shop (Staff roles) |

**Trách nhiệm**: Shop profile, Shop staff management  
**Relation**: `Store.OwnerId` → `User.Id` (by ID, no FK)

**Status**:
- [x] Store entity
- [x] StoreAddress entity
- [x] Repositories (Read/Write)
- [x] DbContext, UnitOfWork, DI
- [ ] Application layer (Commands, Queries)
- [ ] API Controllers
- [ ] UserStore entity

---

### 3. PRODUCT Module ⏳ Planned

| Entity | Description |
|--------|-------------|
| `Product` | Sản phẩm chính (StoreId) |
| `ProductVariant` | Biến thể: Size M - Red (SKU, Price, Stock) |
| `Category` | Danh mục (hierarchical: parent-child) |
| `ProductCategory` | Junction: Product ↔ Category |
| `ProductAttribute` | Thuộc tính: Brand, Material, Origin |
| `ProductImage` | Hình ảnh sản phẩm |
| `ProductVideo` | Video sản phẩm (optional) |

**Trách nhiệm**: Catalog management, Product search  
**Relation**: `Product.StoreId` → `Store.Id` (by ID, no FK)

---

### 4. INVENTORY Module (Quản lý kho) ⏳ Planned

**Cách Shopee hoạt động**: 
- **Multi-warehouse** (Nhiều kho): Mỗi seller có kho riêng + Kho fulfillment của platform
- **Trạng thái tồn kho**: Không chỉ "còn hàng" mà chia nhỏ thành nhiều trạng thái
- **Reservation TTL** (Giữ hàng tạm thời): 10-30 phút, tự động trả lại nếu không mua
- **Trừ tồn theo từng bước**: Thêm giỏ → Đặt hàng → Ship → Giao (không trừ hết 1 lần)
- **Chống overselling** (Bán quá số lượng): Dùng database lock + Redis

| Entity | Mô tả |
|--------|-------|
| `Warehouse` | Kho hàng - Loại: Kho seller / Kho platform / Điểm lấy hàng |
| `ProductStock` | Tồn kho theo từng Variant + từng Warehouse |
| `StockReservation` | Giữ hàng khi thêm vào giỏ (tự động hết hạn sau 20 phút) |
| `InventoryTransaction` | Lịch sử xuất/nhập/điều chỉnh (không thể sửa/xóa - immutable) |
| `StockAlertRule` | Cảnh báo hết hàng (ngưỡng thấp, ngưỡng nguy hiểm, điểm đặt hàng lại) |

**Các trạng thái tồn kho** (quan trọng!):
- **OnHand** (Đang có): Số lượng thực tế trong kho vật lý
- **Reserved** (Đã giữ): Đang trong giỏ hàng của ai đó (chưa thanh toán)
- **Committed** (Đã cam kết): Đơn hàng đã thanh toán, đang chờ ship
- **Damaged** (Hư hỏng): Hàng lỗi, không bán được
- **SafetyStock** (Tồn an toàn): Lượng tồn tối thiểu, không bán hết
- **Available** (Có thể bán) = OnHand - Reserved - Committed - Damaged - SafetyStock

**Ví dụ cụ thể**:
```
Bia Tiger 330ml tại Kho HCM:
├─ OnHand: 1000 lon (thực tế trong kho)
├─ Reserved: 50 lon (đang trong giỏ hàng của 10 người)
├─ Committed: 200 lon (30 đơn hàng đang chờ ship)
├─ Damaged: 5 lon (lon móp)
├─ SafetyStock: 100 lon (luôn giữ lại)
└─ Available: 1000 - 50 - 200 - 5 - 100 = 645 lon (khách có thể mua)
```

**Trách nhiệm**: Quản lý tồn kho, Nhiều kho, Giữ hàng tạm thời, Chống bán quá số  
**Liên kết**: `ProductStock.VariantId` → `ProductVariant.Id` (chỉ qua ID)

---

### 5. CART Module (Giỏ hàng) ⏳ Planned

**Cách Shopee hoạt động**:
- **1 giỏ hàng duy nhất**, nhưng **nhóm theo shop** (để tính phí ship riêng)
- User có thể **chọn/bỏ chọn** từng shop hoặc từng item để checkout
- **Lưu 2 nơi**: Redis (nhanh, 30 phút) + Database (vĩnh viễn)
- **Lưu giá lúc thêm**, kiểm tra lại khi checkout (phát hiện thay đổi giá)
- **Theo dõi giỏ bị bỏ rơi** (30-90 ngày) để gửi email nhắc mua hàng

| Entity | Mô tả |
|--------|-------|
| `Cart` | Giỏ hàng (UserId, SessionId cho khách vãng lai, Trạng thái) |
| `CartItem` | Sản phẩm trong giỏ (ShopId, VariantId, Số lượng, Giá lúc thêm, Được chọn?) |
| `CartItemValidation` | Kết quả kiểm tra (Còn hàng?, Số lượng tồn, Giá đã thay đổi?) |

**Cấu trúc giỏ hàng (kiểu Shopee)**:
```
Giỏ hàng của User
├─ Shop A (Đã chọn: ✓)
│  ├─ Bia Lager 330ml (số lượng: 6)
│  └─ Bia IPA 500ml (số lượng: 2)
├─ Shop B (Đã chọn: ✓)
│  └─ Combo Craft Beer (số lượng: 1)
└─ Shop C (Đã chọn: ✗) ← Không checkout shop này
   └─ Bộ ly bia (số lượng: 1)
```

**Quy trình kiểm tra** (khi xem giỏ / checkout):
1. Lấy dữ liệu sản phẩm hiện tại (giá, tồn, trạng thái)
2. So sánh với giá lúc thêm → Cảnh báo GIÁ_TĂNG / GIÁ_GIẢM
3. Kiểm tra tồn kho → Cảnh báo HẾT_HÀNG hoặc GIẢM_SỐ_LƯỢNG
4. Trả về kết quả với thông báo rõ ràng

**Trách nhiệm**: Quản lý giỏ hàng, Nhóm theo shop, Kiểm tra giá, Theo dõi giỏ bị bỏ  
**Liên kết**: `Cart.UserId` → `User.Id`, `CartItem.VariantId` → `Variant.Id`, `CartItem.ShopId` → `Shop.Id`

---

### 6. ORDER Module (Đơn hàng) ⏳ Planned

**Cách Shopee hoạt động**:
- **1 checkout = N đơn hàng** (mỗi shop 1 đơn riêng để ship riêng)
- **Chụp giá** (Snapshot): Lưu giá, tên, hình sản phẩm tại thời điểm đặt (không đổi dù seller sửa sau)
- **Mã đơn**: `ORD-YYMMDD-XXXXXXXX` (sắp xếp theo ngày, dễ tìm)
- **Tự động hoàn tất**: Sau 7 ngày nếu buyer không xác nhận
- **Giữ tiền** (Escrow): Platform giữ tiền cho đến khi giao hàng thành công

| Entity | Mô tả |
|--------|-------|
| `Order` | Đơn hàng cha (UserId, Mã đơn, Tổng tiền, Trạng thái thanh toán) |
| `ShopOrder` | Đơn hàng con theo shop (ShopId, Mã tracking, Trạng thái riêng) |
| `OrderItem` | Chi tiết sản phẩm (VariantId, Số lượng, Giá, ProductSnapshot - không đổi) |
| `OrderTimeline` | Lịch sử trạng thái (Trạng thái, Ai thay đổi, Thời gian) |
| `OrderCancellation` | Lý do hủy (Ai hủy: Buyer/Seller/System, Lý do) |

**Quy trình tách đơn**:
```
Checkout (chọn 3 shop)
        │
        ▼
┌────────────────────────────────────────┐
│ Đơn cha: ORD-240123-A1B2C3D4             │
│ Tổng: 5.000.000đ                           │
├────────────────────────────────────────┤
│  ├─ Đơn con -01 (Shop A): 2.000.000đ      │
│  │   → Ship riêng, tracking riêng          │
│  ├─ Đơn con -02 (Shop B): 1.500.000đ      │
│  │   → Ship riêng, tracking riêng          │
│  └─ Đơn con -03 (Shop C): 1.500.000đ      │
│       → Ship riêng, tracking riêng          │
└────────────────────────────────────────┘
```

**Trách nhiệm**: Vòng đời đơn hàng, Tách đơn theo shop, Chụp giá, Theo dõi trạng thái  
**Trạng thái đơn hàng (theo từng ShopOrder)**:
```
Chờ thanh toán
    │
    ├─ Thanh toán thất bại → Đã hủy
    │
    ▼
Đã xác nhận → Đang xử lý → Sẵn sàng giao → Đang giao → Đã giao
    │           │                                 │         │
    │           └─ Seller hủy ────────────────┼─────────┤
    │                                             │         │
    └─ Buyer hủy ─────────────────────────────┘         ▼
                                                       Hoàn tất
                                                            │
                                                            ▼
                                                 Yêu cầu hoàn → Đã hoàn tiền
```

**Ai được hủy đơn khi nào?**:
| Trạng thái | Buyer hủy | Seller hủy | Hậu quả |
|------------|----------|-------------|----------|
| Chờ thanh toán | ✓ | ✓ | Không phạt |
| Đã xác nhận | ✓ | ✓* | Hoàn tiền 100% |
| Đang xử lý | ✓* | ✓* | Hoàn tiền 100% |
| Sẵn sàng giao | ✗ | ✓* | Seller bị trừ điểm |
| Đang giao | ✗ | ✗ | Phải giao hàng |

---

### 7. PAYMENT Module ⏳ Planned

**Shopee Operations**:
- Pre-payment (Cards, E-wallets) vs Post-payment (COD)
- Escrow system: Platform holds funds until delivery confirmed
- Multi-gateway support: PayMongo, Stripe, GrabPay, ShopeePay
- Refund to original method (5-14 days) or Platform wallet (instant)
- Transaction audit log for compliance

| Entity | Description |
|--------|-------------|
| `Payment` | Thanh toán (OrderId, Amount, Method, Status, GatewayReference) |
| `PaymentMethod` | Enum: COD, CreditCard, DebitCard, BankTransfer, ShopeePay, GrabPay |
| `PaymentTransaction` | Audit log (Type, Amount, GatewayEvent, IpAddress, Timestamp) |
| `Refund` | Hoàn tiền (PaymentId, Amount, Method: Original/Wallet, Status) |
| `EscrowTransaction` | Giữ tiền (ShopOrderId, Amount, Status: Held/Released/Refunded) |

**Escrow Flow**:
```
Order Paid → Funds HELD in Escrow
       │
       ▼
   Delivered
       │
       ├─ Buyer Confirms (Day 5) → Funds RELEASED to Seller
       │
       └─ Auto-Confirm (Day 10) → Funds RELEASED to Seller
                                         │
                                         ▼
                               Seller receives: Amount - Platform Fee (2%)
```

**Payment States**:
```
Pending → Processing → Completed
    │         │             │
    │         │             ├─ Refund Pending → Partially Refunded
    │         │             │                         │
    │         │             └─ Refund Pending → Refunded (Full)
    │         │
    │         └─ Failed (retry or cancel)
    │
    └─ Cancelled (user cancelled)
```

**Trách nhiệm**: Payment processing, Escrow, Multi-gateway integration, Refund handling

---

### 8. SHIPPING Module ⏳ Planned

**Shopee Operations**:
- Multi-carrier: GHN, GHTK, J&T, Ninja Van, Viettel Post
- Shipping fee = BaseFee + WeightSurcharge + ZoneSurcharge - Discount
- Weight: MAX(PhysicalWeight, VolumetricWeight) với Volumetric = L×W×H/5000
- Tracking webhook từ carriers (real-time status updates)
- Return shipping: Buyer/Seller/Platform pays tùy policy

| Entity | Description |
|--------|-------------|
| `ShippingCarrier` | Đơn vị vận chuyển (Code, ApiEndpoint, SupportsCOD, SupportsPickup) |
| `ShippingZone` | Vùng ship (Urban/Suburban/Remote/Island, Surcharge) |
| `ShippingRate` | Bảng phí (CarrierId, ServiceType, WeightRange, BasePrice, PricePerKg) |
| `Shipment` | Vận đơn (OrderId, CarrierId, TrackingNumber, Status, CODAmount) |
| `ShipmentTracking` | Lịch sử tracking (Status, Location, Timestamp, RawData) |
| `ReturnShipment` | Vận đơn hoàn (OriginalShipmentId, PayerType: Buyer/Seller/Platform) |

**Service Types**:
| Type | Surcharge | Delivery |
|------|-----------|----------|
| Economy | -20% | 5-7 days |
| Standard | 0% | 3-5 days |
| Express | +30% | 1-2 days |
| Same Day | +80% | Same day |

**Shipment Status Flow**:
```
Created → Pending Pickup → Picked Up → In Transit → At Hub → Out for Delivery
                                                            │
                                          ┌────────────────┼─────────────┐
                                          ▼                 │             ▼
                                     Delivered     Delivery Attempted   Failed (3x)
                                                   (retry next day)        │
                                                                           ▼
                                                                      Returning → Returned
```

**Trách nhiệm**: Carrier integration, Fee calculation, Tracking, Return handling

---

### 9. VOUCHER Module ⏳ Planned

**Shopee Operations**:
- Voucher types: Platform, Shop, Free Shipping, Cashback
- Stacking: Platform + Shop + Free Shipping (1 mỗi loại)
- Claim required cho một số voucher, auto-apply cho số khác
- Flash Deal khác Voucher: Áp dụng trực tiếp vào giá sản phẩm
- Fraud detection: Flag unusual patterns (same IP, bulk usage)

| Entity | Description |
|--------|-------------|
| `Voucher` | Mã giảm giá (Code, Type, DiscountType: Percentage/Fixed/FreeShip) |
| `VoucherRule` | Điều kiện (MinOrder, Categories, Products, TargetUserType, MaxUsage) |
| `VoucherClaim` | User claim voucher (VoucherId, UserId, ClaimedAt, IsUsed) |
| `VoucherUsage` | Lịch sử sử dụng (VoucherId, UserId, OrderId, DiscountAmount) |
| `FlashDeal` | Deal sốc (ProductId, DealPrice, TotalQty, SoldQty, TimeSlot) |
| `PromotionCampaign` | Sự kiện lớn (11.11, 12.12, VoucherIds, FlashDealIds) |

**Voucher Stacking Matrix**:
| Can Stack? | Platform | Shop | Free Ship | Coins |
|------------|----------|------|-----------|-------|
| Platform | ✗ | ✓ | ✓ | ✓ |
| Shop | ✓ | ✗ | ✓ | ✓ |
| Free Ship | ✓ | ✓ | ✗ | ✓ |
| Coins | ✓ | ✓ | ✓ | ✗ |

**Discount Calculation Order**:
```
1. Original Price: ₫500,000
2. Flash Deal: -₫50,000 → ₫450,000
3. Shop Voucher (10%): -₫45,000 → ₫405,000
4. Platform Voucher (₫30k off min ₫300k): -₫30,000 → ₫375,000
5. Free Shipping: Shipping ₫30k → ₫0
6. Final: ₫375,000
```

**Trách nhiệm**: Promotions, Discounts, Flash Deals, Campaign management

---

### 10. REVIEW Module ⏳ Planned

**Shopee Operations**:
- 1 review per OrderItem, chỉ sau khi Delivered
- Breakdown rating: Quality, Shipping, Seller Service (1-5 sao)
- Review window: 15-30 ngày sau delivery
- Coin rewards: Text +20, Photos +30-50, Video +100
- Moderation: Spam detection, profanity filter, NSFW image check

| Entity | Description |
|--------|-------------|
| `Review` | Đánh giá (OrderItemId, UserId, OverallRating, QualityRating, ShippingRating) |
| `ReviewMedia` | Hình ảnh/Video (MediaType, Url, ThumbnailUrl, Status) |
| `ReviewAttribute` | Rating dimensions per category (e.g., "Battery Life", "Taste") |
| `SellerResponse` | Shop reply (ReviewId, Content, ResponderId) |
| `ReviewLike` | Helpful votes (ReviewId, UserId) |
| `ReviewReport` | Report spam (ReportType, Reason, Status) |
| `ProductRatingAggregate` | Denormalized (AverageRating, TotalReviews, Rating5Count...) |
| `ShopRatingAggregate` | Shop rating (OverallRating, ResponseRate, ResponseTime) |

**Review Coin Rewards**:
| Action | Coins | Bonus |
|--------|-------|-------|
| Text review (50+ chars) | 20 | - |
| With 1-3 photos | +30 | - |
| With 4+ photos | +50 | - |
| With video (10-60s) | +100 | - |
| Detailed (200+ chars) | +20 | - |
| Within 48 hours | - | 1.5x |
| First review of product | - | 2x |

**Moderation Pipeline**:
```
Review Submitted
      │
      ▼
Automated Checks (Spam, Profanity, NSFW, Personal Info)
      │
      ├─ Pass → Published
      │
      └─ Flag → Manual Review Queue → Approve/Reject/Edit
```

**Trách nhiệm**: Product reviews, Ratings aggregation, Moderation, Coin incentives

---

### 11. CHAT Module ⏳ Planned

**Shopee Operations**:
- Real-time messaging via SignalR/WebSocket
- Message types: Text, Image, Product link, Order link, Sticker
- Auto-reply khi seller offline hoặc ngoài giờ làm việc
- Chat shortcuts (Quick replies) cho seller
- Typing indicators, read receipts, online status
- Message persistence: 1-2 năm, archive older to cold storage

| Entity | Description |
|--------|-------------|
| `Conversation` | Cuộc trò chuyện (ShopId, BuyerId, OrderId?, LastMessageAt, UnreadCount) |
| `Message` | Tin nhắn (ConversationId, SenderId, MessageType, Content, Status) |
| `MessageType` | Enum: Text, Image, Product, Order, Sticker, File, QuickReply |
| `ChatParticipant` | Thành viên (ConversationId, UserId, Role, LastReadMessageId, IsOnline) |
| `ChatAutoReply` | Auto reply (ShopId, TriggerType, Keywords, ReplyContent, BusinessHours) |
| `ChatShortcut` | Quick replies (ShopId, Shortcut: "/stock", Title, Content) |
| `ChatAttachment` | File đính kèm (MessageId, FileType, Url, ThumbnailUrl) |

**Message Delivery Lifecycle**:
```
Sending (Client) → Sent (Server) → Delivered (Recipient) → Read (Recipient)
    │                   │
    ▼                   ▼
  Failed              Recalled (Unsend)
```

**Real-time Architecture**:
```
┌────────────┐         ┌───────────────┐         ┌────────────┐
│  Client A  │◄───WS───►│ SignalR Hub   │◄───WS───►│  Client B  │
│  (Buyer)   │         │               │         │  (Seller)  │
└────────────┘         └───────┬───────┘         └────────────┘
                                │
                      ┌───────┴────────┐
                      │ Redis Backplane │ (for scale-out)
                      └────────────────┘
```

**Trách nhiệm**: Real-time messaging, Auto-reply, Quick replies, Attachment handling

---

### 12. NOTIFICATION Module ⏳ Planned

**Shopee Operations**:
- Multi-channel: In-App, Push (FCM/APNS), Email, SMS, Web Push
- Template system với variables: {{orderCode}}, {{trackingUrl}}
- User preferences: Enable/disable per channel + category
- Quiet hours: Không push trong giờ ngủ (queue for later)
- Priority levels: Critical (instant) → Low (daily digest)
- Rate limiting: Max 5 push/hour per user

| Entity | Description |
|--------|-------------|
| `Notification` | Thông báo (UserId, Type, Category, Title, Body, ActionUrl, IsRead) |
| `NotificationType` | Enum: Order, Promotion, Chat, System, Payment, Shipping, Review |
| `NotificationDelivery` | Delivery record (Channel, Status, ProviderMessageId, AttemptCount) |
| `NotificationTemplate` | Template (Code: ORDER_SHIPPED, TitleTemplate, BodyTemplate, Variables) |
| `UserNotificationPreference` | User settings (Channel, Category, IsEnabled, QuietHours) |
| `UserDevice` | Device tokens (DeviceToken, Platform: iOS/Android/Web, PushProvider) |
| `NotificationBatch` | Scheduled/Campaign notifications (TargetAudience, ScheduledAt) |

**Notification Categories & Channels**:
| Category | In-App | Push | Email | SMS |
|----------|--------|------|-------|-----|
| Order Updates | ✓ | ✓ | ✓ | Critical only |
| Payment | ✓ | ✓ | ✓ | ✓ |
| Chat Messages | ✓ | ✓ | ✗ | ✗ |
| Promotions | ✓ | Optional | Optional | ✗ |
| System Alerts | ✓ | ✓ | ✓ | Critical only |

**Priority Handling**:
| Priority | Channels | Timing | Example |
|----------|----------|--------|--------|
| Critical | Push + SMS + In-App | Immediate | Payment failed, Security alert |
| High | Push + In-App | Immediate | Order shipped, Chat message |
| Normal | Push + In-App | May batch (1-5 min) | Review request |
| Low | In-App only | Daily digest | Promotions |

**Multi-Channel Delivery Flow**:
```
Event Trigger (e.g., Order Shipped)
      │
      ▼
Notification Service
      │
      ├─ Check user preferences
      ├─ Check quiet hours
      ├─ Select enabled channels
      └─ Render templates with variables
      │
      ├─────────────┬─────────────┐
      ▼             ▼             ▼
   In-App      Push Queue    Email Queue
  (DB Insert)  (RabbitMQ)    (RabbitMQ)
                  │             │
                  ▼             ▼
              FCM/APNS      SendGrid
```

**Trách nhiệm**: Multi-channel delivery, Template management, User preferences, Rate limiting

---

## 🔗 Module Dependencies & Integration Map

```
                              ┌─────────┐
                              │  AUTH   │ (Identity & Access)
                              └────┬────┘
                                   │ UserId
         ┌─────────────────────────┼─────────────────────────┐
         ▼                         ▼                         ▼
    ┌─────────┐              ┌──────────┐              ┌──────────┐
    │  SHOP   │              │   CART   │              │   CHAT   │
    │         │              │ + Redis  │              │ + SignalR│
    └────┬────┘              └────┬─────┘              └──────────┘
         │ StoreId                │
         ▼                        │ VariantId
    ┌─────────┐                   │
    │ PRODUCT │◄──────────────────┘
    │ Catalog │
    └────┬────┘
         │ VariantId
    ┌────┴────┐
    ▼         ▼
┌─────────┐ ┌──────────┐              ┌──────────┐
│INVENTORY│ │  ORDER   │◄─────────────│ VOUCHER  │
│ + Stock │ │ + Escrow │              │ + Promos │
└─────────┘ └────┬─────┘              └──────────┘
                 │ OrderId
   ┌─────────────┼─────────────┬─────────────┐
   ▼             ▼             ▼             ▼
┌────────┐ ┌──────────┐  ┌────────┐   ┌──────────┐
│PAYMENT │ │ SHIPPING │  │ REVIEW │   │  NOTIF   │
│+ Escrow│ │ + Carrier│  │+ Rating│   │ + Push   │
└────────┘ └──────────┘  └────────┘   └──────────┘
```

**Key Integration Flows**:
```
1. Checkout Flow:
   Cart → Inventory (reserve) → Order → Payment → Inventory (commit)

2. Fulfillment Flow:
   Order Paid → Shipping Created → Carrier Tracking → Delivered → Review Eligible

3. Escrow Flow:
   Payment → Escrow Held → Order Delivered → Buyer Confirms → Escrow Released → Seller Paid

4. Stock Flow:
   Cart Add → Reserve (20 min TTL) → Checkout → Commit → Ship → Deduct OnHand

5. Notification Flow:
   [Any Event] → Notification Service → User Preferences → Push/Email/SMS
```

**Quy tắc**: Module chỉ reference ID của module khác, KHÔNG reference Entity trực tiếp

---

## � Inter-Service Communication Patterns

### Hiện tại (Modular Monolith)

```
┌─────────────────────────────────────────────────────────┐
│                    SINGLE PROCESS                        │
│  ┌─────────┐    ┌─────────┐    ┌─────────┐             │
│  │  Order  │───►│Inventory│───►│ Payment │             │
│  │ Module  │    │ Module  │    │ Module  │             │
│  └─────────┘    └─────────┘    └─────────┘             │
│       │              │              │                   │
│       └──────────────┴──────────────┘                   │
│                      ▼                                   │
│              ┌─────────────┐                            │
│              │  Database   │ (Shared, but separate      │
│              │  (SQL)      │  DbContexts per module)    │
│              └─────────────┘                            │
└─────────────────────────────────────────────────────────┘

Communication: Direct method call via UnitOfWork
Transaction: Shared DB transaction
```

### Tương lai (Microservices)

```
┌─────────────────────────────────────────────────────────┐
│                  SEPARATE PROCESSES                      │
│  ┌─────────┐    ┌─────────┐    ┌─────────┐             │
│  │  Order  │    │Inventory│    │ Payment │             │
│  │ Service │    │ Service │    │ Service │             │
│  └────┬────┘    └────┬────┘    └────┬────┘             │
│       │              │              │                   │
│       ▼              ▼              ▼                   │
│  ┌─────────┐    ┌─────────┐    ┌─────────┐             │
│  │OrderDB  │    │InvDB    │    │PayDB    │             │
│  └─────────┘    └─────────┘    └─────────┘             │
│       │              │              │                   │
│       └──────────────┴──────────────┘                   │
│                      ▼                                   │
│       ┌───────────────────────────────┐                 │
│       │    EVENT BUS (RabbitMQ)       │                 │
│       │  OrderCreated, StockReserved  │                 │
│       └───────────────────────────────┘                 │
└─────────────────────────────────────────────────────────┘

Communication: Async Events + Sync API calls
Transaction: Saga Pattern (Eventual Consistency)
```

---

## 📨 Domain Events (Chuẩn bị cho Event-Driven)

### Order Flow Events

```
1. OrderCreatedEvent           → Order placed, waiting for payment
2. OrderPaidEvent              → Payment successful, trigger: Reserve stock
3. OrderConfirmedEvent         → Seller accepted, trigger: Process & pack
4. OrderShippedEvent           → Handed to carrier, trigger: Tracking notifications
5. OrderDeliveredEvent         → Buyer received, trigger: Review request
6. OrderCompletedEvent         → Buyer confirmed / Auto-complete, trigger: Release escrow
7. OrderCancelledEvent         → Cancelled by buyer/seller/system, trigger: Release stock
8. RefundRequestedEvent        → Buyer requests refund
9. RefundApprovedEvent         → Refund processed, trigger: Payment refund
```

### Payment Events

```
1. PaymentInitiatedEvent       → User starts payment process
2. PaymentCompletedEvent       → Payment successful, trigger: Confirm order
3. PaymentFailedEvent          → Payment declined, trigger: Notify user
4. EscrowHeldEvent             → Funds locked in escrow
5. EscrowReleasedEvent         → Funds released to seller
6. RefundProcessedEvent        → Refund completed
```

### Shipping Events

```
1. ShipmentCreatedEvent        → Shipping label generated
2. ShipmentPickedUpEvent       → Carrier collected package
3. ShipmentInTransitEvent      → Package moving between hubs
4. ShipmentDeliveredEvent      → Successfully delivered
5. ShipmentFailedEvent         → Delivery failed (max attempts)
6. ShipmentReturnedEvent       → Package returned to seller
```

### Inventory Events

```
1. StockReservedEvent          → Stock deducted from Available (Cart add)
2. StockReservationExpiredEvent→ Stock returned if TTL passes
3. StockCommittedEvent         → Stock moved from Reserved to Committed (Order paid)
4. StockDeductedEvent          → Stock reduced from OnHand (Order shipped)
5. LowStockAlertEvent          → Trigger reorder (Stock < threshold)
6. StockDepletedEvent          → Product out of stock (OnHand = 0)
```

### Stock Deduction Flow (Order → Fulfillment)

```
Step 1: Customer Adds to Cart
├─ Check Available Stock ≥ Qty?
├─ YES: Create StockReservation (TTL: 20 min)
│  └─ Available -= Qty (Reserved += Qty)
├─ NO: Return 400 OUT_OF_STOCK
└─ Publish: StockReservedEvent

Step 2: Reservation Expires (if no purchase)
├─ Batch job every 5 minutes
├─ Find expired reservations (ExpiredAt < NOW)
├─ Available += Qty (Reserved -= Qty)
└─ Publish: StockReservationExpiredEvent

Step 3: Customer Checks Out (Payment confirmed)
├─ Find StockReservation
├─ Status = CONFIRMED (survives expiry now)
├─ Move: Reserved → Committed
├─ Publish: StockCommittedEvent
└─ Reserve stays locked until shipped

Step 4: Warehouse Picks & Ships
├─ Pick staff scans items from warehouse
├─ Deduct from OnHand
├─ OnHand -= Qty (Committed -= Qty)
├─ Publish: StockDeductedEvent
└─ Final state: OnHand reflects actual physical stock

Step 5: Delivery Confirmed
├─ Order marked as Delivered
├─ Inventory reconciled
└─ (OnHand already reduced at ship time)
```

### Multi-Warehouse Stock Routing

```
Order from Buyer in Region A
├─ Check warehouse priority:
│  1. Closest warehouse (Region A) - fastest delivery
│  2. Inventory level (prefer high stock)
│  3. Fulfillment SLA
├─ Warehouse A has Available stock?
│  ├─ YES: Reserve from Warehouse A ✅
│  ├─ NO: Check Warehouse B
│  │  ├─ Has stock? YES
│  │  └─ Create inter-warehouse transfer (2-3 days)
│  └─ NO: Backorder or offer drop-ship alternative
└─ Assign warehouse, deduct stock
```

### Overselling Prevention (High Concurrency)

```
Scenario: 100 customers try to buy last 5 units simultaneously

Layer 1: Database Transaction Lock (Pessimistic)
├─ SELECT ProductStock WHERE SKU = 'BEER-001' FOR UPDATE
├─ IF Available >= Qty THEN
│  ├─ Available -= Qty
│  └─ COMMIT
├─ ELSE
│  └─ ROLLBACK (409 CONFLICT)
└─ Result: Only 5 succeed, others fail gracefully

Layer 2: Redis Atomic Operation (High-speed)
├─ DECR inventory:BEER-001:available
├─ IF result < 0 THEN
│  ├─ INCR inventory:BEER-001:available
│  └─ Return 400 OUT_OF_STOCK
└─ ELSE Return 200 OK

Layer 3: Real-time Inventory Check
├─ Before each reservation, double-check
├─ Compare cache vs. DB (cache may be stale)
└─ Retry logic with exponential backoff
```



### Saga: Create Order

```
┌──────────┐    ┌───────────┐    ┌─────────┐    ┌──────────┐
│  ORDER   │    │INVENTORY  │    │ PAYMENT │    │ SHIPPING │
└────┬─────┘    └─────┬─────┘    └────┬────┘    └────┬─────┘
     │                │               │              │
     │ CreateOrder    │               │              │
     │ (Reserve Stock)│               │              │
     ├───────────────►│               │              │
     │                │ StockReserved │              │
     │                │ (20 min TTL)  │              │
     │                │               │              │
     │ Checkout       │               │              │
     │ (Payment)      │               │              │
     ├───────────────────────────────►│              │
     │                │               │ PaymentOK   │
     │                │               │◄─────────────┤
     │ Confirm Order  │               │              │
     │ (Commit Stock) │               │              │
     ├───────────────►│               │              │
     │                │ StockCommitted│              │
     │                │ (Permanent)   │              │
     │                │               │              │
     │ Pick & Ship    │               │              │
     ├────────────────────────────────────────────►│
     │                │               │              │ Create
     │                │ StockDeducted │              │ Shipment
     │                │ (OnHand ↓)    │              │
     │◄───────────────┴───────────────┴──────────────┤
     │         OrderCompleted                        │
```



---

## 🏗️ Migration Path: Monolith → Microservices

### Phase 1: Modular Monolith (Hiện tại)
```
✅ Separate DbContext per module
✅ Communication by ID only
✅ No cross-module Entity references
✅ Module-specific UnitOfWork
⏳ Domain Events (in-process)
```

### Phase 2: Extract First Service
```
Candidate: NOTIFICATION Service
Reason: 
- Ít dependency nhất
- Có thể async hoàn toàn
- Dễ test độc lập

Extract steps:
1. Tạo separate API project
2. Dùng RabbitMQ cho events
3. Deploy riêng
```

### Phase 3: Extract Core Services
```
Order: Catalog → Inventory → Ordering → Payment → Shipping
Each extraction:
1. Setup message queue
2. Implement Saga pattern
3. Add Circuit Breaker
4. Deploy + Monitor
```

---

## �📁 Folder Structure

```
BE/Src/Core/
├── BeerStore.Api/
│   └── Controllers/
│       ├── Auth/           # AuthController, UserController, RoleController
│       ├── Shop/           # StoreController, StoreAddressController
│       ├── Product/        # ProductController, CategoryController
│       ├── Cart/           # CartController
│       ├── Order/          # OrderController
│       └── ...
│
├── BeerStore.Application/
│   ├── Interface/
│   │   └── IUnitOfWork/
│   │       ├── Auth/       # IAuthUnitOfWork
│   │       ├── Shop/       # IShopUnitOfWork
│   │       └── Product/    # IProductUnitOfWork
│   └── Modules/
│       ├── Auth/           # User/, Role/, ...
│       ├── Shop/           # Store/, StoreAddress/
│       └── Product/        # Product/, Category/, Variant/
│
├── BeerStore.Domain/
│   ├── Entities/
│   │   ├── Auth/           # User, Role, Permission...
│   │   ├── Shop/           # Store, StoreAddress, UserStore
│   │   └── Product/        # Product, Variant, Category...
│   ├── ValueObjects/
│   │   ├── Auth/
│   │   ├── Shop/           # StoreName, Slug
│   │   └── Product/        # SKU, ProductName, Price
│   ├── Enums/
│   │   ├── Auth/
│   │   ├── Shop/
│   │   └── Product/
│   └── IRepository/
│       ├── Auth/
│       ├── Shop/
│       └── Product/
│
└── BeerStore.Infrastructure/
    ├── Persistence/
    │   ├── Db/
    │   │   ├── AuthDbContext.cs
    │   │   ├── ShopDbContext.cs
    │   │   └── ProductDbContext.cs
    │   └── EntityConfigurations/
    │       ├── Auth/
    │       ├── Shop/
    │       └── Product/
    ├── Repository/
    │   ├── Auth/
    │   ├── Shop/
    │   └── Product/
    ├── UnitOfWork/
    │   ├── AuthUnitOfWork.cs
    │   ├── ShopUnitOfWork.cs
    │   └── ProductUnitOfWork.cs
    └── DependencyInjection/
        ├── AuthDependencyInjection.cs
        ├── ShopDependencyInjection.cs
        └── ProductDependencyInjection.cs
```

---

## 🎯 Roadmap

| Phase | Module | Priority | Status | Description |
|-------|--------|----------|--------|-------------|
| 1 | Auth | Critical | ✅ Done | Authentication, User management |
| 2 | Shop | Critical | 🔄 In Progress | Store registration, Store profile |
| 3 | Product | High | ⏳ Planned | Catalog, Variants, Categories |
| 4 | Inventory | High | ⏳ Planned | Stock management |
| 5 | Cart | High | ⏳ Planned | Shopping cart |
| 6 | Order | High | ⏳ Planned | Order processing |
| 7 | Payment | Medium | ⏳ Planned | Payment gateway integration |
| 8 | Shipping | Medium | ⏳ Planned | Carrier integration |
| 9 | Review | Medium | ⏳ Planned | Product reviews |
| 10 | Voucher | Medium | ⏳ Planned | Promotions |
| 11 | Chat | Low | ⏳ Planned | Messaging |
| 12 | Notification | Low | ⏳ Planned | Push notifications |

---

## 📝 Next Steps

1. [ ] Hoàn thiện Shop Module (Application + API)
2. [ ] Thiết kế chi tiết Product Module
3. [ ] Tạo Inventory Module
4. [ ] Tạo Cart Module
5. [ ] Tạo Order Module

---

## 📚 References

- [AGENTS.md](docs/Project/AGENTS.md) - Golden Rules
- [Shop Module](docs/Project/Layer/modules/shop/README.md)
- [Auth Module](docs/Project/Layer/modules/auth/README.md)
- [SKILL.md](docs/Agent-Kit/skills/dotnet-patterns/SKILL.md) - Coding patterns
- [Microsoft eShop](https://github.com/dotnet/eShop) - Reference .NET Microservices
- [Microservices.io](https://microservices.io/patterns/) - Patterns Catalog

---

## ⚠️ Đánh Giá & Điều Chỉnh

### ✅ Điểm mạnh của Plan hiện tại

| Aspect | Status | Lý do |
|--------|--------|-------|
| Module = Bounded Context | ✅ Good | Mỗi module có thể tách thành service |
| Separate DbContext | ✅ Good | Database per service ready |
| Communication by ID | ✅ Good | Loose coupling, no FK |
| Shared Kernel (*.Core) | ✅ Good | Có thể publish NuGet package |

### ⚠️ Cần điều chỉnh

| Issue | Current | Should Be |
|-------|---------|-----------|
| Module naming | `Auth`, `Shop`, `Product` | Cân nhắc đổi thành **Service name** cho consistency |
| Naming convention | Mixed | Thống nhất: `Identity`, `Catalog`, `Ordering`, `Basket` (theo eShop) |
| Event system | Chưa có | Thêm Domain Events infrastructure |
| API versioning | Chưa có | Cần cho backward compatibility |

### 🔄 Đề xuất đổi tên Module (Optional)

| Current | eShop Style | Giữ nguyên? |
|---------|-------------|-------------|
| Auth | Identity | ✅ Giữ `Auth` (ngắn gọn hơn) |
| Shop | - | ✅ Giữ `Shop` (specific cho multi-vendor) |
| Product | Catalog | ⚠️ Có thể đổi thành `Catalog` |
| Cart | Basket | ⚠️ Có thể đổi thành `Basket` |
| Order | Ordering | ⚠️ Có thể đổi thành `Ordering` |

> **Quyết định**: Giữ nguyên naming hiện tại vì phù hợp với nghiệp vụ Shopee hơn.

---

## ✅ Checklist Microservice-Ready

### Per Module Checklist

- [ ] Separate DbContext (không share với module khác)
- [ ] Separate UnitOfWork
- [ ] Separate DependencyInjection
- [ ] Communication by ID only (không FK)
- [ ] Domain Events defined
- [ ] API versioning
- [ ] Health checks endpoint
- [ ] Logging correlation ID

### Infrastructure Checklist (Future)

- [ ] API Gateway (YARP / Ocelot)
- [ ] Service Discovery (Consul / K8s)
- [ ] Message Queue (RabbitMQ / Azure Service Bus)
- [ ] Distributed Cache (Redis)
- [ ] Centralized Logging (Seq / ELK)
- [ ] Distributed Tracing (OpenTelemetry)
- [ ] Circuit Breaker (Polly)
