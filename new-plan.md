# Backend Optimization & Refactoring Plan

## 1. Domain Refactoring (Strict DDD & Audit)
**Goal**: Centralize Audit logic, standardizing Aggregates vs Entities.

### Shared Kernel (Domain.Core)
- [ ] Refactor `AggregateRoot`.
  - Add Audit Properties: `CreatedBy`, `UpdatedBy`, `CreatedAt`, `UpdatedAt`.

### Domain Layer (Auth Module)
- [ ] Refactor `User` (AR).
  - Remove manual Audit properties (inherit from `AggregateRoot`).
- [ ] Refactor `UserAddress`.
  - **Downgrade**: Change `AggregateRoot` -> `Entity`.
  - **Move**: Move to `Entities/Auth/Junction` (or child folder).
- [ ] Implement `UserCredentials` (New Entity).
  - **Type**: `Entity` (Child of User).
  - **Properties**: `UserId`, `CredentialType`, `ProviderKey`, `ExtraProps`.
  - **Location**: `Entities/Auth/Junction`.

### Domain Layer (Shop Module)
- [ ] Refactor `Store` (AR).
  - Remove manual Audit properties.
- [ ] Refactor `StoreAddress` (was ShopAddress).
  - **Downgrade**: Change `AggregateRoot` -> `Entity`.
  - **Move**: Move to `Entities/Shop/Junction` (or child folder).

### Infrastructure Layer (Cleanup)
- [ ] Remove Write Repositories for Entities (Strict DDD):
  - `IWUserAddressRepository`
  - `IWStoreAddressRepository` (ShopAddress)
  - `IWUserRoleRepository`
- [ ] Ensure Read Repositories exist if needed for fast lookup:
  - `IRUserCredentialRepository` (New)
  - `IRUserRoleRepository` (Keep)

## 2. Pagination & Filtering (Infrastructure)
**Goal**: Fix "Load All" performance issue.
- [ ] Create `PagedList<T>` & `PaginationHelper`.
- [ ] Refactor Repositories to expose `IQueryable` (or specific filterable methods).
- [ ] Refactor Handlers to use Paging.

## 3. API Standardization
- [ ] Consolidate Search Endpoints (Query Params).
