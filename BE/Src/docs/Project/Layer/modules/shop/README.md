# Shop Module

> Store management module

---

## 📦 Entities

| Entity | Description |
|--------|-------------|
| `Store` | Store information |
| `UserStore` | Junction: User ↔ Store (ownership) |

---

## 🔗 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/store` | Get all stores |
| GET | `/api/store/{id}` | Get store by ID |
| POST | `/api/store` | Create store |
| PUT | `/api/store/{id}` | Update store |
| DELETE | `/api/store/{id}` | Remove store |

---

## 🔐 Permissions

| Permission | Description |
|------------|-------------|
| `Store.Read.All` | Read any store |
| `Store.Read.Self` | Read own stores |
| `Store.Create.All` | Create stores |
| `Store.Update.All` | Update any store |
| `Store.Update.Self` | Update own stores |
| `Store.Delete.All` | Delete any store |
| `Store.Delete.Self` | Delete own stores |

---

## 📁 File Locations

```
Domain/Entities/Shop/
├── Store.cs
└── UserStore.cs

Domain/ValueObjects/Shop/
├── StoreName.cs
└── StoreAddress.cs

Application/Modules/Shop/
├── Store/
│   ├── Commands/
│   └── Queries/
└── Junction/

Infrastructure/Repository/Shop/
├── RStoreRepository.cs
├── WStoreRepository.cs
└── ...

Api/Controllers/Shop/
└── StoreController.cs
```

---

## ✅ Status

- [x] Store CRUD
- [ ] UserStore management
- [ ] Store statistics
