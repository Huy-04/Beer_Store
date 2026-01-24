using BeerStore.Application.DTOs.Shop.StoreAddress.Requests;
using BeerStore.Domain.Entities.Shop;
using BeerStore.Domain.Enums.Shop;
using Domain.Core.ValueObjects.Address;

namespace BeerStore.Application.Mapping.Shop.StoreAddressMap
{
    public static class RequestToStoreAddress
    {
        public static StoreAddress ToStoreAddress(this CreateStoreAddressRequest request, Guid storeId, Guid createdBy, Guid updatedBy)
        {
            return StoreAddress.Create(
                storeId,
                Phone.Create(request.Phone),
                FullName.Create(request.ContactName),
                Province.Create(request.Province),
                District.Create(request.District),
                Ward.Create(request.Ward),
                Street.Create(request.Street),
                (StoreAddressType)request.Type,
                IsDefault.Create(request.IsDefault),
                createdBy,
                updatedBy);
        }

        public static void ApplyUpdate(this StoreAddress address, UpdateStoreAddressRequest request, Guid updatedBy)
        {
            address.UpdatePhone(Phone.Create(request.Phone));
            address.UpdateContactName(FullName.Create(request.ContactName));
            address.UpdateProvince(Province.Create(request.Province));
            address.UpdateDistrict(District.Create(request.District));
            address.UpdateWard(Ward.Create(request.Ward));
            address.UpdateStreet(Street.Create(request.Street));
            address.UpdateType((StoreAddressType)request.Type);
            address.UpdateIsDefault(IsDefault.Create(request.IsDefault));
            address.SetUpdatedBy(updatedBy);
        }
    }
}
