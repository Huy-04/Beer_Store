using BeerStore.Domain.Enums;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Permission
{
    public class Operation : EnumBase<OperationEnum>
    {
        private Operation(OperationEnum value) : base(value)
        {
        }

        public static Operation Create(OperationEnum value)
        {
            RuleValidator.CheckRules<PermissionField>(new IBusinessRule<PermissionField>[]
            {
                new EnumValidateRule<OperationEnum, PermissionField>(value,
                    Enum.GetValues(typeof(OperationEnum)).Cast<OperationEnum>().ToList(),
                    PermissionField.Action)
            });
            return new Operation(value);
        }
    }
}
