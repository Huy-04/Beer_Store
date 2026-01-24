using Domain.Core.Enums;
using Domain.Core.ValueObjects.Address;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Core.PropertyConverters
{
    public static class AddressConverter
    {
        #region Location
        public static readonly ValueConverter<Province, string>
            ProvinceConverter = new(v => v.Value, v => Province.Create(v));

        public static readonly ValueConverter<District, string>
            DistrictConverter = new(v => v.Value, v => District.Create(v));

        public static readonly ValueConverter<Ward, string>
            WardConverter = new(v => v.Value, v => Ward.Create(v));

        public static readonly ValueConverter<Street, string>
            StreetConverter = new(v => v.Value, v => Street.Create(v));
        #endregion

        #region Contact
        public static readonly ValueConverter<Phone, string>
            PhoneConverter = new(v => v.Value, v => Phone.Create(v));

        public static readonly ValueConverter<FullName, string>
            FullNameConverter = new(v => v.Value, v => FullName.Create(v));
        #endregion

        #region Status
        public static readonly ValueConverter<IsDefault, int>
            IsDefaultConverter = new(v => (int)v.Value, v => IsDefault.Create((StatusEnum)v));
        #endregion
    }
}
