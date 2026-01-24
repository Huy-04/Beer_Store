using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace Domain.Core.ValueObjects.Address
{
    public class Phone : StringBase
    {
        private Phone(string value) : base(value)
        {
        }

        public static Phone Create(string value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new StringMaxLength<AddressField>(value, 18, AddressField.Phone),
                new StringNotEmpty<AddressField>(value, AddressField.Phone),
                new StringMinimum<AddressField>(value, 5, AddressField.Phone),
            });
            return new Phone(value);
        }
    }
}
