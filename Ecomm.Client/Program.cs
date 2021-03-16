using Ecomm.Lib.WebApi.Http;
using Ecomm.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Ecomm.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
              .Build();
            var services = new ServiceCollection();

            services.AddLogging(builder => builder.AddConsole())
                .AddMvcCdnHttpClient<EcommHttpClient, EcommHttpClient>(configuration);
            var provider = services.BuildServiceProvider();

            var client = provider.GetRequiredService<EcommHttpClient>();
            Console.WriteLine("This was tested only for happy path... so errors might occur");
            Console.WriteLine("Type one of the following: create-cart, get-cart, add-item, remove-item");
            while (true)
            {
                Console.Write("Option=");
                var key = Console.ReadLine();
                string cartId;
                string productId;
                int quantity;
                ApiResponse<object> response;
                switch (key)
                {
                    case "create-cart":
                        response = client.PostAsync<object>("/api/v1/cart", null).Result;
                        break;
                    case "get-cart":
                        Console.Write("Cart id=");
                        cartId = Console.ReadLine();
                        Console.WriteLine();
                        response = client.GetAsync<object>($"/api/v1/cart/{cartId}").Result;
                        break;
                    case "add-item":
                        Console.Write("Cart id=");
                        cartId = Console.ReadLine();
                        Console.WriteLine();
                        Console.Write("Product id=");
                        productId = Console.ReadLine();
                        Console.WriteLine();
                        Console.Write("Quantity=");
                        quantity = Convert.ToInt32(Console.ReadLine());
                        response = client.PostAsync<object>($"/api/v1/cart/{cartId}/items", new { ProductId = productId, Quantity = quantity }).Result;
                        break;

                    case "remove-item":
                        Console.Write("Cart id=");
                        cartId = Console.ReadLine();
                        Console.WriteLine();
                        Console.Write("Product id=");
                        productId = Console.ReadLine();
                        Console.WriteLine();
                        response = client.DeleteAsync<object>($"/api/v1/cart/{cartId}/items/{productId}").Result;
                        break;
                    default:
                        Console.WriteLine("Option not found. It is case sensitive");
                        continue;
                }
                Console.WriteLine(JsonConvert.SerializeObject(response,Formatting.Indented));

            }

        }
    }
}
