using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.Address
{
    public class IsDefault : EnumBase<StatusEnum>
    {
        private IsDefault(StatusEnum value) : base(value)
        {
        }

        public static IsDefault Create(StatusEnum value)
        {
            RuleValidator.CheckRules<UserAddressField>(new IBusinessRule<UserAddressField>[]
            {
                new EnumValidateRule<StatusEnum, UserAddressField>(value,
                Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList(),UserAddressField.IsDefault)
            });
            return new IsDefault(value);
        }
    }
}