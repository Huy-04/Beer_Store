using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Address
{
    public class Province : NameBase
    {
        private Province(string value) : base(value)
        {
        }

        public static Province Create(string value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new StringMaxLength<AddressField>(value,69,AddressField.Province),
                new StringNotEmpty<AddressField>(value,AddressField.Province)
            });
            return new Province(value);
        }
    }
}