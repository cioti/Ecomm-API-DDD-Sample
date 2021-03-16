using Ecomm.Api.Services;
using Ecomm.Application;
using Ecomm.Domain.Abstractions;
using Ecomm.Infrastructure;
using Ecomm.Infrastructure.Initializer;
using Ecomm.Lib.WebApi.ResponseWrapper.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Ecomm.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddInfrastructure(Configuration.GetConnectionString("SQLite"));
            services.AddApplication();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecomm.Api", Version = "v1" });
            });

             services.AddResponseWrapper(cfg =>
             {
                 cfg.ApiVersion="1.0.0.0";
                 cfg.ShowApiVersion=true;
                 cfg.IsDebug = true;
                 cfg.IgnoreNullValue = true;
                 cfg.EnableRequestLogging = true;
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecomm.Api v1"));
            }
            app.AddResponseWrapperMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DatabaseInitializer.RunAsync(Configuration.GetConnectionString("SQLite"), true).Wait();
        }
    }
}
