using System;
using System.Collections.Generic;

namespace Ecomm.Application.Common.DTOs
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public List< DiscountDto> Discounts { get; set; }
    }
}
