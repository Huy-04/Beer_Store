using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.RefreshToken
{
    public class IpAddress : StringBase
    {
        public const int MaxLength = 69;

        private IpAddress(string value) : base(value)
        {
        }

        public static IpAddress Create(string value)
        {
            RuleValidator.CheckRules<RefreshTokenField>(new IBusinessRule<RefreshTokenField>[]
            {
                new StringNotEmpty<RefreshTokenField>(value, RefreshTokenField.IpAddress),
                new StringMaxLength<RefreshTokenField>(value, MaxLength, RefreshTokenField.IpAddress),
            });
            return new IpAddress(value);
        }
    }
}
