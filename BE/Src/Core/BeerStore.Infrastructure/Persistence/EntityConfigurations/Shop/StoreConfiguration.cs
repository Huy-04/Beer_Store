using BeerStore.Domain.Entities.Shop;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Shop.Converter;
using Infrastructure.Core.PropertyConverters;
using Infrastructure.Core.PropertyConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Shop
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> entity)
        {
            entity.ToTable("Store");

            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasColumnName("IdStore");

            entity.Property(s => s.OwnerId)
                .IsRequired();

            entity.HasIndex(s => s.OwnerId);

            entity.Property(s => s.StoreName)
                .HasConversion(ShopConverter.StoreNameConverter)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(s => s.Slug)
                .HasConversion(CommonConverterExtension.SlugConverter)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(s => s.Slug)
                .IsUnique();

            entity.Property(s => s.Logo)
                .HasConversion(CommonConverterExtension.ImgConverter)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(s => s.Description)
                .HasConversion(CommonConverterExtension.DescriptionConverter)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(s => s.StoreType)
                .HasConversion(ShopConverter.StoreTypeConverter)
                .IsRequired();

            entity.Property(s => s.StoreStatus)
                .HasConversion(ShopConverter.StoreStatusConverter)
                .IsRequired();

            entity.Property(s => s.CreatedBy)
                .IsRequired();

            entity.Property(s => s.UpdatedBy)
                .IsRequired();

            entity.Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(s => s.UpdatedAt)
                .IsRequired();
        }
    }
}