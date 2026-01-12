using Domain.Core.Base;

namespace BeerStore.Domain.Entities.Auth.Junction
{
    public class UserRole : Entity
    {
        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }

        private UserRole()
        {
        }

        private UserRole(Guid id, Guid userId, Guid roleId)
            : base(id)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public static UserRole Create(Guid userId, Guid roleId)
        {
            var userRole = new UserRole(Guid.NewGuid(), userId, roleId);
            return userRole;
        }
    }
}
