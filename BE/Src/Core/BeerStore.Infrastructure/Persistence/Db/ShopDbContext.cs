using BeerStore.Domain.Entities.Shop;
using BeerStore.Domain.Entities.Shop.Junction;
using BeerStore.Infrastructure.Persistence.EntityConfigurations.Shop;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BeerStore.Infrastructure.Persistence.Db
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreAddress> StoreAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                a => a.Namespace!.Contains(".EntityConfigurations.Shop"));
        }
    }
}
