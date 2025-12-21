using BeerStore.Domain.ValueObjects.Auth.Permissions;
using BeerStore.Domain.ValueObjects.Auth.Permissions.Enums;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.Entities.Auth
{
    public class Permission : AggregateRoot
    {
        public PermissionName PermissionName { get; private set; }

        public Module Module { get; private set; }

        public Operation Operation { get; private set; }

        public Description Description { get; private set; }

        public Guid CreatedBy { get; private set; }

        public Guid UpdatedBy { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private Permission()
        { }

        private Permission(Guid id, PermissionName permissionName, Module module, Operation operation, Description description, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            PermissionName = permissionName;
            Module = module;
            Operation = operation;
            Description = description;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Permission Create(PermissionName permissionName, Module module, Operation operation, Description description, Guid createdBy, Guid updatedBy)
        {
            var permission = new Permission(Guid.NewGuid(), permissionName, module, operation, description, createdBy, updatedBy);
            return permission;
        }

        public void UpdatePermissionName(PermissionName permissionName)
        {
            if (PermissionName == permissionName) return;
            PermissionName = permissionName;
            Touch();
        }

        public void UpdateModule(Module module)
        {
            if (Module == module) return;
            Module = module;
            Touch();
        }

        public void UpdateOperation(Operation operation)
        {
            if (Operation == operation) return;
            Operation = operation;
            Touch();
        }

        public void UpdateDescription(Description description)
        {
            if (Description == description) return;
            Description = description;
            Touch();
        }

        public void SetUpdatedBy(Guid updatedBy)
        {
            if (UpdatedBy == updatedBy) return;
            UpdatedBy = updatedBy;
            Touch();
        }

        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}