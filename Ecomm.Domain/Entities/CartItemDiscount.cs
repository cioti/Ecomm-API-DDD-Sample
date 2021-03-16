using Ecomm.Domain.ValueObjects;
using System;

namespace Ecomm.Domain.Entities
{
    public class CartItemDiscount : Discount
    {
        private CartItemDiscount() : base() { }
        public CartItemDiscount(string code, double percentage) : base(code, new Percentage(percentage)) { }
        public CartItemDiscount(string code, Percentage percentage) : base(code, percentage) { }
        public int CartItemId { get; private set; }
    }
}
