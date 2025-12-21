using Application.Core.IUnitOfWork;
using BeerStore.Domain.IRepository.Auth;
using BeerStore.Domain.IRepository.Auth.Read;

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
    }
}