using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.User
{
    public class FullName : NameBase
    {
        private FullName(string value) : base(value)
        {
        }

        public static FullName Create(string value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new StringMaxLength<UserField>(value,69,UserField.FullName),
                new StringNotEmpty<UserField>(value,UserField.FullName),
                new StringMinimum<UserField>(value,5,UserField.FullName),
            });
            return new FullName(value);
        }
    }
}
