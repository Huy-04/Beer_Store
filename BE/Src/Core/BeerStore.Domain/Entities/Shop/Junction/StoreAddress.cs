using BeerStore.Domain.Enums.Shop;
using Domain.Core.ValueObjects;
using Domain.Core.ValueObjects.Address;
using Domain.Core.ValueObjects.Common;

using Domain.Core.Base;

namespace BeerStore.Domain.Entities.Shop.Junction
{
    public class StoreAddress : Entity
    {
        public Guid StoreId { get; private set; }

        public Phone Phone { get; private set; }

        public FullName ContactName { get; private set; }

        public Province Province { get; private set; }

        public District District { get; private set; }

        public Ward Ward { get; private set; }

        public Street Street { get; private set; }

        public StoreAddressType StoreAddressType { get; private set; }

        public IsDefault IsDefault { get; private set; }

        private StoreAddress()
        { }

        private StoreAddress(Guid id, Guid storeId, Phone phone, FullName contactName, Province province, District district, Ward ward, Street street, StoreAddressType storeAddressType, IsDefault isDefault)
            : base(id)
        {
            StoreId = storeId;
            Phone = phone;
            ContactName = contactName;
            Province = province;
            District = district;
            Ward = ward;
            Street = street;
            StoreAddressType = storeAddressType;
            IsDefault = isDefault;
        }

        public static StoreAddress Create(Guid storeId, Phone phone, FullName contactName, Province province, District district, Ward ward, Street street, StoreAddressType type, IsDefault isDefault)
        {
            var address = new StoreAddress(Guid.NewGuid(), storeId, phone, contactName, province, district, ward, street, type, isDefault);
            return address;
        }

        public void UpdatePhone(Phone phone)
        {
            if (Phone == phone) return;
            Phone = phone;
        }

        public void UpdateContactName(FullName contactName)
        {
            if (ContactName == contactName) return;
            ContactName = contactName;
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

        public void UpdateType(StoreAddressType storeAddressType)
        {
            if (StoreAddressType == storeAddressType) return;
            StoreAddressType = storeAddressType;
        }

        public void UpdateIsDefault(IsDefault isDefault)
        {
            if (IsDefault == isDefault) return;
            IsDefault = isDefault;
        }
    }
}