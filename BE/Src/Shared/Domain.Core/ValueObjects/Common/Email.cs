using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace Domain.Core.ValueObjects.Common
{
    public class Email : StringBase
    {
        private Email(string value) : base(value)
        {
        }

        public static Email Create(string value)
        {
            RuleValidator.CheckRules<CommonField>(new IBusinessRule<CommonField>[]
            {
                new StringMaxLength<CommonField>(value,100,CommonField.Email),
                new StringNotEmpty<CommonField>(value,CommonField.Email),
                new StringMinimum<CommonField>(value,5,CommonField.Email),
            });
            return new Email(value);
        }
    }
}
