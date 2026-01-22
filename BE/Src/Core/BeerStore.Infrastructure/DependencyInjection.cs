using Application.Core.Interface.ISettings;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.IRepository.Auth.Read.Junction;
using BeerStore.Domain.IRepository.Auth.Write;
using BeerStore.Domain.IRepository.Auth.Write.Junction;
using BeerStore.Infrastructure.Persistence.Db;
using BeerStore.Infrastructure.Repository.Auth.Read;
using BeerStore.Infrastructure.Repository.Auth.Read.Junction;
using BeerStore.Infrastructure.Repository.Auth.Write;
using BeerStore.Infrastructure.Repository.Auth.Write.Junction;
using BeerStore.Infrastructure.Services;
using BeerStore.Infrastructure.Services.Authorization;
using BeerStore.Infrastructure.UnitOfWork;
using Infrastructure.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeerStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                var cs = configuration.GetConnectionString("BeerStoreDatabase");
                options.UseSqlServer(cs);
                options.EnableSensitiveDataLogging(false);
            });

            // User
            services.AddScoped<IRUserRepository, RUserRepository>();
            services.AddScoped<IWUserRepository, WUserRepository>();

            // Role
            services.AddScoped<IRRoleRepository, RRoleRepository>();
            services.AddScoped<IWRoleRepository, WRoleRepository>();

            // Permissions
            services.AddScoped<IRPermissionRepository, RPermissionRepository>();
            services.AddScoped<IWPermissionRepository, WPermissionRepository>();

            // UserAddress
            services.AddScoped<IRUserAddressRepository, RUserAddressRepository>();
            services.AddScoped<IWUserAddressRepository, WUserAddressRepository>();

            // RefreshToken
            services.AddScoped<IRRefreshTokenRepository, RRefreshTokenRepository>();
            services.AddScoped<IWRefreshTokenRepository, WRefreshTokenRepository>();

            // UserRole
            services.AddScoped<IRUserRoleRepository, RUserRoleRepository>();
            services.AddScoped<IWUserRoleRepository, WUserRoleRepository>();

            // RolePermission
            services.AddScoped<IRRolePermissionRepository, RRolePermissionRepository>();
            services.AddScoped<IWRolePermissionRepository, WRolePermissionRepository>();

            // Auth Unit Of Work
            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

            // JWT Service
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IJwtSettings, JwtSettings>();

            // Password Hasher
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Current User Context
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();

            // Authorization Services
            services.AddScoped<IAuthAuthorizationService, AuthAuthorizationService>();

            return services;
        }
    }
}
