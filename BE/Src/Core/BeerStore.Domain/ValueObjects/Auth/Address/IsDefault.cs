using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Address
{
    public class IsDefault : EnumBase<StatusEnum>
    {
        private IsDefault(StatusEnum value) : base(value)
        {
        }

        public static IsDefault Create(StatusEnum value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new EnumValidateRule<StatusEnum, AddressField>(value,
                Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList(),AddressField.IsDefault)
            });
            return new IsDefault(value);
        }
    }
}
