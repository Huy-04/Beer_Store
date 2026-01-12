using BeerStore.Domain.Entities.Auth;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.ToTable("Address");

            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("IdAddress");

            entity.Property(a => a.UserId)
                .IsRequired();

            entity.HasIndex(a => a.UserId);

            entity.Property(a => a.Phone)
                .HasConversion(AuthConverter.PhoneConverter)
                .HasMaxLength(18)
                .IsRequired();

            entity.Property(a => a.FullName)
                .HasConversion(AuthConverter.FullNameConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.Province)
                .HasConversion(AuthConverter.ProvinceConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.District)
                .HasConversion(AuthConverter.DistrictConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.Ward)
                .HasConversion(AuthConverter.WardConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(a => a.Street)
                .HasConversion(AuthConverter.StreetConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(a => a.IsDefault)
                .HasConversion(AuthConverter.IsDefaultConverter)
                .IsRequired();

            entity.Property(a => a.AddressType)
                .HasConversion(AuthConverter.AddressTypeConverter)
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
