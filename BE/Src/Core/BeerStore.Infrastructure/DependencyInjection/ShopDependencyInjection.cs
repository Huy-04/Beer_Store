using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.IRepository.Shop.Read;
using BeerStore.Domain.IRepository.Shop.Write;
using BeerStore.Infrastructure.Persistence.Db;
using BeerStore.Infrastructure.Repository.Shop.Read;
using BeerStore.Infrastructure.Repository.Shop.Write;
using BeerStore.Infrastructure.Services.Shop.Authorization;
using BeerStore.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeerStore.Infrastructure
{
    public static class ShopDependencyInjection
    {
        public static IServiceCollection AddShopInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ShopDb"));
                options.EnableSensitiveDataLogging(false);
            });

            // Store
            services.AddScoped<IRStoreRepository, RStoreRepository>();
            services.AddScoped<IWStoreRepository, WStoreRepository>();

            // StoreAddress
            services.AddScoped<IRStoreAddressRepository, RStoreAddressRepository>();
            services.AddScoped<IWStoreAddressRepository, WStoreAddressRepository>();

            // Shop Unit Of Work
            services.AddScoped<IShopUnitOfWork, ShopUnitOfWork>();

            // Authorization Service
            services.AddScoped<IShopAuthorizationService, ShopAuthorizationService>();

            return services;
        }
    }
}