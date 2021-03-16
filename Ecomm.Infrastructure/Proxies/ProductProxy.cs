using Ecomm.Application.Common.DTOs;
using Ecomm.Application.Common.Interfaces;
using Ecomm.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Infrastructure.Proxies
{
    /// <summary>
    /// Communication with external Product service
    /// </summary>
    public class ProductProxy : IProductProxy
    {
        private readonly HttpClient _client;

        public ProductProxy(HttpClient client)
        {
            _client = client;
        }

        public async Task<ProductDto> GetProductWithDiscountsAsync(int productId, CancellationToken cancellationToken = default)
        {
            //omitted for brevity
            // _client.GetAsync()
            return Products.FirstOrDefault(p => p.Id == productId);
        }

        public IEnumerable<ProductDto> Products
            => new List<ProductDto>
            {
                new ProductDto
                {
                    Id = 1,
                    ImageBytes = null,
                    Name = "Asos T-shirt",
                    Price = 21,
                    Currency = Currency.EUR,
                    Discounts = new List<DiscountDto>
                    {
                        new DiscountDto{Code="Fidelity10",Percentage=10},
                        new DiscountDto{Code="BlackFriday30",Percentage=30}
                    }
                },
                new ProductDto
                {
                    Id = 2,
                    ImageBytes = null,
                    Name = "Dolce & gabana jeans",
                    Price = 99.95m,
                    Currency = Currency.RON,
                    Discounts = new List<DiscountDto>
                    {
                        new DiscountDto{Code="Fidelity10",Percentage=10},
                        new DiscountDto{Code="BlackFriday30",Percentage=30}
                    }
                },
                new ProductDto
                {
                    Id = 3,
                    ImageBytes = null,
                    Name = "Calvin Klein jeans",
                    Price = 39.99M,
                    Currency = Currency.EUR,
                    Discounts = new List<DiscountDto>
                    {
                        new DiscountDto{Code="Fidelity10",Percentage=10},
                        new DiscountDto{Code="BlackFriday30",Percentage=30}
                    }
                },
                new ProductDto
                {
                    Id = 4,
                    ImageBytes = null,
                    Name = "Jack & Jones Vest",
                    Price = 121,
                    Currency = Currency.EUR,
                    Discounts = new List<DiscountDto>
                    {
                        new DiscountDto{Code="Fidelity10",Percentage=10},
                        new DiscountDto{Code="BlackFriday30",Percentage=30}
                    }
                },
                new ProductDto
                {
                    Id = 5,
                    ImageBytes = null,
                    Name = "Le Coq Sportif shoes",
                    Price = 255,
                    Currency = Currency.USD,
                    Discounts = new List<DiscountDto>
                    {
                        new DiscountDto{Code="Fidelity10",Percentage=10},
                        new DiscountDto{Code="BlackFriday30",Percentage=30}
                    }
                },
            };
    }
}
