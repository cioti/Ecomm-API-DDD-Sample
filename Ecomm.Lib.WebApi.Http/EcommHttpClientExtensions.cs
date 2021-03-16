using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Polly;

namespace Ecomm.Lib.WebApi.Http
{
    public static class EcommHttpClientExtensions
    {
         public static void AddCdnHttpClient<IClient, TCLient>(this IServiceCollection services, IConfiguration configuration)
            where IClient : class
            where TCLient : EcommHttpClient, IClient
        {
            Configure<IClient, TCLient>(services, configuration);
        }

        public static void AddMvcCdnHttpClient<IClient, TCLient>(this IServiceCollection services, IConfiguration configuration)
            where IClient : class
            where TCLient : EcommHttpClient, IClient
        {
            var clientBuilder = Configure<IClient, TCLient>(services, configuration);
            //clientBuilder.AddHeaderPropagation();
            ConfigureMvc(services, clientBuilder);
        }

        private static void ConfigureMvc(IServiceCollection services, IHttpClientBuilder clientBuilder)
        {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            //services.AddHeaderPropagation(options =>
            //{
            //    options.Headers.Add("Cookie");
            //});
            //clientBuilder.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        }

        private static IHttpClientBuilder Configure<IClient, TClient>(IServiceCollection services, IConfiguration configuration)
            where IClient : class
            where TClient : EcommHttpClient, IClient
        {
            IConfigurationSection clientSection = null;

            if (configuration.GetChildren().Any(item => item.Key.Equals("HttpClient")))
            {
                clientSection = configuration.GetSection("HttpClient");
            }
            if (configuration.GetChildren().Any(item => item.Key.Equals(typeof(TClient).Name)))
            {
                clientSection = configuration.GetSection(typeof(TClient).Name);
            }

            //set default values.  Replace if present
            HttpClientSettings clientSettings = new HttpClientSettings();
            if (clientSection != null)
            {
                clientSettings = clientSection.Get<HttpClientSettings>();
            }
            if (clientSettings == null)
            {
                throw new ArgumentNullException(nameof(HttpClientSettings));
            }
            var clientBuilder = services.AddHttpClient<IClient, TClient>(cfg =>
            {
                cfg.BaseAddress = new Uri(clientSettings.Url);
            });
            //set retry policy if configured
            if (clientSettings.RetryCount > 0)
            {
                clientBuilder.AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(clientSettings.RetryCount, _ => TimeSpan.FromMilliseconds(clientSettings.RetryDelayInMs)));
            }
            clientBuilder.SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return clientBuilder;
        }
    }
}
