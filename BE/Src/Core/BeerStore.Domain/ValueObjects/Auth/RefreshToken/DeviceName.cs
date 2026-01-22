using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.RefreshToken
{
    public class DeviceName : StringBase
    {
        public const int MaxLength = 36;

        private DeviceName(string value) : base(value)
        {
        }

        public static DeviceName Create(string value)
        {
            RuleValidator.CheckRules<RefreshTokenField>(new IBusinessRule<RefreshTokenField>[]
            {
                new StringNotEmpty<RefreshTokenField>(value, RefreshTokenField.DeviceName),
                new StringMaxLength<RefreshTokenField>(value, MaxLength, RefreshTokenField.DeviceName),
            });
            return new DeviceName(value);
        }
    }
}
