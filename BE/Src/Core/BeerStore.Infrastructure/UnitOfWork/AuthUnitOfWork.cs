using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.IRepository.Auth.Read.Junction;
using BeerStore.Domain.IRepository.Auth.Write;
using BeerStore.Domain.IRepository.Auth.Write.Junction;
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
            IWPermissionRepository wPermissionRepo,
            IRUserAddressRepository rUserAddressRepo,
            IWUserAddressRepository wUserAddressRepo,
            IRRefreshTokenRepository rRefreshTokenRepo,
            IWRefreshTokenRepository wRefreshTokenRepo,
            IRUserRoleRepository rUserRoleRepo,
            IWUserRoleRepository wUserRoleRepo,
            IRRolePermissionRepository rRolePermissionRepo,
            IWRolePermissionRepository wRolePermissionRepo) : base(context)
        {
            RUserRepository = rUserRepo;
            WUserRepository = wUserRepo;

            RRoleRepository = rRoleRepo;
            WRoleRepository = wRoleRepo;

            RPermissionRepository = rPermissionRepo;
            WPermissionRepository = wPermissionRepo;

            RUserAddressRepository = rUserAddressRepo;
            WUserAddressRepository = wUserAddressRepo;

            RRefreshTokenRepository = rRefreshTokenRepo;
            WRefreshTokenRepository = wRefreshTokenRepo;

            RUserRoleRepository = rUserRoleRepo;
            WUserRoleRepository = wUserRoleRepo;

            RRolePermissionRepository = rRolePermissionRepo;
            WRolePermissionRepository = wRolePermissionRepo;
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

        // UserAddress
        public IRUserAddressRepository RUserAddressRepository { get; }

        public IWUserAddressRepository WUserAddressRepository { get; }

        // RefreshToken
        public IRRefreshTokenRepository RRefreshTokenRepository { get; }

        public IWRefreshTokenRepository WRefreshTokenRepository { get; }

        // UserRole
        public IRUserRoleRepository RUserRoleRepository { get; }

        public IWUserRoleRepository WUserRoleRepository { get; }

        // RolePermission
        public IRRolePermissionRepository RRolePermissionRepository { get; }

        public IWRolePermissionRepository WRolePermissionRepository { get; }
    }
}
