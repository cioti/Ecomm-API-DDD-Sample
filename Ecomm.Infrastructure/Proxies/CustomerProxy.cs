using Ecomm.Application.Common.DTOs;
using Ecomm.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ecomm.Infrastructure.Proxies
{
    /// <summary>
    /// Communication with external Customer service
    /// </summary>
    public class CustomerProxy : ICustomerProxy
    {
        private readonly HttpClient _client;

        public CustomerProxy(HttpClient client)
        {
            _client = client;
        }

        public async Task<CustomerDto> GetCustomerWithDiscountsAsync(string userId)
        {
            //omitted for brevity
            // _client.GetAsync()

            return new CustomerDto
            {
                Id = Guid.Parse("4932a8e4-f7bb-43f6-af57-acbc4aa55110"),
                Discounts = new List<DiscountDto>
                {
                    new DiscountDto{Code="Fidelity10",Percentage=10},
                    new DiscountDto{Code="BlackFriday30",Percentage=30}
                }
            };
        }
    }
}
