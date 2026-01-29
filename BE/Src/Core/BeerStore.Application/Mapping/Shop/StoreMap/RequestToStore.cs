using BeerStore.Application.DTOs.Shop.Store.Requests;
using BeerStore.Domain.Entities.Shop;
using BeerStore.Domain.Enums.Shop;
using BeerStore.Domain.ValueObjects.Shop;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Application.Mapping.Shop.StoreMap
{
    public static class RequestToStore
    {
        public static Store ToStore(this RegisterStoreRequest request, Guid ownerId, Guid createdBy, Guid updatedBy)
        {
            return Store.Create(
                ownerId,
                StoreName.Create(request.Name),
                Slug.Create(request.Slug),
                (StoreType)request.StoreType,
                Img.Create(request.Logo ?? "default-logo.png"),
                Description.Create(request.Description ?? "No description"),
                createdBy,
                updatedBy);
        }

        public static void ApplyUpdate(this Store store, UpdateStoreRequest request, Guid updatedBy)
        {
            store.UpdateName(StoreName.Create(request.Name));
            if (request.Logo != null)
                store.UpdateLogo(Img.Create(request.Logo));
            if (request.Description != null)
                store.UpdateDescription(Description.Create(request.Description));
            store.SetUpdatedBy(updatedBy);
        }

        public static void ApplyResubmit(this Store store, ResubmitStoreRequest request, Guid updatedBy)
        {
            store.UpdateName(StoreName.Create(request.Name));
            if (request.Logo != null)
                store.UpdateLogo(Img.Create(request.Logo));
            if (request.Description != null)
                store.UpdateDescription(Description.Create(request.Description));
            store.Resubmit();
            store.SetUpdatedBy(updatedBy);
        }
    }
}
