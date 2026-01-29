using BeerStore.Domain.ValueObjects.Auth.Permission;
using BeerStore.Domain.ValueObjects.Auth.Permission.Enums;
using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using BeerStore.Domain.ValueObjects.Auth.Role;
using BeerStore.Domain.ValueObjects.Auth.User;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using Domain.Core.Enums;
using BeerStore.Domain.Enums.Auth;
using BeerStore.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using BeerStore.Domain.ValueObjects.Auth.UserAddress;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter
{
    public static class AuthConverter
    {
        #region User

        public static readonly ValueConverter<UserName, string>
            UserNameConverter = new(v => v.Value, v => UserName.Create(v));

        public static readonly ValueConverter<Password, string>
            PasswordConverter = new(v => v.Value, v => Password.Create(v));

        public static readonly ValueConverter<UserStatus, int>
            UserStatusConverter = new(v => (int)v.Value, v => UserStatus.Create((StatusEnum)v));

        public static readonly ValueConverter<PhoneStatus, int>
            PhoneStatusConverter = new(v => (int)v.Value, v => PhoneStatus.Create((StatusEnum)v));

        public static readonly ValueConverter<EmailStatus, int>
            EmailStatusConverter = new(v => (int)v.Value, v => EmailStatus.Create((StatusEnum)v));

        #endregion User

        #region Role

        public static readonly ValueConverter<RoleName, string>
            RoleNameConverter = new(v => v.Value, v => RoleName.Create(v));

        #endregion Role

        #region Permission

        public static readonly ValueConverter<PermissionName, string>
            PermissionNameConverter = new(v => v.Value, v => PermissionName.Create(v));

        public static readonly ValueConverter<Module, int>
            ModuleConverter = new(v => (int)v.Value, v => Module.Create((ModuleEnum)v));

        public static readonly ValueConverter<Operation, int>
            OperationConverter = new(v => (int)v.Value, v => Operation.Create((OperationEnum)v));

        #endregion Permission

        #region UserAddress

        public static readonly ValueConverter<UserAddressType, int>
            UserAddressTypeConverter = new(v => (int)v.Value, v => UserAddressType.Create((AddressTypeEnum)v));

        #endregion UserAddress

        #region RefreshToken

        public static readonly ValueConverter<TokenHash, string>
            TokenHashConverter = new(v => v.Value, v => TokenHash.Create(v));

        public static readonly ValueConverter<DeviceId, string>
            DeviceIdConverter = new(v => v.Value, v => DeviceId.Create(v));

        public static readonly ValueConverter<DeviceName, string>
            DeviceNameConverter = new(v => v.Value, v => DeviceName.Create(v));

        public static readonly ValueConverter<TokenStatus, int>
            TokenStatusConverter = new(v => (int)v.Value, v => TokenStatus.Create((StatusEnum)v));

        #endregion RefreshToken
    }
}