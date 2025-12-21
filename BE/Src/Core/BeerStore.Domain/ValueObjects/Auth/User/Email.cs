using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.User
{
    public class Email : NameBase
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
