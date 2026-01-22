using Domain.Core.Base;
using Domain.Core.Enums;

namespace BeerStore.Domain.Entities.Auth.Junction
{
    public class UserPermission : Entity
    {
        public Guid UserId { get; private set; }
        public Guid PermissionId { get; private set; }
        public StatusEnum Status { get; private set; }

        private UserPermission()
        {
        }

        private UserPermission(Guid id, Guid userId, Guid permissionId, StatusEnum status)
            : base(id)
        {
            UserId = userId;
            PermissionId = permissionId;
            Status = status;
        }

        public static UserPermission Create(Guid userId, Guid permissionId, StatusEnum status = StatusEnum.Active)
        {
            return new UserPermission(Guid.NewGuid(), userId, permissionId, status);
        }

        public void Activate()
        {
            Status = StatusEnum.Active;
        }

        public void Deactivate()
        {
            Status = StatusEnum.Inactive;
        }
    }
}
