
using Ecomm.Domain.ValueObjects;

namespace Ecomm.Domain.Abstractions
{
    public interface IAuditableEntity
    {
        public Audit Audit { get; }
    }
}
