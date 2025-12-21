using BeerStore.Domain.Enums;
using BeerStore.Domain.ValueObjects.Auth.Address;
using BeerStore.Domain.ValueObjects.Auth.Permissions;
using BeerStore.Domain.ValueObjects.Auth.Permissions.Enums;
using BeerStore.Domain.ValueObjects.Auth.Roles;
using BeerStore.Domain.ValueObjects.Auth.User;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using Domain.Core.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client.Extensibility;

namespace BeerStore.Infrastructure.Persistence.EntityConfigurations.Auth.Converter
{
    public static class AuthConverter
    {
        // Common

        public static readonly ValueConverter<Phone, string>
            PhoneConverter = new(v => v.Value, v => Phone.Create(v));

        public static readonly ValueConverter<FullName, string>
            FullNameConverter = new(v => v.Value, v => FullName.Create(v));

        // User
        public static readonly ValueConverter<Email, string>
            EmailConverter = new(v => v.Value, v => Email.Create(v));

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

        // Role
        public static readonly ValueConverter<RoleName, string>
            RoleNameConverter = new(v => v.Value, v => RoleName.Create(v));

        // Permission
        public static readonly ValueConverter<PermissionName, string>
            PermissionNameConverter = new(v => v.Value, v => PermissionName.Create(v));

        public static readonly ValueConverter<Module, int>
            ModuleConverter = new(v => (int)v.Value, v => Module.Create((ModuleEnum)v));

        public static readonly ValueConverter<Operation, int>
            OperationConverter = new(v => (int)v.Value, v => Operation.Create((OperationEnum)v));

        // Address
        public static readonly ValueConverter<Province, string>
            ProvinceConverter = new(v => v.Value, v => Province.Create(v));

        public static readonly ValueConverter<District, string>
            DistrictConverter = new(v => v.Value, v => District.Create(v));

        public static readonly ValueConverter<Ward, string>
            WardConverter = new(v => v.Value, v => Ward.Create(v));

        public static readonly ValueConverter<Street, string>
            StreetConverter = new(v => v.Value, v => Street.Create(v));

        public static readonly ValueConverter<IsDefault, int>
            IsDefaultConverter = new(v => (int)v.Value, v => IsDefault.Create((StatusEnum)v));

        public static readonly ValueConverter<AddressType, int>
            AddressTypeConverter = new(v => (int)v.Value, v => AddressType.Create((AddressTypeEnum)v));
    }
}