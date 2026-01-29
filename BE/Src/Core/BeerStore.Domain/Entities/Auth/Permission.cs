using BeerStore.Domain.ValueObjects.Auth.Permission;
using BeerStore.Domain.ValueObjects.Auth.Permission.Enums;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Domain.Entities.Auth
{
    public class Permission : AggregateRoot
    {
        public PermissionName PermissionName { get; private set; }

        public Module Module { get; private set; }

        public Operation Operation { get; private set; }

        public Description Description { get; private set; }



        private Permission()
        { }

        private Permission(Guid id, PermissionName permissionName, Module module, Operation operation, Description description, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            PermissionName = permissionName;
            Module = module;
            Operation = operation;
            Description = description;
            SetCreationAudit(createdBy, updatedBy);
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


    }
}
