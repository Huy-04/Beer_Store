using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.User
{
    public class Password : NameBase
    {
        private Password(string value) : base(value)
        {
        }

        public static Password Create(string value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new StringMaxLength<UserField>(value,255,UserField.Password),
                new StringNotEmpty<UserField>(value,UserField.Password),
                new StringMinimum<UserField>(value,5,UserField.Password),
            });
            return new Password(value);
        }
    }
}
