using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.User.Status
{
    public class PhoneStatus : EnumBase<StatusEnum>
    {
        private PhoneStatus(StatusEnum value) : base(value)
        {
        }

        public static PhoneStatus Create(StatusEnum value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new EnumValidateRule<StatusEnum, UserField>(value,
                    Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList(),
                    UserField.PhoneStatus)
            });
            return new PhoneStatus(value);
        }
    }
}
