using BeerStore.Domain.Entities.Shop;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Shop.Converter;
using Infrastructure.Core.PropertyConverters;
using BeerStore.Domain.Entities.Shop.Junction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Shop.Junction
{
    public class StoreAddressConfiguration : IEntityTypeConfiguration<StoreAddress>
    {
        public void Configure(EntityTypeBuilder<StoreAddress> entity)
        {
            entity.ToTable("StoreAddress");

            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("IdStoreAddress");

            entity.Property(a => a.StoreId)
                .IsRequired();

            entity.HasIndex(a => a.StoreId);

            entity.Property(a => a.Phone)
                .HasConversion(AddressConverter.PhoneConverter)
                .HasMaxLength(18)
                .IsRequired();

            entity.Property(a => a.ContactName)
                .HasConversion(AddressConverter.FullNameConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.Province)
                .HasConversion(AddressConverter.ProvinceConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.District)
                .HasConversion(AddressConverter.DistrictConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.Ward)
                .HasConversion(AddressConverter.WardConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.Street)
                .HasConversion(AddressConverter.StreetConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.IsDefault)
                .HasConversion(AddressConverter.IsDefaultConverter)
                .IsRequired();

            entity.Property(a => a.StoreAddressType)
                .HasConversion(ShopConverter.StoreAddressTypeConverter)
                .IsRequired();


        }
    }
}
