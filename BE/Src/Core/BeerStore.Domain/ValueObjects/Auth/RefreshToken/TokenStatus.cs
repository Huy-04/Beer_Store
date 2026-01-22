using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.RefreshToken
{
    public class TokenStatus : EnumBase<StatusEnum>
    {
        private TokenStatus(StatusEnum value) : base(value)
        {
        }

        public static TokenStatus Create(StatusEnum value)
        {
            RuleValidator.CheckRules<RefreshTokenField>(new IBusinessRule<RefreshTokenField>[]
            {
                new EnumValidateRule<StatusEnum, RefreshTokenField>(value,
                    Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList(),
                    RefreshTokenField.TokenStatus)
            });
            return new TokenStatus(value);
        }

        public static TokenStatus Active => Create(StatusEnum.Active);
        public static TokenStatus Inactive => Create(StatusEnum.Inactive);
    }
}
