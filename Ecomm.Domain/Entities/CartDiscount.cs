using Ecomm.Domain.ValueObjects;
using System;

namespace Ecomm.Domain.Entities
{
    public class CartDiscount : Discount
    {
        private CartDiscount() : base() { }
        public CartDiscount(string code, double percentage)
           : base(code, new Percentage(percentage))
        {
        }

        public CartDiscount(string code, Percentage percentage)
            : base(code, percentage)
        {
        }

        public Guid CartId { get; private set; }
    }
}
