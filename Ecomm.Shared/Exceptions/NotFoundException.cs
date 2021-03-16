using System;

namespace Ecomm.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string notFoundResource = "")
            : base(string.IsNullOrWhiteSpace(notFoundResource)
                ? "Resource not found." : $"'{notFoundResource}' resource not found.")
        {
        }
    }
}
