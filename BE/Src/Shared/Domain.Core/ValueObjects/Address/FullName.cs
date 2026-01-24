using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace Domain.Core.ValueObjects.Address
{
    public class FullName : StringBase
    {
        private FullName(string value) : base(value)
        {
        }

        public static FullName Create(string value)
        {
            RuleValidator.CheckRules<AddressField>(new IBusinessRule<AddressField>[]
            {
                new StringMaxLength<AddressField>(value, 69, AddressField.FullName),
                new StringNotEmpty<AddressField>(value, AddressField.FullName),
                new StringMinimum<AddressField>(value, 5, AddressField.FullName),
            });
            return new FullName(value);
        }
    }
}
