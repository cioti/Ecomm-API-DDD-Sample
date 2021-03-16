using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;

namespace Ecomm.Domain.ValueObjects
{
    public class Price : ValueObject
    {
        private Price() { }
        public Price(Money gross, Money net, Money effective)
        {
            if (gross.Currency != net.Currency ||
                gross.Currency != effective.Currency)
            {
                throw new InvalidOperationException("Price currency must be the same for all values.");
            }
            Gross = Guard.Against.Null(gross, nameof(gross));
            Net = Guard.Against.Null(net, nameof(net));
            Effective = Guard.Against.Null(effective, nameof(effective));
        }

        public Money Gross { get; private set; }
        public Money Net { get; private set; }
        public Money Effective { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Gross;
            yield return Net;
            yield return Effective;
        }
    }
}
