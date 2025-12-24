# Beer Store API Documentation

## Tổng Quan
API quản lý cửa hàng bia xây dựng với ASP.NET Core, sử dụng mô hình CQRS (Command Query Responsibility Segregation) với MediatR. Hệ thống quản lý xác thực, phân quyền và quản lý địa chỉ người dùng.

## Kiến Trúc API

### Base Controller
Tất cả các controller được xác thực đều kế thừa từ `BaseApiController`, cung cấp các thuộc tính tiện ích:
- `CurrentUserId`: Lấy ID người dùng từ JWT token
- `CurrentUserEmail`: Lấy email người dùng từ JWT claims
- `CurrentUserRoles`: Lấy danh sách roles của người dùng từ JWT claims

### Xác Thực
Phần lớn endpoints yêu cầu xác thực (Authorization) ngoại trừ các endpoint đăng nhập/đăng ký.

---

## API Endpoints

### 1. Authentication Controller (`/api/Auth`)
Quản lý đăng nhập và đăng ký người dùng.

#### POST `/api/Auth/login`
Đăng nhập người dùng
- **Yêu cầu xác thực**: Không
- **Body**:
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- **Phản hồi**: Token JWT và thông tin người dùng

#### POST `/api/Auth/register`
Đăng ký người dùng mới
- **Yêu cầu xác thực**: Không
- **Body**: CreateUserRequest (email, password, tên, số điện thoại, v.v.)
- **Phản hồi**: Thông tin người dùng đã đăng ký

---

### 2. User Controller (`/api/User`)
Quản lý thông tin người dùng. **Yêu cầu xác thực**.

#### GET `/api/User`
Lấy danh sách tất cả người dùng
- **Phản hồi**: Mảng UserResponse

#### GET `/api/User/{id}`
Lấy thông tin người dùng theo ID
- **Tham số**: `id` (Guid)
- **Phản hồi**: UserResponse

#### GET `/api/User/email/{email}`
Tìm người dùng theo email
- **Tham số**: `email` (string)
- **Phản hồi**: UserResponse

#### GET `/api/User/username/{username}`
Tìm người dùng theo tên đăng nhập
- **Tham số**: `username` (string)
- **Phản hồi**: UserResponse

#### GET `/api/User/phone/{phone}`
Tìm người dùng theo số điện thoại
- **Tham số**: `phone` (string)
- **Phản hồi**: UserResponse

#### GET `/api/User/emailstatus/{status}`
Lấy danh sách người dùng theo trạng thái email
- **Tham số**: `status` (StatusEnum)
- **Phản hồi**: Mảng UserResponse

#### GET `/api/User/phonestatus/{status}`
Lấy danh sách người dùng theo trạng thái số điện thoại
- **Tham số**: `status` (StatusEnum)
- **Phản hồi**: Mảng UserResponse

#### GET `/api/User/userstatus/{status}`
Lấy danh sách người dùng theo trạng thái tài khoản
- **Tham số**: `status` (StatusEnum)
- **Phản hồi**: Mảng UserResponse

#### POST `/api/User`
Tạo người dùng mới
- **Body**: CreateUserRequest
- **Phản hồi**: UserResponse (201 Created)

#### PUT `/api/User/{id}`
Cập nhật thông tin người dùng
- **Tham số**: `id` (Guid)
- **Body**: UpdateUserRequest
- **Phản hồi**: UserResponse

#### DELETE `/api/User/{id}`
Xóa người dùng
- **Tham số**: `id` (Guid)
- **Phản hồi**: 204 No Content

---

### 3. Role Controller (`/api/Role`)
Quản lý vai trò (roles). **Yêu cầu xác thực**.

#### GET `/api/Role`
Lấy danh sách tất cả vai trò
- **Phản hồi**: Mảng RoleResponse

#### GET `/api/Role/{id}`
Lấy chi tiết vai trò theo ID
- **Tham số**: `id` (Guid)
- **Phản hồi**: RoleResponse

#### POST `/api/Role`
Tạo vai trò mới
- **Body**: RoleRequest (tên vai trò, mô tả, v.v.)
- **Phản hồi**: RoleResponse (201 Created)

#### PUT `/api/Role/{id}`
Cập nhật vai trò
- **Tham số**: `id` (Guid)
- **Body**: RoleRequest
- **Phản hồi**: RoleResponse

#### DELETE `/api/Role/{id}`
Xóa vai trò
- **Tham số**: `id` (Guid)
- **Phản hồi**: 204 No Content

---

### 4. Permission Controller (`/api/Permission`)
Quản lý quyền (permissions). **Yêu cầu xác thực**.

#### GET `/api/Permission`
Lấy danh sách tất cả quyền
- **Phản hồi**: Mảng PermissionResponse

#### GET `/api/Permission/{id}`
Lấy chi tiết quyền theo ID
- **Tham số**: `id` (Guid)
- **Phản hồi**: PermissionResponse

#### GET `/api/Permission/module/{module}`
Lấy danh sách quyền theo module
- **Tham số**: `module` (ModuleEnum)
- **Phản hồi**: Mảng PermissionResponse

#### POST `/api/Permission`
Tạo quyền mới
- **Body**: PermissionRequest (tên quyền, module, hành động, v.v.)
- **Phản hồi**: PermissionResponse (201 Created)

#### PUT `/api/Permission/{id}`
Cập nhật quyền
- **Tham số**: `id` (Guid)
- **Body**: PermissionRequest
- **Phản hồi**: PermissionResponse

#### DELETE `/api/Permission/{id}`
Xóa quyền
- **Tham số**: `id` (Guid)
- **Phản hồi**: 204 No Content

---

### 5. Address Controller (`/api/Address`)
Quản lý địa chỉ. **Yêu cầu xác thực**.

#### GET `/api/Address`
Lấy danh sách tất cả địa chỉ
- **Phản hồi**: Mảng AddressResponse

#### GET `/api/Address/{id}`
Lấy chi tiết địa chỉ theo ID
- **Tham số**: `id` (Guid)
- **Phản hồi**: AddressResponse

#### GET `/api/Address/phone/{phone}`
Tìm địa chỉ theo số điện thoại
- **Tham số**: `phone` (string)
- **Phản hồi**: Mảng AddressResponse

#### POST `/api/Address`
Tạo địa chỉ mới
- **Body**: AddressRequest (đường phố, thành phố, quốc gia, v.v.)
- **Phản hồi**: AddressResponse (201 Created)

#### PUT `/api/Address/{id}`
Cập nhật địa chỉ
- **Tham số**: `id` (Guid)
- **Body**: AddressRequest
- **Phản hồi**: AddressResponse

#### DELETE `/api/Address/{id}`
Xóa địa chỉ
- **Tham số**: `id` (Guid)
- **Phản hồi**: 204 No Content

---

### 6. Role Permission Controller (`/api/RolePermission`)
Quản lý mối quan hệ giữa vai trò và quyền. **Yêu cầu xác thực**.

#### GET `/api/RolePermission`
Lấy danh sách tất cả mối quan hệ Role-Permission
- **Phản hồi**: Mảng RolePermissionResponse

#### GET `/api/RolePermission/role/{roleId}`
Lấy tất cả quyền của một vai trò
- **Tham số**: `roleId` (Guid)
- **Phản hồi**: Mảng RolePermissionResponse

#### POST `/api/RolePermission`
Gán quyền cho vai trò
- **Body**: 
  ```json
  {
    "roleId": "guid",
    "permissionId": "guid"
  }
  ```
- **Phản hồi**: RolePermissionResponse (201 Created)

#### DELETE `/api/RolePermission/{id}`
Xóa quyền khỏi vai trò
- **Tham số**: `id` (Guid)
- **Phản hồi**: 204 No Content

---

### 7. User Role Controller (`/api/UserRole`)
Quản lý mối quan hệ giữa người dùng và vai trò. **Yêu cầu xác thực**.

#### GET `/api/UserRole`
Lấy danh sách tất cả mối quan hệ User-Role
- **Phản hồi**: Mảng UserRoleResponse

#### GET `/api/UserRole/user/{userId}`
Lấy tất cả vai trò của một người dùng
- **Tham số**: `userId` (Guid)
- **Phản hồi**: Mảng UserRoleResponse

#### POST `/api/UserRole`
Gán vai trò cho người dùng
- **Body**: 
  ```json
  {
    "userId": "guid",
    "roleId": "guid"
  }
  ```
- **Phản hồi**: UserRoleResponse (201 Created)

#### DELETE `/api/UserRole/{id}`
Xóa vai trò khỏi người dùng
- **Tham số**: `id` (Guid)
- **Phản hồi**: 204 No Content

---

### 8. User Address Controller (`/api/UserAddress`)
Quản lý mối quan hệ giữa người dùng và địa chỉ. **Yêu cầu xác thực**.

#### GET `/api/UserAddress`
Lấy danh sách tất cả mối quan hệ User-Address
- **Phản hồi**: Mảng UserAddressResponse

#### GET `/api/UserAddress/user/{userId}`
Lấy tất cả địa chỉ của một người dùng
- **Tham số**: `userId` (Guid)
- **Phản hồi**: Mảng UserAddressResponse

#### POST `/api/UserAddress`
Gán địa chỉ cho người dùng
- **Body**: 
  ```json
  {
    "userId": "guid",
    "addressId": "guid"
  }
  ```
- **Phản hồi**: UserAddressResponse (201 Created)

#### DELETE `/api/UserAddress/{id}`
Xóa địa chỉ khỏi người dùng
- **Tham số**: `id` (Guid)
- **Phản hồi**: 204 No Content

---

## Enums

### StatusEnum
Trạng thái của tài khoản, email, số điện thoại:
- `Active` = 1
- `Inactive` = 2
- `Suspended` = 3
- `Deleted` = 4

### ModuleEnum
Danh sách các module trong hệ thống:
- `User` = 1
- `Role` = 2
- `Permission` = 3
- `Address` = 4
- `Order` = 5
- `Product` = 6
- v.v.

---

## Mô Hình Dữ Liệu

### UserResponse
```json
{
  "idUser": "guid",
  "email": "string",
  "username": "string",
  "firstName": "string",
  "lastName": "string",
  "phoneNumber": "string",
  "status": "StatusEnum",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### RoleResponse
```json
{
  "idRole": "guid",
  "name": "string",
  "description": "string",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### PermissionResponse
```json
{
  "idPermission": "guid",
  "name": "string",
  "description": "string",
  "module": "ModuleEnum",
  "operation": "OperationEnum",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### AddressResponse
```json
{
  "idAddress": "guid",
  "street": "string",
  "city": "string",
  "state": "string",
  "postalCode": "string",
  "country": "string",
  "phoneNumber": "string",
  "addressType": "AddressTypeEnum",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

---

## Xác Thực & Phân Quyền

### JWT Token
- Tất cả endpoint có attribute `[Authorize]` đều yêu cầu Bearer token
- Token được trả về sau khi đăng nhập thành công
- Gửi token trong header: `Authorization: Bearer <token>`

### Claims trong Token
- `NameIdentifier`: ID của người dùng (UserId)
- `Email`: Email của người dùng
- `Role`: Danh sách các vai trò của người dùng

---

## Mã Trạng Thái HTTP

| Mã | Ý Nghĩa |
|---|---|
| 200 | OK - Yêu cầu thành công |
| 201 | Created - Tài nguyên mới được tạo |
| 204 | No Content - Yêu cầu thành công, không có nội dung trả về |
| 400 | Bad Request - Dữ liệu không hợp lệ |
| 401 | Unauthorized - Không được xác thực |
| 403 | Forbidden - Không có quyền truy cập |
| 404 | Not Found - Tài nguyên không tìm thấy |
| 500 | Internal Server Error - Lỗi máy chủ |

---

## Ví Dụ Sử Dụng

### 1. Đăng ký tài khoản
```bash
POST /api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password@123",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "0123456789",
  "username": "johndoe"
}
```

### 2. Đăng nhập
```bash
POST /api/Auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password@123"
}

Response:
{
  "token": "eyJhbGc...",
  "user": { ... }
}
```

### 3. Lấy danh sách người dùng
```bash
GET /api/User
Authorization: Bearer <token>
```

### 4. Tạo vai trò mới
```bash
POST /api/Role
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Admin",
  "description": "Vai trò quản trị viên"
}
```

### 5. Gán quyền cho vai trò
```bash
POST /api/RolePermission
Authorization: Bearer <token>
Content-Type: application/json

{
  "roleId": "role-guid",
  "permissionId": "permission-guid"
}
```

### 6. Gán vai trò cho người dùng
```bash
POST /api/UserRole
Authorization: Bearer <token>
Content-Type: application/json

{
  "userId": "user-guid",
  "roleId": "role-guid"
}
```

---

## Thông Tin Thêm

- **Framework**: ASP.NET Core
- **Pattern**: CQRS (Command Query Responsibility Segregation)
- **Mediator**: MediatR
- **Xác thực**: JWT (JSON Web Tokens)
- **Database**: SQL Server (được sử dụng trong dự án)
- **ORM**: Entity Framework Core
