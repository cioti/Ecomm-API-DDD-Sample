using Ecomm.Domain.Common.Enums;
using System;
using System.Collections.Generic;

namespace Ecomm.Domain.ValueObjects
{
    public class Amount : ValueObject
    {
        private Amount() { }
        public Amount(Unit unit, decimal value)
        {
            if(value < 0)
            {
                throw new ArgumentException($"Amount must be a non-negative number.");
            }

            if(unit == Unit.Piece && value % 1 != 0)
            {
                throw new ArgumentException($"Amount value must be a natural number for unit of type '{Unit.Piece}'");
            }

            Value = value;
            Unit = unit;
        }

        public decimal Value { get; private set; }
        public Unit Unit { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Unit;
        }
    }
}
