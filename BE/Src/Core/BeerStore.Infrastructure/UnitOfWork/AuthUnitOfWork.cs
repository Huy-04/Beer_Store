using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Domain.IRepository.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.UnitOfWork;

namespace BeerStore.Infrastructure.UnitOfWork
{
    public class AuthUnitOfWork : UnitOfWorkGeneric, IAuthUnitOfWork
    {
        public AuthUnitOfWork(
            AuthDbContext context,
            IRUserRepository rUserRepo,
            IWUserRepository wUserRepo,
            IRRoleRepository rRoleRepo,
            IWRoleRepository wRoleRepo,
            IRPermissionRepository rPermissionRepo,
            IWPermissionRepository wPermissionRepo) : base(context)
        {
            RUserRepository = rUserRepo;
            WUserRepository = wUserRepo;

            RRoleRepository = rRoleRepo;
            WRoleRepository = wRoleRepo;

            RPermissionRepository = rPermissionRepo;
            WPermissionRepository = wPermissionRepo;
        }

        // User
        public IRUserRepository RUserRepository { get; }

        public IWUserRepository WUserRepository { get; }

        // Role
        public IRRoleRepository RRoleRepository { get; }

        public IWRoleRepository WRoleRepository { get; }

        // Permission
        public IRPermissionRepository RPermissionRepository { get; }

        public IWPermissionRepository WPermissionRepository { get; }
    }
}