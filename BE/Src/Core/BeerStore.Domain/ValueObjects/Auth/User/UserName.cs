using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.User
{
    public class UserName : NameBase
    {
        public static readonly UserName System = new("system");
        public static readonly UserName Administrator = new("admin");

        private UserName(string value) : base(value)
        {
        }

        public static UserName Create(string value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new StringMaxLength<UserField>(value,69,UserField.UserName),
                new StringNotEmpty<UserField>(value,UserField.UserName),
                new StringMinimum<UserField>(value,5,UserField.UserName),
            });
            return new UserName(value);
        }
    }
}
