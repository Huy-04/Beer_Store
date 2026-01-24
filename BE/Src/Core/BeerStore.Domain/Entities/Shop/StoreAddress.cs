using BeerStore.Domain.Enums.Shop;
using BeerStore.Domain.ValueObjects.Auth.Address;
using Domain.Core.ValueObjects;
using Domain.Core.ValueObjects.Address;

namespace BeerStore.Domain.Entities.Shop
{
    public class StoreAddress : AggregateRoot
    {
        public Guid StoreId { get; private set; }

        public Phone Phone { get; private set; }

        public FullName ContactName { get; private set; }

        public Province Province { get; private set; }

        public District District { get; private set; }

        public Ward Ward { get; private set; }

        public Street Street { get; private set; }

        public StoreAddressType Type { get; private set; }

        public IsDefault IsDefault { get; private set; }

        public Guid CreatedBy { get; private set; }

        public Guid UpdatedBy { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private StoreAddress()
        { }

        private StoreAddress(Guid id, Guid storeId, Phone phone, FullName contactName, Province province, District district, Ward ward, Street street, StoreAddressType type, IsDefault isDefault, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            StoreId = storeId;
            Phone = phone;
            ContactName = contactName;
            Province = province;
            District = district;
            Ward = ward;
            Street = street;
            Type = type;
            IsDefault = isDefault;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static StoreAddress Create(Guid storeId, Phone phone, FullName contactName, Province province, District district, Ward ward, Street street, StoreAddressType type, IsDefault isDefault, Guid createdBy, Guid updatedBy)
        {
            var address = new StoreAddress(Guid.NewGuid(), storeId, phone, contactName, province, district, ward, street, type, isDefault, createdBy, updatedBy);
            return address;
        }

        public void UpdatePhone(Phone phone)
        {
            if (Phone == phone) return;
            Phone = phone;
            Touch();
        }

        public void UpdateContactName(FullName contactName)
        {
            if (ContactName == contactName) return;
            ContactName = contactName;
            Touch();
        }

        public void UpdateProvince(Province province)
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

        public void UpdateType(StoreAddressType type)
        {
            if (Type == type) return;
            Type = type;
            Touch();
        }

        public void UpdateIsDefault(IsDefault isDefault)
        {
            if (IsDefault == isDefault) return;
            IsDefault = isDefault;
            Touch();
        }

        public void SetUpdatedBy(Guid updatedBy)
        {
            if (UpdatedBy == updatedBy) return;
            UpdatedBy = updatedBy;
            Touch();
        }

        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
