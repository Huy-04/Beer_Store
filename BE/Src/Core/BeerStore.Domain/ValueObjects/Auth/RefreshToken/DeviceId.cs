using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.RefreshToken
{
    public class DeviceId : StringBase
    {
        public const int MaxLength = 69;

        private DeviceId(string value) : base(value)
        {
        }

        public static DeviceId Create(string value)
        {
            RuleValidator.CheckRules<RefreshTokenField>(new IBusinessRule<RefreshTokenField>[]
            {
                new StringNotEmpty<RefreshTokenField>(value, RefreshTokenField.DeviceId),
                new StringMaxLength<RefreshTokenField>(value, MaxLength, RefreshTokenField.DeviceId),
            });
            return new DeviceId(value);
        }
    }
}
