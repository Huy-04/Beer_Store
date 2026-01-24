using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.User
{
    public class Email : StringBase
    {
        private Email(string value) : base(value)
        {
        }

        public static Email Create(string value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new StringMaxLength<UserField>(value,100,UserField.Email),
                new StringNotEmpty<UserField>(value,UserField.Email),
                new StringMinimum<UserField>(value,5,UserField.Email),
            });
            return new Email(value);
        }
    }
}
