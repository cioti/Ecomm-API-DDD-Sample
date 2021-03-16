using System;
using System.Collections.Generic;

namespace Ecomm.Domain.ValueObjects
{
    public class Percentage : ValueObject
    {
        private Percentage() { }
        public Percentage(double value)
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentException("Percentage value must be between 0-100");
            }
            Value = value;
        }
        public double Value { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
