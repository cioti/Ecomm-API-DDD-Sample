using Ardalis.GuardClauses;
using Ecomm.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecomm.Domain.Entities
{
    public class CartItem : BaseEntity<int>
    {
        private readonly List<CartItemDiscount> _discounts;

        private CartItem() { }
        public CartItem(int productId, string description, int quantity,
            Money unitPrice, byte[] imageBytes, List<CartItemDiscount> discounts)
        {

            ProductId = Guard.Against.NegativeOrZero(productId, nameof(productId));
            Description = Guard.Against.NullOrWhiteSpace(description, nameof(description));
            Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            ImageBytes = imageBytes;
            UnitPrice = Guard.Against.Null(unitPrice, nameof(unitPrice));
            _discounts = discounts ?? new List<CartItemDiscount>();
        }

        public Guid CartId { get; private set; }
        public int ProductId { get; private set; }
        public string Description { get; private set; }
        public Money UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public byte[] ImageBytes { get; private set; }
        public IReadOnlyCollection<CartItemDiscount> Discounts => _discounts.ToList(); //defensive copy
        public decimal TotalPrice => UnitPrice.Value * Quantity;
        public void IncreaseQuantity(int quantity)
        {
            Quantity += Guard.Against.NegativeOrZero(quantity, nameof(quantity));
        }

        public decimal CalculateTotalPriceWithDiscount()
        {
            if (!_discounts.Any())
            {
                return TotalPrice;
            }
            var maxDiscount = _discounts.Max(pd => pd.Percentage.Value);
            if (maxDiscount == 0)
            {
                return TotalPrice;
            }
            decimal discountValue = TotalPrice * (decimal)maxDiscount / 100;
            return TotalPrice - discountValue;
        }
    }
}
