using BeerStore.Domain.Enums;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.Permission.Enums
{
    public class Module : EnumBase<ModuleEnum>
    {
        private Module(ModuleEnum value) : base(value)
        {
        }

        public static Module Create(ModuleEnum value)
        {
            RuleValidator.CheckRules<PermissionField>(new IBusinessRule<PermissionField>[]
            {
                new EnumValidateRule<ModuleEnum, PermissionField>(value,
                    Enum.GetValues(typeof(ModuleEnum)).Cast<ModuleEnum>().ToList(),
                    PermissionField.Module)
            });
            return new Module(value);
        }
    }
}
