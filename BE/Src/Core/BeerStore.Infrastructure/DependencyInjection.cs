using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Domain.IRepository.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Infrastructure.Persistence.Db;
using BeerStore.Infrastructure.Repository.Auth.Read;
using BeerStore.Infrastructure.Repository.Auth.Write;
using BeerStore.Infrastructure.Services;
using BeerStore.Infrastructure.UnitOfWork;
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

            // Auth Unit Of Work
            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

            // JWT Service
            services.AddScoped<IJwtService, JwtService>();

            // Password Hasher
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}