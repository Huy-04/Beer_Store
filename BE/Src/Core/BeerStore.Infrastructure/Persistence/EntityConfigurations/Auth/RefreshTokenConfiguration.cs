using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter;
using Infrastructure.Core.PropertyConverters;
using Domain.Core.ValueObjects.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
        {
            entity.ToTable("RefreshTokens");

            entity.HasKey(rt => rt.Id);
            entity.Property(rt => rt.Id).HasColumnName("IdRefreshToken");

            // UserId FK
            entity.Property(rt => rt.UserId)
                .IsRequired();

            entity.HasIndex(rt => rt.UserId);

            // TokenHash (Unique Index)
            entity.Property(rt => rt.TokenHash)
                .HasConversion(AuthConverter.TokenHashConverter)
                .HasMaxLength(TokenHash.MaxLength)
                .IsRequired();

            entity.HasIndex(rt => rt.TokenHash)
                .IsUnique();

            // DeviceId
            entity.Property(rt => rt.DeviceId)
                .HasConversion(AuthConverter.DeviceIdConverter)
                .HasMaxLength(DeviceId.MaxLength)
                .IsRequired();

            // DeviceName
            entity.Property(rt => rt.DeviceName)
                .HasConversion(AuthConverter.DeviceNameConverter)
                .HasMaxLength(DeviceName.MaxLength)
                .IsRequired();

            // IpAddress
            entity.Property(rt => rt.IpAddress)
                .HasConversion(CommonConverterExtension.IpAddressConverter)
                .HasMaxLength(IpAddress.MaxLength)
                .IsRequired();

            // ExpiresAt
            entity.Property(rt => rt.ExpiresAt)
                .IsRequired();

            // TokenStatus
            entity.Property(rt => rt.TokenStatus)
                .HasConversion(AuthConverter.TokenStatusConverter)
                .IsRequired();

            // Audit fields
            entity.Property(rt => rt.CreatedBy)
                .IsRequired();

            entity.Property(rt => rt.UpdatedBy)
                .IsRequired();

            entity.Property(rt => rt.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(rt => rt.UpdatedAt)
                .IsRequired();
        }
    }
}
