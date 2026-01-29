using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.ValueObjects.Auth.Role;
using Domain.Core.ValueObjects.Common;


namespace BeerStore.Domain.Entities.Auth
{
    public class Role : AggregateRoot
    {
        public RoleName RoleName { get; private set; }

        public Description Description { get; private set; } // Changed RoleDescription to Description

        private readonly List<RolePermission> _rolePermission = new List<RolePermission>();

        public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermission.AsReadOnly();

        private Role()
        { }

        private Role(Guid id, RoleName roleName, Description description, Guid createdBy, Guid updatedBy) // Changed RoleDescription to Description
            : base(id)
        {
            RoleName = roleName;
            Description = description;
            SetCreationAudit(createdBy, updatedBy);
        }

        public static Role Create(RoleName roleName, Description description, Guid createdBy, Guid updatedBy) // Changed RoleDescription to Description
        {
            var role = new Role(Guid.NewGuid(), roleName, description, createdBy, updatedBy);
            return role;
        }

        public void UpdateRoleName(RoleName roleName)
        {
            if (RoleName == roleName) return;
            RoleName = roleName;
            Touch(); // Uncommented and kept Touch() as per example
        }

        public void UpdateDescription(Description description) // Changed RoleDescription to Description
        {
            if (Description == description) return;
            Description = description;
            Touch(); // Uncommented and kept Touch() as per example
        }

        // SetUpdatedBy method removed as part of manual audit logic removal

        // Permission Management
        public RolePermission AddPermission(Guid roleId, Guid permissionId)
        {
            var rolePermission = RolePermission.Create(roleId, permissionId);
            _rolePermission.Add(rolePermission);
            // Touch(); // Removed manual audit logic
            return rolePermission;
        }

        public void RemoveRolePermission(Guid idUserRole)
        {
            var userRole = _rolePermission.FirstOrDefault(ur => ur.Id == idUserRole);
            if (userRole == null) return;
            _rolePermission.Remove(userRole);
            Touch();
        }


    }
}
