using Ecomm.Domain.Entities;
using Ecomm.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Infrastructure.Initializer
{
    public static class DatabaseInitializer 
    {
        public static async Task RunAsync(string connectionString, bool seedDefaultData = false, CancellationToken cancellationToken = default)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<CartContext>(o => o.UseSqlite(connectionString,
                opt => opt.MigrationsAssembly(typeof(ShoppingCartEntityTypeConfiguration).Assembly.FullName)));

            using var provider = services.BuildServiceProvider();
            using var context = provider.GetRequiredService<CartContext>();

            await context.Database.MigrateAsync(cancellationToken);

            if (seedDefaultData)
            {
                await SeedDefaultDataAsync(context, cancellationToken);
            }
        }

        private static async Task SeedDefaultDataAsync(CartContext context, CancellationToken cancellationToken)
        {
            if (!await context.Carts.AnyAsync(cancellationToken))
            {
                var cart = new ShoppingCart(Guid.NewGuid());
                await context.Carts.AddAsync(cart, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
