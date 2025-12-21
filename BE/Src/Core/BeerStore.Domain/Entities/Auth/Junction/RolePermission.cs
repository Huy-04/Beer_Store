using Domain.Core.Base;

namespace BeerStore.Domain.Entities.Auth.Junction
{
    public class RolePermission : Entity
    {
        public Guid RoleId { get; private set; }
        public Guid PermissionId { get; private set; }

        private RolePermission()
        {
        }

        private RolePermission(Guid id, Guid roleId, Guid permissionId)
            : base(id)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        internal static RolePermission Create(Guid roleId, Guid permissionId)
        {
            var rolePermission = new RolePermission(Guid.NewGuid(), roleId, permissionId);
            return rolePermission;
        }
    }
}