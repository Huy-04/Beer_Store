namespace BeerStore.Application.Interface.Services.Authorization
{
    public interface IAuthAuthorizationService
    {
        // User
        void EnsureCanReadAllUsers();
        void EnsureCanReadUser(Guid targetUserId);
        void EnsureCanCreateUser();
        void EnsureCanUpdateUser(Guid targetUserId);
        void EnsureCanRemoveUser();

        // Role
        void EnsureCanReadRole();
        void EnsureCanCreateRole();
        void EnsureCanUpdateRole();
        void EnsureCanRemoveRole();

        // Permission
        void EnsureCanReadPermission();
        void EnsureCanCreatePermission();
        void EnsureCanUpdatePermission();
        void EnsureCanRemovePermission();

        // Address
        void EnsureCanReadAllAddresses();
        Task EnsureCanReadAddress(Guid addressId);
        void EnsureCanCreateAddress(Guid targetUserId);
        Task EnsureCanUpdateAddress(Guid addressId);
        Task EnsureCanRemoveAddress(Guid addressId);

        // RefreshToken
        void EnsureCanReadAllRefreshTokens();
        void EnsureCanReadRefreshToken(Guid targetUserId);
        void EnsureCanCreateRefreshToken(Guid targetUserId);
        void EnsureCanRevokeRefreshToken(Guid targetUserId);

        // UserRole (Junction)
        void EnsureCanReadUserRole();
        void EnsureCanAddUserRole();
        void EnsureCanRemoveUserRole();

        // RolePermission (Junction)
        void EnsureCanReadRolePermission();
        void EnsureCanAddRolePermission();
        void EnsureCanRemoveRolePermission();
    }
}


