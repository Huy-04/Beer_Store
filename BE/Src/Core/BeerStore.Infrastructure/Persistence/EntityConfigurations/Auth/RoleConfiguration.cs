using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter;
using Infrastructure.Core.PropertyConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable("Roles");

            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasColumnName("IdRole");

            entity.Property(r => r.RoleName)
                .HasConversion(AuthConverter.RoleNameConverter)
                .HasMaxLength(69)
                .IsRequired();
            entity.HasIndex(r => r.RoleName).IsUnique()
                .HasDatabaseName("RoleName_Unique");

            entity.Property(r => r.Description)
                .HasConversion(CommonConverterExtension.DescriptionConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(r => r.CreatedBy)
                .IsRequired();

            entity.Property(r => r.UpdatedBy)
                .IsRequired();

            entity.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(r => r.UpdatedAt)
                .IsRequired();
        }
    }
}