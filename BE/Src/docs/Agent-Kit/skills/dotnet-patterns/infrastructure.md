# Infrastructure Layer Patterns

> Repositories, DbContext, EF Configuration, Services, DI

---

> ⚠️ **Before creating Converter**: Check `Infrastructure.Core/PropertyConverters/` first.
> See [SKILL.md - Core Components](SKILL.md#2-core-components---check-before-creating)

---

## Folder Structure

```
BeerStore.Infrastructure/
├── Persistence/
│   ├── Db/AuthDbContext.cs
│   └── EntityConfigurations/
│       └── Auth/
│           ├── UserConfiguration.cs
│           ├── Converter/AuthConverter.cs
│           └── Junction/UserRoleConfiguration.cs
├── Repository/
│   └── Auth/
│       ├── Read/RUserRepository.cs
│       └── Write/WUserRepository.cs
├── UnitOfWork/AuthUnitOfWork.cs
├── Services/
│   └── Auth/
│       ├── JwtService.cs
│       └── Authorization/AuthenticationService.cs
└── DependencyInjection/AuthDependencyInjection.cs
```

---

## Repository Rules

| Type | Prefix | Base Class | Purpose |
|------|--------|------------|---------|
| Read | `R` | `ReadRepositoryGeneric<T>` | Queries, AsNoTracking |
| Write | `W` | `WriteRepositoryGeneric<T>` | Add, Update, Remove |

---

## Read Repository

```csharp
public class RUserRepository : ReadRepositoryGeneric<User>, IRUserRepository
{
    public RUserRepository(AuthDbContext context) : base(context) { }

    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken token = default)
        => AnyAsync(u => u.Email == email, token);

    public async Task<User?> GetByEmailWithRolesAsync(Email email, CancellationToken token = default)
        => await _entities.AsNoTracking()
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Email == email, token);
}
```

---

## Write Repository

```csharp
public class WUserRepository : WriteRepositoryGeneric<User>, IWUserRepository
{
    public WUserRepository(AuthDbContext context) : base(context) { }
    // Usually empty - inherits generic methods
}
```

---

## DbContext

```csharp
public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Filter by namespace
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            a => a.Namespace!.Contains(".EntityConfigurations.Auth"));
    }
}
```

---

## Entity Configuration

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");
        entity.HasKey(u => u.Id);
        entity.Property(u => u.Id).HasColumnName("IdUser");

        // StringBase - use converter
        entity.Property(u => u.Email)
            .HasConversion(AuthConverter.EmailConverter)
            .HasMaxLength(100)
            .IsRequired();

        // Unique index
        entity.HasIndex(u => u.Email).IsUnique();

        // EnumBase - store as int
        entity.Property(u => u.UserStatus)
            .HasConversion(AuthConverter.UserStatusConverter)
            .IsRequired();
    }
}
```

---

## Value Object Converters

```csharp
public static class AuthConverter
{
    // StringBase
    public static readonly ValueConverter<Email, string>
        EmailConverter = new(v => v.Value, v => Email.Create(v));

    public static readonly ValueConverter<Password, string>
        PasswordConverter = new(v => v.Value, v => Password.Create(v));

    // EnumBase
    public static readonly ValueConverter<UserStatus, int>
        UserStatusConverter = new(v => (int)v.Value, v => UserStatus.Create((StatusEnum)v));
}
```

---

## Junction Configuration

```csharp
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> entity)
    {
        entity.ToTable("UserRoles");
        entity.HasKey(ur => ur.Id);

        entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();

        entity.HasOne<User>()
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

---

## Authorization Service

```csharp
public class AuthenticationService : IAuthAuthorizationService
{
    private readonly ICurrentUserContext _currentUser;

    public void EnsureCanReadUser(Guid targetUserId)
    {
        if (_currentUser.HasPermission("User.Read.All")) return;
        if (_currentUser.HasPermission("User.Read.Self") && _currentUser.UserId == targetUserId) return;
        ThrowForbidden(UserField.IdUser);
    }

    public void EnsureCanCreateUser()
    {
        if (_currentUser.HasPermission("User.Create.All")) return;
        ThrowForbidden(UserField.IdUser);
    }
}
```

---

## Dependency Injection

```csharp
public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration config)
{
    // DbContext
    services.AddDbContext<AuthDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("AuthDb")));

    // Repositories
    services.AddScoped<IRUserRepository, RUserRepository>();
    services.AddScoped<IWUserRepository, WUserRepository>();

    // UnitOfWork
    services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

    // Services
    services.AddScoped<IAuthAuthorizationService, AuthenticationService>();
    services.AddScoped<IPasswordHasher, PasswordHasher>();
    services.AddScoped<IJwtService, JwtService>();

    return services;
}
```
