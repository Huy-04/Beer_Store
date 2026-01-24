using BeerStore.Domain.Entities.Auth;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter;
using Infrastructure.Core.PropertyConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> entity)
        {
            entity.ToTable("UserAddress");

            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("IdUserAddress");

            entity.Property(a => a.UserId)
                .IsRequired();

            entity.HasIndex(a => a.UserId);

            entity.Property(a => a.Phone)
                .HasConversion(AddressConverter.PhoneConverter)
                .HasMaxLength(18)
                .IsRequired();

            entity.Property(a => a.FullName)
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

            entity.Property(a => a.UserAddressType)
                .HasConversion(AuthConverter.UserAddressTypeConverter)
                .IsRequired();

            entity.Property(a => a.CreatedBy)
                .IsRequired();

            entity.Property(a => a.UpdatedBy)
                .IsRequired();

            entity.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(a => a.UpdatedAt)
                .IsRequired();
        }
    }
}