using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Junction
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> entity)
        {
            entity.ToTable("UserAddreses");

            entity.HasKey(ud => ud.Id);
            entity.Property(ud => ud.Id).HasColumnName("IdUserAddress");

            entity.Property(ud => ud.UserId)
                .IsRequired();

            entity.Property(ud => ud.AddressId)
                .IsRequired();

            entity.HasIndex(ud => new { ud.UserId, ud.AddressId })
                .IsUnique()
                .HasDatabaseName("UserAddress_Unique");

            entity.HasOne<User>()
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(ud => ud.UserId)
                .HasConstraintName("FK_UserAddress_User");

            entity.HasOne<Address>()
                .WithMany()
                .HasForeignKey(ud => ud.AddressId)
                .HasConstraintName("FK_UserAddress_Address");
        }
    }
}