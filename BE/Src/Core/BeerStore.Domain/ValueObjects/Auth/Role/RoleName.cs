using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Auth.Role
{
    public class RoleName : StringBase
    {
        public static readonly RoleName Customer = new("Customer");
        public static readonly RoleName Administrator = new("Administrator");
        public static readonly RoleName Manager = new("Manager");
        public static readonly RoleName System = new("System");

        private RoleName(string value) : base(value)
        {
        }

        public static RoleName Create(string value)
        {
            RuleValidator.CheckRules<RoleField>(new IBusinessRule<RoleField>[]
            {
                new StringMaxLength<RoleField>(value,69,RoleField.RoleName),
                new StringNotEmpty<RoleField>(value,RoleField.RoleName),
                new StringMinimum<RoleField>(value,5,RoleField.RoleName)
            });
            return new RoleName(value);
        }
    }
}
