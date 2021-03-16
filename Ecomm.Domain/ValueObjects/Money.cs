using Ecomm.Domain.Common.Enums;
using System;
using System.Collections.Generic;

namespace Ecomm.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        private Money() { }

        public Money(Currency currency, decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Money value cannot be negative");
            }
            Currency = currency;
            Value = value;
        }

        public Currency Currency { get; }
        public decimal Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Value;
        }
    }
}
