using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BeerStore.Infrastructure.Persistence.Db
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // Auth
        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<UserAddress> Addresses => Set<UserAddress>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                a => a.Namespace!.Contains(".EntityConfigurations"));
        }
    }
}
