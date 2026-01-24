using BeerStore.Domain.ValueObjects.Auth.Address;
using Domain.Core.ValueObjects.Address;

namespace BeerStore.Domain.Entities.Auth
{
    public class UserAddress : AggregateRoot
    {
        public Guid UserId { get; private set; }

        public Phone Phone { get; set; }

        public FullName FullName { get; set; }

        public Province Province { get; private set; }

        public District District { get; private set; }

        public Ward Ward { get; private set; }

        public Street Street { get; private set; }

        public IsDefault IsDefault { get; private set; }

        public UserAddressType UserAddressType { get; private set; }

        public Guid CreatedBy { get; private set; }

        public Guid UpdatedBy { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private UserAddress()
        { }

        private UserAddress(Guid id, Guid userId, Phone phone, FullName fullName, Province province, District district, Ward ward, Street street, IsDefault isDefault, UserAddressType addressType, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            UserId = userId;
            Phone = phone;
            FullName = fullName;
            Province = province;
            District = district;
            Ward = ward;
            Street = street;
            IsDefault = isDefault;
            UserAddressType = addressType;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static UserAddress Create(Guid userId, Phone phone, FullName fullName, Province province, District district, Ward ward, Street street, IsDefault isDefault, UserAddressType userAddressType, Guid createdBy, Guid updatedBy)
        {
            var address = new UserAddress(Guid.NewGuid(), userId, phone, fullName, province, district, ward, street, isDefault, userAddressType, createdBy, updatedBy);
            return address;
        }

        public void UpdatePhone(Phone phone)
        {
            if (Phone == phone) return;
            Phone = phone;
            Touch();
        }

        public void UpdateFullName(FullName fullName)
        {
            if (FullName == fullName) return;
            FullName = fullName;
            Touch();
        }

        public void UpdateProvice(Province province)
        {
            if (Province == province) return;
            Province = province;
            Touch();
        }

        public void UpdateDistrict(District district)
        {
            if (District == district) return;
            District = district;
            Touch();
        }

        public void UpdateWard(Ward ward)
        {
            if (Ward == ward) return;
            Ward = ward;
            Touch();
        }

        public void UpdateStreet(Street street)
        {
            if (Street == street) return;
            Street = street;
            Touch();
        }

        public void UpdateIsDefault(IsDefault isDefault)
        {
            if (IsDefault == isDefault) return;
            IsDefault = isDefault;
            Touch();
        }

        public void UpdateAddressType(UserAddressType userAddressType)
        {
            if (UserAddressType == userAddressType) return;
            UserAddressType = userAddressType;
            Touch();
        }

        public void SetUpdatedBy(Guid updateBy)
        {
            if (UpdatedBy == updateBy) return;
            UpdatedBy = updateBy;
            Touch();
        }

        // Extension
        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}