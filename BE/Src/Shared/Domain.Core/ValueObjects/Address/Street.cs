using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace Domain.Core.ValueObjects.Address
{
    public class Street : StringBase
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