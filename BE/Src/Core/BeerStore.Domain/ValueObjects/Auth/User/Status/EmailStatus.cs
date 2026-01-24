using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.User.Status
{
    public class EmailStatus : EnumBase<StatusEnum>
    {
        private EmailStatus(StatusEnum value) : base(value)
        {
        }

        public static EmailStatus Create(StatusEnum value)
        {
            RuleValidator.CheckRules<UserField>(new IBusinessRule<UserField>[]
            {
                new EnumValidateRule<StatusEnum, UserField>(value,
                    Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList(),
                    UserField.EmailStatus)
            });
            return new EmailStatus(value);
        }
    }
}
