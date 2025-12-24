using Application.Core.IUnitOfWork;
using BeerStore.Domain.IRepository.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.IRepository.Auth.Write;

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

        // Address

        IRAddressRepository RAddressRepository { get; }

        IWAddressRepository WAddressRepository { get; }

        // UserRole
        IRUserRoleRepository RUserRoleRepository { get; }

        IWUserRoleRepository WUserRoleRepository { get; }

        // UserAddress
        IRUserAddressRepository RUserAddressRepository { get; }

        IWUserAddressRepository WUserAddressRepository { get; }

        // RolePermission
        IRRolePermissionRepository RRolePermissionRepository { get; }

        IWRolePermissionRepository WRolePermissionRepository { get; }
    }
}