using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.User
{
    public class Phone : NameBase
    {
        private Phone(string value) : base(value)
        {
        }

        public static Phone Create(string value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new StringMaxLength<UserField>(value,18,UserField.Phone),
                new StringNotEmpty<UserField>(value,UserField.Phone),
                new StringMinimum<UserField>(value,5,UserField.Phone),
            });
            return new Phone(value);
        }
    }
}
