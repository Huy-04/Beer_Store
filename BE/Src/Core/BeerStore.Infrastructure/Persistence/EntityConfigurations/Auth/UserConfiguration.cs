using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter;
using Infrastructure.Core.PropertyConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("IdUser");

            entity.Property(u => u.Email)
                .HasConversion(CommonConverterExtension.EmailConverter)
                .HasMaxLength(100)
                .IsRequired();
            entity.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            entity.Property(u => u.Phone)
                .HasConversion(AddressConverter.PhoneConverter)
                .HasMaxLength(18)
                .IsRequired();

            entity.Property(u => u.FullName)
                .HasConversion(AddressConverter.FullNameConverter)
                .HasMaxLength(69)
                .IsRequired();

            entity.Property(u => u.UserName)
                .HasConversion(AuthConverter.UserNameConverter)
                .HasMaxLength(69)
                .IsRequired();
            entity.HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserName");

            entity.Property(u => u.Password)
                .HasConversion(AuthConverter.PasswordConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.UserStatus)
                .HasConversion(AuthConverter.UserStatusConverter)
                .IsRequired();

            entity.Property(u => u.EmailStatus)
                .HasConversion(AuthConverter.EmailStatusConverter)
                .IsRequired();

            entity.Property(u => u.PhoneStatus)
                .HasConversion(AuthConverter.PhoneStatusConverter)
                .IsRequired();

            entity.Property(u => u.CreatedBy)
                .IsRequired();

            entity.Property(u => u.UpdatedBy)
                .IsRequired();

            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(u => u.UpdatedAt)
                .IsRequired();
        }
    }
}
