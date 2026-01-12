using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.ValueObjects.Auth.Role;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.Entities.Auth
{
    public class Role : AggregateRoot
    {
        public RoleName RoleName { get; private set; }

        public Description Description { get; private set; }

        private readonly List<RolePermission> _rolePermission = new List<RolePermission>();

        public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermission.AsReadOnly();

        public Guid CreatedBy { get; private set; }

        public Guid UpdatedBy { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private Role()
        { }

        private Role(Guid id, RoleName roleName, Description description, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            RoleName = roleName;
            Description = description;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Role Create(RoleName roleName, Description description, Guid createdBy, Guid updatedBy)
        {
            var role = new Role(Guid.NewGuid(), roleName, description, createdBy, updatedBy);
            return role;
        }

        public void updateRoleName(RoleName roleName)
        {
            if (RoleName == roleName) return;
            RoleName = roleName;
            Touch();
        }

        public void updateDescription(Description description)
        {
            if (Description == description) return;
            Description = description;
            Touch();
        }

        public void SetUpdatedBy(Guid updateBy)
        {
            if (UpdatedBy == updateBy) return;
            UpdatedBy = updateBy;
            Touch();
        }

        // Permission Management
        public RolePermission AddPermission(Guid roleId, Guid permissionId)
        {
            var rolePermission = RolePermission.Create(roleId, permissionId);
            Touch();
            return rolePermission;
        }

        public void RemoveRolePermission(Guid idUserRole)
        {
            var userRole = _rolePermission.FirstOrDefault(ur => ur.Id == idUserRole);
            if (userRole == null) return;
            _rolePermission.Remove(userRole);
            Touch();
        }

        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
