using Ardalis.GuardClauses;
using Ecomm.Domain.ValueObjects;
using System;

namespace Ecomm.Domain.Entities
{
    public abstract class Discount : BaseEntity<Guid>
    {
        protected Discount() { }
        protected Discount(string code, Percentage percentage)
        {
            Code = Guard.Against.NullOrWhiteSpace(code, nameof(code));
            Percentage = Guard.Against.Null(percentage, nameof(percentage));
        }

        public string Code { get; protected set; }
        public Percentage Percentage { get; protected set; }

        public virtual void UpdatePercentage(Percentage percentage)
        {
            Percentage = percentage;
        }
    }
}
