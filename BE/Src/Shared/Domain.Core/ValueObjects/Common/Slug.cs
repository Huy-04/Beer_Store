using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace Domain.Core.ValueObjects.Common
{
    public class Slug : StringBase
    {
        private Slug(string value) : base(value)
        {
        }

        public static Slug Create(string value)
        {
            RuleValidator.CheckRules<CommonField>(new IBusinessRule<CommonField>[]
            {
                new StringNotEmpty<CommonField>(value, CommonField.Slug),
                new StringMaxLength<CommonField>(value, 100, CommonField.Slug),
                new StringPattern<CommonField>(value, StringPattern<CommonField>.DEFAULT_PATTERN, CommonField.Slug)
            });
            return new Slug(value);
        }
    }
}