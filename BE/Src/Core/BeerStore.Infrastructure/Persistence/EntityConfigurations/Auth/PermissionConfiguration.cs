using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter;
using Infrastructure.Core.PropertyConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> entity)
        {
            entity.ToTable("Permissions");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("IdPermission");

            entity.Property(p => p.PermissionName)
                .HasConversion(AuthConverter.PermissionNameConverter)
                .HasMaxLength(69)
                .IsRequired();
            entity.HasIndex(p => p.PermissionName).IsUnique()
                .HasDatabaseName("PermissionName_Unique");

            entity.Property(p => p.Module)
                .HasConversion(AuthConverter.ModuleConverter)
                .IsRequired();

            entity.Property(p => p.Operation)
                .HasConversion(AuthConverter.OperationConverter)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasConversion(CommonConverterExtension.DescriptionConverter)
                .IsRequired();

            entity.Property(p => p.CreatedBy)
                .IsRequired();

            entity.Property(p => p.UpdatedBy)
                .IsRequired();

            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(p => p.UpdatedAt)
                .IsRequired();
        }
    }
}