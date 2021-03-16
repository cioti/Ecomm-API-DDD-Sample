using Ecomm.Lib.WebApi.Http;
using Ecomm.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.Client
{
    public class EcommApiClient : EcommHttpClient, IEcommApiClient
    {
        public EcommApiClient(HttpClient client, ILogger<EcommHttpClient> logger) : base(client, logger)
        {
        }

        public async Task<ApiResponse<string>> CreateShoppingCartAsync()
        {
            return await PostAsync<string>("/api/v1/cart", null);
        }
    }
}
