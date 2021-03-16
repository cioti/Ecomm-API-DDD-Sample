using Ardalis.GuardClauses;
using Ecomm.Domain.Abstractions;
using Ecomm.Domain.Common.Enums;
using Ecomm.Domain.ValueObjects;
using Ecomm.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecomm.Domain.Entities
{
    public class ShoppingCart : BaseEntity<Guid>, IAggregateRoot, IAuditableEntity
    {
        private Guid _customerId;
        private readonly List<CartItem> _items;
        private readonly List<CartDiscount> _discounts;

        private ShoppingCart() { }
        public ShoppingCart(Guid customerId)
        {
            _customerId = customerId;
            _items = new List<CartItem>();
            _discounts = new List<CartDiscount>();
        }

        public Guid CustomerId => _customerId;
        public IReadOnlyCollection<CartItem> Items => _items.ToList(); //defensive copy
        public IReadOnlyCollection<CartDiscount> Discounts => _discounts.ToList(); //defensive copy
        public Audit Audit { get; } = new Audit();
        public decimal TotalPrice => _items.Sum(ci => ci.TotalPrice);
        public decimal CalculateTotalPriceWithDiscount()
        {
            var maxDiscount = _discounts.Max(pd => pd.Percentage.Value);
            if (maxDiscount == 0)
            {
                return TotalPrice;
            }
            decimal discountValue = TotalPrice * (decimal)maxDiscount / 100;
            return TotalPrice - discountValue;
        }

        public int AddItem(int productId, string description, int quantity,
            decimal price, Currency currency, byte[] imageBytes, List<CartItemDiscount> discounts)
        {
            var item = GetItem(productId);
            if (item is not null)
            {
                item.IncreaseQuantity(quantity);
                return item.Id;
            }

            item = new CartItem(productId,
                description,
                quantity,
                new Money(currency, price),
                imageBytes,
                discounts);
            _items.Add(item);
            return item.Id;
        }

        public void RemoveItem(int productId)
        {
            var item = GetItem(productId);
            if (item is null)
            {
                throw new NotFoundException(nameof(CartItem));
            }
            _items.Remove(item);
        }

        public void AddDiscounts(List<CartDiscount> discounts)
        {
            Guard.Against.Null(discounts, nameof(discounts));
            foreach (var discount in discounts)
            {
                var existingDiscount = _discounts.Find(d => d.Code == discount.Code);
                if (existingDiscount is null)
                {
                    _discounts.Add(discount);
                    continue;
                }
                existingDiscount.UpdatePercentage(discount.Percentage);
            }
        }

        private CartItem GetItem(int productId)
            => _items.SingleOrDefault(i => i.ProductId == productId);
    }
}
