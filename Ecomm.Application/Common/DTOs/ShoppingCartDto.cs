using System;
using System.Collections.Generic;

namespace Ecomm.Application.Common.DTOs
{
    public class ShoppingCartDto
    {
        public Guid CustomerId { get; set; }
        public decimal CartTotalPrice { get; set; }
        public decimal CartTotalPriceWithDiscount { get; set; }
        public List<CartItemDto> Items { get; set; }
    }
}
