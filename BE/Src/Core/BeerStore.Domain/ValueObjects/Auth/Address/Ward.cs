using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Address
{
    public class Ward : NameBase
    {
        private Ward(string value) : base(value)
        {
        }

        public static Ward Create(string value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new StringMaxLength<AddressField>(value,69,AddressField.Ward),
                new StringNotEmpty<AddressField>(value,AddressField.Ward),
            });
            return new Ward(value);
        }
    }
}