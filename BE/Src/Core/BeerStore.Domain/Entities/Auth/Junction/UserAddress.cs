using Domain.Core.ValueObjects.Address;
using Domain.Core.ValueObjects.Common;

using Domain.Core.Base;
using BeerStore.Domain.ValueObjects.Auth.UserAddress;

namespace BeerStore.Domain.Entities.Auth.Junction
{
    public class UserAddress : Entity
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

        private UserAddress()
        { }

        private UserAddress(Guid id, Guid userId, Phone phone, FullName fullName, Province province, District district, Ward ward, Street street, IsDefault isDefault, UserAddressType addressType)
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
        }

        public static UserAddress Create(Guid userId, Phone phone, FullName fullName, Province province, District district, Ward ward, Street street, IsDefault isDefault, UserAddressType userAddressType)
        {
            var address = new UserAddress(Guid.NewGuid(), userId, phone, fullName, province, district, ward, street, isDefault, userAddressType);
            return address;
        }

        public void UpdatePhone(Phone phone)
        {
            if (Phone == phone) return;
            Phone = phone;
        }

        public void UpdateFullName(FullName fullName)
        {
            if (FullName == fullName) return;
            FullName = fullName;
        }

        public void UpdateProvince(Province province)
        {
            if (Province == province) return;
            Province = province;
        }

        public void UpdateDistrict(District district)
        {
            if (District == district) return;
            District = district;
        }

        public void UpdateWard(Ward ward)
        {
            if (Ward == ward) return;
            Ward = ward;
        }

        public void UpdateStreet(Street street)
        {
            if (Street == street) return;
            Street = street;
        }

        public void UpdateIsDefault(IsDefault isDefault)
        {
            if (IsDefault == isDefault) return;
            IsDefault = isDefault;
        }

        public void UpdateAddressType(UserAddressType userAddressType)
        {
            if (UserAddressType == userAddressType) return;
            UserAddressType = userAddressType;
        }
    }
}