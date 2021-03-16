using Ecomm.Domain.Abstractions;
using System;

namespace Ecomm.Infrastructure.Services
{
    public class DatetimeService : IDatetimeService
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
