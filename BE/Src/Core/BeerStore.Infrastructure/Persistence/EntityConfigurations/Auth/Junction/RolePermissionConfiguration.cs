using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Junction
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> entity)
        {
            entity.ToTable("RolePermissions");

            entity.HasKey(rp => rp.Id);
            entity.Property(rp => rp.Id).HasColumnName("IdRolePermission");

            entity.Property(rp => rp.RoleId)
                .IsRequired();

            entity.Property(rp => rp.PermissionId)
                .IsRequired();

            entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                .IsUnique()
                .HasDatabaseName("RolePermission_Unique");

            entity.HasOne<Role>()
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RolePermission_Role");

            entity.HasOne<Permission>()
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RolePermission_Permission");
        }
    }
}
