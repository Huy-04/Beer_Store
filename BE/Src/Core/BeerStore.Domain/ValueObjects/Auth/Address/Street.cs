using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Address
{
    public class Street : NameBase
    {
        private Street(string value) : base(value)
        {
        }

        public static Street Create(string value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new StringMaxLength<AddressField>(value,255,AddressField.Street),
                new StringNotEmpty<AddressField>(value,AddressField.Street)
            });
            return new Street(value);
        }
    }
}