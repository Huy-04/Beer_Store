using BeerStore.Domain.Enums.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.ValueObjects.Auth.Permission
{
    public class PermissionName : StringBase
    {
        private PermissionName(string value) : base(value)
        {
        }

        public static PermissionName Create(string value)
        {
            RuleValidator.CheckRules<PermissionField>(new IBusinessRule<PermissionField>[]
            {
                new StringMaxLength<PermissionField>(value,69,PermissionField.PermissionName),
                new StringNotEmpty<PermissionField>(value,PermissionField.PermissionName),
                new StringMinimum<PermissionField>(value,5,PermissionField.PermissionName),
            });
            return new PermissionName(value);
        }
    }
}
