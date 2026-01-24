using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.User.Status
{
    public class UserStatus : EnumBase<StatusEnum>
    {
        private UserStatus(StatusEnum value) : base(value)
        {
        }

        public static UserStatus Create(StatusEnum value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new EnumValidateRule<StatusEnum, UserField>(value,
                    Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList(),
                    UserField.UserStatus)
            });
            return new UserStatus(value);
        }
    }
}
