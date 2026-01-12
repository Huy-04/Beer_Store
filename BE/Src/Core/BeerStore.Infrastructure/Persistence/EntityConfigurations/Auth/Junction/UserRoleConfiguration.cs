using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Junction
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.ToTable("UserRoles");

            entity.HasKey(ur => ur.Id);
            entity.Property(ur => ur.Id).HasColumnName("IdUserRole");

            entity.Property(ur => ur.UserId)
                .IsRequired();

            entity.Property(ur => ur.RoleId)
                .IsRequired();

            entity.HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique()
                .HasDatabaseName("UserRole_Unique");

            entity.HasOne<User>()
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserRole_User");

            entity.HasOne<Role>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserRole_Role");
        }
    }
}
