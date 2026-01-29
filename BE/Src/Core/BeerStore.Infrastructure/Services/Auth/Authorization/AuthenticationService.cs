using BeerStore.Application.Interface.IUnitOfWork.Auth;
using Application.Core.Interface.Services;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Auth.Messages;
using BeerStore.Domain.Enums.Auth.Messages.Junction;
using static Domain.Core.Helpers.AuthorizationHelper;
using BeerStore.Domain.Constants.Permission;

namespace BeerStore.Infrastructure.Services.Auth.Authorization
{
    public class AuthenticationService : IAuthAuthorizationService
    {
        private readonly ICurrentUserContext _currentUser;
        private readonly IAuthUnitOfWork _auow;

        public AuthenticationService(ICurrentUserContext currentUser, IAuthUnitOfWork auow)
        {
            _currentUser = currentUser;
            _auow = auow;
        }

        #region User

        public void EnsureCanReadAllUsers()
        {
            if (_currentUser.HasPermission(AuthConstant.User.ReadAll)) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanReadUser(Guid targetUserId)
        {
            if (_currentUser.HasPermission(AuthConstant.User.ReadAll)) return;
            if (_currentUser.HasPermission(AuthConstant.User.ReadSelf) && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanCreateUser()
        {
            if (_currentUser.HasPermission(AuthConstant.User.CreateAll)) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanUpdateUser(Guid targetUserId)
        {
            if (_currentUser.HasPermission(AuthConstant.User.UpdateAll)) return;
            if (_currentUser.HasPermission(AuthConstant.User.UpdateSelf) && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(UserField.IdUser);
        }

        public void EnsureCanRemoveUser()
        {
            if (_currentUser.HasPermission(AuthConstant.User.RemoveAll)) return;

            ThrowForbidden(UserField.IdUser);
        }

        #endregion User

        #region Role

        public void EnsureCanReadRole()
        {
            if (_currentUser.HasPermission(AuthConstant.Role.ReadAll)) return;

            ThrowForbidden(RoleField.IdRole);
        }

        public void EnsureCanCreateRole()
        {
            if (_currentUser.HasPermission(AuthConstant.Role.CreateAll)) return;

            ThrowForbidden(RoleField.IdRole);
        }

        public void EnsureCanUpdateRole()
        {
            if (_currentUser.HasPermission(AuthConstant.Role.UpdateAll)) return;

            ThrowForbidden(RoleField.IdRole);
        }

        public void EnsureCanRemoveRole()
        {
            if (_currentUser.HasPermission(AuthConstant.Role.RemoveAll)) return;

            ThrowForbidden(RoleField.IdRole);
        }

        #endregion Role

        #region Permission

        public void EnsureCanReadPermission()
        {
            if (_currentUser.HasPermission(AuthConstant.Permission.ReadAll)) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        public void EnsureCanCreatePermission()
        {
            if (_currentUser.HasPermission(AuthConstant.Permission.CreateAll)) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        public void EnsureCanUpdatePermission()
        {
            if (_currentUser.HasPermission(AuthConstant.Permission.UpdateAll)) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        public void EnsureCanRemovePermission()
        {
            if (_currentUser.HasPermission(AuthConstant.Permission.RemoveAll)) return;

            ThrowForbidden(PermissionField.IdPermission);
        }

        #endregion Permission

        #region Address

        public void EnsureCanReadAllAddresses()
        {
            if (_currentUser.HasPermission(AuthConstant.Address.ReadAll)) return;

            ThrowForbidden(UserAddressField.IdAddress);
        }

        public async Task EnsureCanReadAddress(Guid addressId)
        {
            if (_currentUser.HasPermission(AuthConstant.Address.ReadAll)) return;

            if (_currentUser.HasPermission(AuthConstant.Address.ReadSelf))
            {
                var address = await _auow.RUserAddressRepository.GetByIdAsync(addressId);
                if (address?.UserId == _currentUser.UserId) return;
            }

            ThrowForbidden(UserAddressField.IdAddress);
        }

        public void EnsureCanCreateAddress(Guid targetUserId)
        {
            if (_currentUser.HasPermission(AuthConstant.Address.CreateAll)) return;
            if (_currentUser.HasPermission(AuthConstant.Address.CreateSelf) && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(UserAddressField.IdAddress);
        }

        public async Task EnsureCanUpdateAddress(Guid addressId)
        {
            if (_currentUser.HasPermission(AuthConstant.Address.UpdateAll)) return;

            if (_currentUser.HasPermission(AuthConstant.Address.UpdateSelf))
            {
                var address = await _auow.RUserAddressRepository.GetByIdAsync(addressId);
                if (address?.UserId == _currentUser.UserId) return;
            }

            ThrowForbidden(UserAddressField.IdAddress);
        }

        public async Task EnsureCanRemoveAddress(Guid addressId)
        {
            if (_currentUser.HasPermission(AuthConstant.Address.RemoveAll)) return;

            if (_currentUser.HasPermission(AuthConstant.Address.RemoveSelf))
            {
                var address = await _auow.RUserAddressRepository.GetByIdAsync(addressId);
                if (address?.UserId == _currentUser.UserId) return;
            }

            ThrowForbidden(UserAddressField.IdAddress);
        }

        #endregion Address

        #region RefreshToken

        public void EnsureCanReadAllRefreshTokens()
        {
            if (_currentUser.HasPermission(AuthConstant.RefreshToken.ReadAll)) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        public void EnsureCanReadRefreshToken(Guid targetUserId)
        {
            if (_currentUser.HasPermission(AuthConstant.RefreshToken.ReadAll)) return;
            if (_currentUser.HasPermission(AuthConstant.RefreshToken.ReadSelf) && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        public void EnsureCanCreateRefreshToken(Guid targetUserId)
        {
            if (_currentUser.HasPermission(AuthConstant.RefreshToken.CreateAll)) return;
            if (_currentUser.HasPermission(AuthConstant.RefreshToken.CreateSelf) && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        public void EnsureCanRevokeRefreshToken(Guid targetUserId)
        {
            if (_currentUser.HasPermission(AuthConstant.RefreshToken.RevokeAll)) return;
            if (_currentUser.HasPermission(AuthConstant.RefreshToken.RevokeSelf) && _currentUser.UserId == targetUserId) return;

            ThrowForbidden(RefreshTokenField.IdRefreshToken);
        }

        #endregion RefreshToken

        #region UserRole (Junction)

        public void EnsureCanReadUserRole()
        {
            if (_currentUser.HasPermission(AuthConstant.UserRole.ReadAll)) return;

            ThrowForbidden(UserRoleField.UserId);
        }

        public void EnsureCanAddUserRole()
        {
            if (_currentUser.HasPermission(AuthConstant.UserRole.CreateAll)) return;

            ThrowForbidden(UserRoleField.UserId);
        }

        public void EnsureCanRemoveUserRole()
        {
            if (_currentUser.HasPermission(AuthConstant.UserRole.RemoveAll)) return;

            ThrowForbidden(UserRoleField.UserId);
        }

        #endregion UserRole (Junction)

        #region RolePermission (Junction)

        public void EnsureCanReadRolePermission()
        {
            if (_currentUser.HasPermission(AuthConstant.RolePermission.ReadAll)) return;

            ThrowForbidden(RolePermissionField.RoleId);
        }

        public void EnsureCanAddRolePermission()
        {
            if (_currentUser.HasPermission(AuthConstant.RolePermission.CreateAll)) return;

            ThrowForbidden(RolePermissionField.RoleId);
        }

        public void EnsureCanRemoveRolePermission()
        {
            if (_currentUser.HasPermission(AuthConstant.RolePermission.RemoveAll)) return;

            ThrowForbidden(RolePermissionField.RoleId);
        }

        #endregion RolePermission (Junction)
    }
}