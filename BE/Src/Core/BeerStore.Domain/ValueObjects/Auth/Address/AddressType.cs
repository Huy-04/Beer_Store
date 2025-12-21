using BeerStore.Domain.Enums;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Address
{
    public class AddressType : EnumBase<AddressTypeEnum>
    {
        private AddressType(AddressTypeEnum value) : base(value)
        {
        }

        public static AddressType Create(AddressTypeEnum value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new EnumValidateRule<AddressTypeEnum, AddressField>(value,
                    Enum.GetValues(typeof(AddressTypeEnum)).Cast<AddressTypeEnum>().ToList(),AddressField.AddressType)
            });
            return new AddressType(value);
        }
    }
}
