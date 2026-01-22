using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.User
{
    public class Password : StringBase
    {
        public const int MinLength = 8;
        public const int MaxLength = 255;

        private Password(string value) : base(value)
        {
        }

        public static Password Create(string value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new StringNotEmpty<UserField>(value, UserField.Password),
                new StringMinimum<UserField>(value, MinLength, UserField.Password),
                new StringMaxLength<UserField>(value, MaxLength, UserField.Password),
                new StringContainsUppercase<UserField>(value, UserField.Password),
                new StringContainsLowercase<UserField>(value, UserField.Password),
                new StringContainsDigit<UserField>(value, UserField.Password),
            });
            return new Password(value);
        }
    }
}

