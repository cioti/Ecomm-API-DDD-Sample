using Ecomm.Domain.Common.Enums;
using System.Collections.Generic;

namespace Ecomm.Application.Common.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public byte[] ImageBytes { get; set; }
        public List<DiscountDto> Discounts { get; set; }
    }
}
