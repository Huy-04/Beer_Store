using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Address
{
    public class District : NameBase
    {
        private District(string value) : base(value)
        {
        }

        public static District Create(string value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new StringMaxLength<AddressField>(value,69,AddressField.District),
                new StringNotEmpty<AddressField>(value,AddressField.District)
            });
            return new District(value);
        }
    }
}
