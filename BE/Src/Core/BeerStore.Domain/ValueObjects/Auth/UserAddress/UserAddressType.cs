using BeerStore.Domain.Enums;
using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.UserAddress
{
    public class UserAddressType : EnumBase<AddressTypeEnum>
    {
        private UserAddressType(AddressTypeEnum value) : base(value)
        {
        }

        public static UserAddressType Create(AddressTypeEnum value)
        {
            RuleValidator.CheckRules<UserAddressField>(new IBusinessRule<UserAddressField>[]
            {
                new EnumValidateRule<AddressTypeEnum, UserAddressField>(value,
                    Enum.GetValues(typeof(AddressTypeEnum)).Cast<AddressTypeEnum>().ToList(),UserAddressField.AddressType)
            });
            return new UserAddressType(value);
        }
    }
}