# Hướng dẫn sử dụng AI Agent Kit

> Skills, Workflows, Agents cho dự án Beer Store

---

## 🧠 Skills (Tự động)

Skills được **tự động kích hoạt** dựa trên context. Agent sẽ:
1. Nhận diện task → Tìm skill phù hợp
2. Đọc `SKILL.md` trong folder
3. Áp dụng kiến thức

**Không cần gọi thủ công.**

### Danh sách Skills

| Skill | Mô tả |
|-------|-------|
| `api-patterns` | REST design, versioning |
| `architecture` | System design patterns |
| `database-design` | Schema, indexing |
| `testing-patterns` | Unit/Integration tests |
| `clean-code` | Coding standards |
| `vulnerability-scanner` | Security audit |
| `react-patterns` | React hooks, state |
| `tailwind-patterns` | Tailwind CSS v4 |
| `ui-ux-pro-max` | 50 styles, 21 palettes |

---

## 🔄 Workflows (Slash commands)

Gọi bằng lệnh `/command`:

```
/plan Catalog module
/debug login fails
/test UserHandler
```

| Command | Mô tả |
|---------|-------|
| `/plan` | Tạo task breakdown |
| `/debug` | Systematic debugging |
| `/test` | Generate & run tests |
| `/create` | Tạo feature mới |
| `/enhance` | Cải thiện code |
| `/deploy` | Deploy application |
| `/orchestrate` | Multi-agent |
| `/preview` | Preview locally |
| `/ui-ux-pro-max` | UI design |

---

## 🤖 Agents (Mention)

Gọi agent bằng cách mention:

```
Use security-auditor to review Auth module
Use database-architect to optimize Product schema
```

| Agent | Mô tả |
|-------|-------|
| `backend-specialist` | API, business logic |
| `database-architect` | Schema design |
| `security-auditor` | Security audit |
| `debugger` | Root cause analysis |
| `test-engineer` | Testing strategies |
| `project-planner` | Task planning |
| `frontend-specialist` | React frontend |
| `orchestrator` | Multi-agent tasks |
| `devops-engineer` | CI/CD, Docker |

---

## 📋 Tóm tắt

| Component | Cách gọi | Ví dụ |
|-----------|----------|-------|
| Skills | Tự động | — |
| Workflows | `/command` | `/plan`, `/debug` |
| Agents | `Use [agent] to...` | `Use debugger to...` |
