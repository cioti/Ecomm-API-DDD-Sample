using Ecomm.Application.Common.Interfaces;
using Ecomm.Domain.Abstractions;
using Ecomm.Infrastructure.Proxies;
using Ecomm.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecomm.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CartContext>(opts =>
            {
                opts.UseSqlite(connectionString,
                    opts => opts.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            });
            services.AddScoped(typeof(IGenericAsyncRepository<>), typeof(GenericAsyncRepository<>));
            services.AddHttpClient<ICustomerProxy, CustomerProxy>();
            services.AddHttpClient<IProductProxy, ProductProxy>();
            services.AddSingleton<IDatetimeService, DatetimeService>();
            return services;
        }
    }
}
