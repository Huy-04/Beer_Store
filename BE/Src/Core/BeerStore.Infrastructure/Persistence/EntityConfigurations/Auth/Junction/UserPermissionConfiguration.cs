using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Junction
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> entity)
        {
            entity.ToTable("UserPermissions");

            entity.HasKey(up => up.Id);
            entity.Property(up => up.Id).HasColumnName("IdUserPermission");

            entity.Property(up => up.UserId)
                .IsRequired();

            entity.Property(up => up.PermissionId)
                .IsRequired();

            entity.Property(up => up.Status)
                .HasConversion<int>()
                .IsRequired();

            entity.HasIndex(up => new { up.UserId, up.PermissionId })
                .IsUnique()
                .HasDatabaseName("UserPermission_Unique");

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserPermission_User");

            entity.HasOne<Permission>()
                .WithMany()
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserPermission_Permission");
        }
    }
}
