using System;

namespace Ecomm.Domain.Abstractions
{
    public interface IDatetimeService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
