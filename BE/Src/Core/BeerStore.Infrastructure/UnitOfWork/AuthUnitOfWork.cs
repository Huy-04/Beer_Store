using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Domain.IRepository.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.IRepository.Auth.Write;
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
            IRAddressRepository rAddressRepo,
            IWAddressRepository wAddressRepo,
            IRUserRoleRepository rUserRoleRepo,
            IWUserRoleRepository wUserRoleRepo,
            IRUserAddressRepository rUserAddressRepo,
            IWUserAddressRepository wUserAddressRepo,
            IRRolePermissionRepository rRolePermissionRepo,
            IWRolePermissionRepository wRolePermissionRepo) : base(context)
        {
            RUserRepository = rUserRepo;
            WUserRepository = wUserRepo;

            RRoleRepository = rRoleRepo;
            WRoleRepository = wRoleRepo;

            RPermissionRepository = rPermissionRepo;
            WPermissionRepository = wPermissionRepo;

            RAddressRepository = rAddressRepo;
            WAddressRepository = wAddressRepo;

            RUserRoleRepository = rUserRoleRepo;
            WUserRoleRepository = wUserRoleRepo;

            RUserAddressRepository = rUserAddressRepo;
            WUserAddressRepository = wUserAddressRepo;

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

        // Address
        public IRAddressRepository RAddressRepository { get; }

        public IWAddressRepository WAddressRepository { get; }

        // UserRole
        public IRUserRoleRepository RUserRoleRepository { get; }

        public IWUserRoleRepository WUserRoleRepository { get; }

        // UserAddress
        public IRUserAddressRepository RUserAddressRepository { get; }

        public IWUserAddressRepository WUserAddressRepository { get; }

        // RolePermission
        public IRRolePermissionRepository RRolePermissionRepository { get; }

        public IWRolePermissionRepository WRolePermissionRepository { get; }
    }
}