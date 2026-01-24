# Hướng dẫn sử dụng AI Agent Kit

> Skills, Workflows, Agents cho dự án Beer Store

---

## 🧠 Skills (Tự động)

Skills được **tự động kích hoạt** dựa trên context. Agent sẽ:
1. Nhận diện task → Tìm skill phù hợp
2. Đọc `SKILL.md` trong folder
3. Áp dụng kiến thức

**Không cần gọi thủ công** - chỉ cần mô tả task, agent tự chọn skill.

---

### 📋 Danh sách Skills (19)

#### Backend & API (C#/.NET)

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `dotnet-patterns` | Clean Architecture, CQRS, EF Core | Viết Entity, Handler, Repository |
| `api-patterns` | REST design, versioning, response | Thiết kế API endpoints |
| `database-design` | Schema, indexing, optimization | Thiết kế database, tối ưu query |

#### Frontend (React)

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `react-patterns` | Hooks, state, composition | Viết React components |
| `tailwind-patterns` | Tailwind CSS v4 utilities | Styling với Tailwind |

#### Testing

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `testing-patterns` | Unit, integration, mocking | Viết unit/integration tests |
| `tdd-workflow` | Test-driven development | Phát triển theo TDD |
| `webapp-testing` | E2E, Playwright | Viết E2E tests |

#### Code Quality

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `clean-code` | SOLID, coding standards | Review/refactor code |
| `code-review-checklist` | Review guidelines | Code review |
| `lint-and-validate` | Linting, static analysis | Check code quality |

#### Debugging & Security

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `systematic-debugging` | Root cause analysis | Fix bugs |
| `vulnerability-scanner` | Security audit patterns | Security review |

#### Planning & Architecture

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `architecture` | System design patterns | Thiết kế hệ thống |
| `plan-writing` | Task breakdown, roadmaps | Lập kế hoạch |
| `brainstorming` | Idea exploration | Brainstorm ideas |

#### DevOps

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `deployment-procedures` | CI/CD, deploy workflows | Deploy, setup CI/CD |

#### Agent Coordination

| Skill | Mô tả | Khi nào dùng |
|-------|-------|--------------|
| `parallel-agents` | Multi-agent orchestration | Task phức tạp cần nhiều agents |
| `behavioral-modes` | Agent behavior patterns | Điều chỉnh cách agent hoạt động |

---

### 🎯 Skills theo Scenario

#### Scenario 1: Tạo Feature Backend mới

```
"Tạo Order module với CRUD operations"
```

**Skills tự động kích hoạt:**
- `dotnet-patterns` → Entity, Handler patterns
- `api-patterns` → REST endpoints design
- `database-design` → Schema design

#### Scenario 2: Tạo UI Component

```
"Tạo Product listing page với filter và pagination"
```

**Skills tự động kích hoạt:**
- `react-patterns` → Component structure, hooks
- `tailwind-patterns` → Styling

#### Scenario 3: Fix Bug

```
"Login API trả về 500 error"
```

**Skills tự động kích hoạt:**
- `systematic-debugging` → Root cause analysis

#### Scenario 4: Viết Tests

```
"Viết unit tests cho UserHandler"
```

**Skills tự động kích hoạt:**
- `testing-patterns` → Test structure, mocking
- `tdd-workflow` → TDD approach

#### Scenario 5: Security Review

```
"Review security cho Auth module"
```

**Skills tự động kích hoạt:**
- `vulnerability-scanner` → Security checklist

#### Scenario 6: Plan Feature

```
"Lập kế hoạch cho Catalog module"
```

**Skills tự động kích hoạt:**
- `plan-writing` → Task breakdown
- `architecture` → System design

---

### 💡 Tips sử dụng Skills hiệu quả

1. **Mô tả rõ task** - Càng chi tiết, agent chọn skill càng chính xác
2. **Đề cập technology** - "React component", "C# handler", "EF Core"
3. **Nêu mục đích** - "tối ưu performance", "fix bug", "add feature"

**Ví dụ TỐT:**
```
"Tạo CreateOrderCHandler với transaction và authorization check"
```

**Ví dụ CHƯA TỐT:**
```
"Tạo handler"
```

---

## 🔄 Workflows (Slash commands)

Gọi bằng lệnh `/command`:

| Command | Mô tả | Ví dụ |
|---------|-------|-------|
| `/plan` | Tạo task breakdown | `/plan Catalog module` |
| `/create` | Tạo feature mới | `/create Order CRUD` |
| `/debug` | Systematic debugging | `/debug login fails` |
| `/test` | Generate & run tests | `/test UserHandler` |
| `/enhance` | Cải thiện code | `/enhance OrderService performance` |
| `/deploy` | Deploy application | `/deploy to staging` |
| `/brainstorm` | Explore options | `/brainstorm payment integration` |
| `/orchestrate` | Multi-agent task | `/orchestrate full-stack feature` |
| `/preview` | Preview locally | `/preview changes` |
| `/status` | Check project status | `/status` |
| `/ui-ux-pro-max` | UI design | `/ui-ux-pro-max dashboard` |

### Workflow theo giai đoạn

```
1. /plan Feature X       → Lập kế hoạch
2. /create Feature X     → Implement
3. /test Feature X       → Viết tests
4. /enhance Feature X    → Tối ưu
5. /deploy               → Deploy
```

---

## 🤖 Agents (Mention)

Gọi agent bằng cách mention:

| Agent | Chuyên môn | Khi nào dùng |
|-------|------------|--------------|
| `backend-specialist` | C#/.NET, Clean Architecture | Implement BE features |
| `frontend-specialist` | React, UI/UX | Implement FE components |
| `database-architect` | Schema, EF Core | Database design/optimization |
| `security-auditor` | Security compliance | Security review |
| `test-engineer` | Testing strategies | Viết tests |
| `debugger` | Root cause analysis | Fix complex bugs |
| `project-planner` | Task planning | Lập kế hoạch |
| `code-archaeologist` | Legacy refactoring | Refactor old code |
| `devops-engineer` | CI/CD, Docker | DevOps tasks |
| `orchestrator` | Multi-agent coordination | Complex multi-domain tasks |

### Cách gọi Agent

```
Use backend-specialist to implement Order module
Use security-auditor to review Auth module
Use debugger to fix payment processing error
```

### Kết hợp Agents

```
Use orchestrator to coordinate backend-specialist and frontend-specialist for Order feature
```

---

## 📋 Tóm tắt

| Component | Cách gọi | Ví dụ |
|-----------|----------|-------|
| **Skills** | Tự động | Mô tả task chi tiết |
| **Workflows** | `/command` | `/plan Catalog module` |
| **Agents** | `Use [agent] to...` | `Use backend-specialist to...` |

---

## 🚀 Quick Start Examples

### Example 1: Backend Feature
```
Use backend-specialist to create Product entity with CRUD operations
```

### Example 2: Full-stack Feature
```
/plan Order management feature
Use orchestrator to coordinate implementation
```

### Example 3: Bug Fix
```
/debug API returns 500 when creating order
Use debugger to find root cause
```

### Example 4: Security Audit
```
Use security-auditor to review authentication flow
```

### Example 5: Performance
```
/enhance Product listing query performance
Use database-architect to optimize schema
```
