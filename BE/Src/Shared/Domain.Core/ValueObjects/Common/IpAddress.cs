using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace Domain.Core.ValueObjects.Common
{
    public class IpAddress : StringBase
    {
        public const int MaxLength = 69;

        private IpAddress(string value) : base(value)
        {
        }

        public static IpAddress Create(string value)
        {
            RuleValidator.CheckRules<CommonField>(new IBusinessRule<CommonField>[]
            {
                new StringNotEmpty<CommonField>(value, CommonField.IpAddress),
                new StringMaxLength<CommonField>(value, MaxLength, CommonField.IpAddress),
            });
            return new IpAddress(value);
        }
    }
}
