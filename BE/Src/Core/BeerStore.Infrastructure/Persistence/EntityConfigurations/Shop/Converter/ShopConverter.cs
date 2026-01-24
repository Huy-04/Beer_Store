using BeerStore.Domain.Enums.Shop;
using BeerStore.Domain.ValueObjects.Shop;
using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Shop.Converter
{
    public static class ShopConverter
    {
        #region Store
        public static readonly ValueConverter<StoreName, string>
            StoreNameConverter = new(v => v.Value, v => StoreName.Create(v));

        public static readonly ValueConverter<Slug, string>
            SlugConverter = new(v => v.Value, v => Slug.Create(v));

        public static readonly ValueConverter<StoreType, int>
            StoreTypeConverter = new(v => (int)v, v => (StoreType)v);

        public static readonly ValueConverter<StoreStatus, int>
            StoreStatusConverter = new(v => (int)v, v => (StoreStatus)v);
        #endregion

        #region StoreAddress
        public static readonly ValueConverter<StoreAddressType, int>
            StoreAddressTypeConverter = new(v => (int)v, v => (StoreAddressType)v);
        #endregion
    }
}
