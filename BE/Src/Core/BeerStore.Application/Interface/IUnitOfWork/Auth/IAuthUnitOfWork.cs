using Application.Core.Interface.IUnitOfWork;
using BeerStore.Domain.IRepository.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.IRepository.Auth.Read.Junction;
using BeerStore.Domain.IRepository.Auth.Write;
using BeerStore.Domain.IRepository.Auth.Write.Junction;

namespace BeerStore.Application.Interface.IUnitOfWork.Auth
{
    public interface IAuthUnitOfWork : IUnitOfWorkGeneric
    {
        // User
        IRUserRepository RUserRepository { get; }

        IWUserRepository WUserRepository { get; }

        // Role
        IRRoleRepository RRoleRepository { get; }

        IWRoleRepository WRoleRepository { get; }

        // Permission
        IRPermissionRepository RPermissionRepository { get; }

        IWPermissionRepository WPermissionRepository { get; }

        // UserAddress
        IRUserAddressRepository RUserAddressRepository { get; }

        IWUserAddressRepository WUserAddressRepository { get; }

        // RefreshToken
        IRRefreshTokenRepository RRefreshTokenRepository { get; }

        IWRefreshTokenRepository WRefreshTokenRepository { get; }

        // UserRole
        IRUserRoleRepository RUserRoleRepository { get; }

        IWUserRoleRepository WUserRoleRepository { get; }

        // RolePermission
        IRRolePermissionRepository RRolePermissionRepository { get; }

        IWRolePermissionRepository WRolePermissionRepository { get; }

        // UserPermission
        IRUserPermissionRepository RUserPermissionRepository { get; }

        IWUserPermissionRepository WUserPermissionRepository { get; }
    }
}
