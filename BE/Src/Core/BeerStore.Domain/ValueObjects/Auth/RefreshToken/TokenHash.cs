using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.RefreshToken
{
    public class TokenHash : StringBase
    {
        public const int MaxLength = 69;

        private TokenHash(string value) : base(value)
        {
        }

        public static TokenHash Create(string value)
        {
            RuleValidator.CheckRules<RefreshTokenField>(new IBusinessRule<RefreshTokenField>[]
            {
                new StringNotEmpty<RefreshTokenField>(value, RefreshTokenField.TokenHash),
                new StringMaxLength<RefreshTokenField>(value, MaxLength, RefreshTokenField.TokenHash),
            });
            return new TokenHash(value);
        }
    }
}
