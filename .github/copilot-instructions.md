# Beer Store - Copilot Instructions

> This file is automatically read by GitHub Copilot for all requests in this repository.

## Project Overview

- **Type**: .NET 9 Web API with Clean Architecture
- **Pattern**: CQRS (MediatR), DDD, Repository Pattern
- **Database**: SQL Server with EF Core (Code First)

## Before Writing Code

**ALWAYS read the relevant documentation first:**

1. **Main Guide**: `BE/Src/docs/Project/AGENTS.md` - Golden Rules & Architecture
2. **Patterns**: `BE/Src/docs/Agent-Kit/skills/dotnet-patterns/` - Layer-specific patterns
3. **Module Docs**: `BE/Src/docs/Project/Layer/modules/{module}/README.md`

## Golden Rules

### Workflow
1. **SKILL FIRST** - Read skill from `Agent-Kit/skills/` BEFORE coding
2. **PLAN FIRST** - Create plan, wait for user approval
3. **VERIFY** - Build after changes: `dotnet build BE/Src/BeerStore.sln`
4. **REPORT** - Report results after completing task

### Code Standards
5. **Authorization FIRST** - `_authService.EnsureCan...()` is the first line in Handler
6. **NO business logic in Controllers** - Controllers only route to MediatR
7. **Validate in Domain** - Use `RuleValidator`, don't throw manual exceptions
8. **Transactions in Commands** - `BeginTransaction` → `Commit` / `Rollback`
9. **CORE FIRST** - Check `*.Core` before creating new ValueObjects, Converters, Base classes

## Quick Commands

```bash
dotnet build BE/Src/BeerStore.sln
dotnet run --project BE/Src/Core/BeerStore.Api
```

## File Mapping

| Task Type | Read This First |
|-----------|-----------------|
| API/Controller | `BE/Src/docs/Agent-Kit/skills/dotnet-patterns/api.md` |
| Command/Query/Handler | `BE/Src/docs/Agent-Kit/skills/dotnet-patterns/application.md` |
| Entity/ValueObject | `BE/Src/docs/Agent-Kit/skills/dotnet-patterns/domain.md` |
| Repository/DbContext | `BE/Src/docs/Agent-Kit/skills/dotnet-patterns/infrastructure.md` |
| **Core Components** | `BE/Src/docs/Agent-Kit/skills/dotnet-patterns/SKILL.md#2-core-components` |
- ❌ AutoMapper → Use manual mapping extensions
- ❌ Create new ValueObject/Converter → Check `*.Core` first
