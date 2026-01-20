using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Messages;
using static Domain.Core.Helpers.AuthorizationHelper;

namespace BeerStore.Infrastructure.Services.Authorization
{
    public class AuthAuthorizationService : IAuthAuthorizationService
    {
        private readonly ICurrentUserContext _currentUser;
        private readonly IAuthUnitOfWork _auow;

        public AuthAuthorizationService(ICurrentUserContext currentUser, IAuthUnitOfWork auow)
        {
            _currentUser = currentUser;
            _auow = auow;
        }

        #region User

        public void EnsureCanReadAllUsers()
        {
            if (_currentUser.HasPermission("User.Read.All")) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanReadUser(Guid targetUserId)
        {
            if (_currentUser.HasPermission("User.Read.All")) return;
            if (_currentUser.HasPermission("User.Read.Self") && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanCreateUser()
        {
            if (_currentUser.HasPermission("User.Create.All")) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanUpdateUser(Guid targetUserId)
        {
            if (_currentUser.HasPermission("User.Update.All")) return;
            if (_currentUser.HasPermission("User.Update.Self") && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanRemoveUser()
        {
            if (_currentUser.HasPermission("User.Remove.All")) return;

            ThrowForbidden(UserField.IdUser);
        }

        #endregion

        #region Role

        public void EnsureCanReadRole()
        {
            if (_currentUser.HasPermission("Role.Read.All")) return;

            ThrowForbidden(RoleField.IdRole);
        }

        public void EnsureCanCreateRole()
        {
            if (_currentUser.HasPermission("Role.Create.All")) return;

            ThrowForbidden(RoleField.IdRole);
        }

        public void EnsureCanUpdateRole()
        {
            if (_currentUser.HasPermission("Role.Update.All")) return;

            ThrowForbidden(RoleField.IdRole);
        }

        public void EnsureCanRemoveRole()
        {
            if (_currentUser.HasPermission("Role.Remove.All")) return;

            ThrowForbidden(RoleField.IdRole);
        }

        #endregion

        #region Permission

        public void EnsureCanReadPermission()
        {
            if (_currentUser.HasPermission("Permission.Read.All")) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        public void EnsureCanCreatePermission()
        {
            if (_currentUser.HasPermission("Permission.Create.All")) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        public void EnsureCanUpdatePermission()
        {
            if (_currentUser.HasPermission("Permission.Update.All")) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        public void EnsureCanRemovePermission()
        {
            if (_currentUser.HasPermission("Permission.Remove.All")) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        #endregion

        #region Address

        public void EnsureCanReadAllAddresses()
        {
            if (_currentUser.HasPermission("Address.Read.All")) return;

            ThrowForbidden(AddressField.IdAddress);
        }

        public async Task EnsureCanReadAddress(Guid addressId)
        {
            if (_currentUser.HasPermission("Address.Read.All")) return;

            if (_currentUser.HasPermission("Address.Read.Self"))
            {
                var address = await _auow.RAddressRepository.GetByIdAsync(addressId);
                if (address?.UserId == _currentUser.UserId) return;
            }

            ThrowForbidden(AddressField.IdAddress);
        }

        public void EnsureCanCreateAddress(Guid targetUserId)
        {
            if (_currentUser.HasPermission("Address.Create.All")) return;
            if (_currentUser.HasPermission("Address.Create.Self") && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(AddressField.IdAddress);
        }

        public async Task EnsureCanUpdateAddress(Guid addressId)
        {
            if (_currentUser.HasPermission("Address.Update.All")) return;

            if (_currentUser.HasPermission("Address.Update.Self"))
            {
                var address = await _auow.RAddressRepository.GetByIdAsync(addressId);
                if (address?.UserId == _currentUser.UserId) return;
            }

            ThrowForbidden(AddressField.IdAddress);
        }

        public async Task EnsureCanRemoveAddress(Guid addressId)
        {
            if (_currentUser.HasPermission("Address.Remove.All")) return;

            if (_currentUser.HasPermission("Address.Remove.Self"))
            {
                var address = await _auow.RAddressRepository.GetByIdAsync(addressId);
                if (address?.UserId == _currentUser.UserId) return;
            }

            ThrowForbidden(AddressField.IdAddress);
        }

        #endregion

        #region RefreshToken

        public void EnsureCanReadAllRefreshTokens()
        {
            if (_currentUser.HasPermission("RefreshToken.Read.All")) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        public void EnsureCanReadRefreshToken(Guid targetUserId)
        {
            if (_currentUser.HasPermission("RefreshToken.Read.All")) return;
            if (_currentUser.HasPermission("RefreshToken.Read.Self") && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        public void EnsureCanCreateRefreshToken(Guid targetUserId)
        {
            if (_currentUser.HasPermission("RefreshToken.Create.All")) return;
            if (_currentUser.HasPermission("RefreshToken.Create.Self") && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        public void EnsureCanRevokeRefreshToken(Guid targetUserId)
        {
            if (_currentUser.HasPermission("RefreshToken.Revoke.All")) return;
            if (_currentUser.HasPermission("RefreshToken.Revoke.Self") && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        #endregion

        #region UserRole (Junction)

        public void EnsureCanReadUserRole()
        {
            if (_currentUser.HasPermission("UserRole.Read.All")) return;

            ThrowForbidden(UserRoleField.UserId);
        }

        public void EnsureCanAddUserRole()
        {
            if (_currentUser.HasPermission("UserRole.Create.All")) return;

            ThrowForbidden(UserRoleField.UserId);
        }

        public void EnsureCanRemoveUserRole()
        {
            if (_currentUser.HasPermission("UserRole.Remove.All")) return;

            ThrowForbidden(UserRoleField.UserId);
        }

        #endregion

        #region RolePermission (Junction)

        public void EnsureCanReadRolePermission()
        {
            if (_currentUser.HasPermission("RolePermission.Read.All")) return;

            ThrowForbidden(RolePermissionField.RoleId);
        }

        public void EnsureCanAddRolePermission()
        {
            if (_currentUser.HasPermission("RolePermission.Create.All")) return;

            ThrowForbidden(RolePermissionField.RoleId);
        }

        public void EnsureCanRemoveRolePermission()
        {
            if (_currentUser.HasPermission("RolePermission.Remove.All")) return;

            ThrowForbidden(RolePermissionField.RoleId);
        }

        #endregion
    }
}


