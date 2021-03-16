using Ecomm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecomm.Shared.Exceptions
{
    public class ValidationException : Exception
    {
        private const string ErrorMessage = "Validation error occured.";
        public IEnumerable<ValidationError> Errors { get; }
        public ValidationException(IEnumerable<ValidationError> errors)
            : base(ErrorMessage)
        {
            Errors = errors;
        }

        public ValidationException(ValidationError error)
            : base(ErrorMessage)
        {
            Errors = new List<ValidationError> { error };
        }

        public ValidationException(string propertyName, string reason)
            : base(ErrorMessage)
        {
            Errors = new List<ValidationError> { new ValidationError(propertyName, reason) };
        }

        public ValidationException(string reason)
            : base(ErrorMessage)
        {
            Errors = new List<ValidationError> { new ValidationError(reason) };
        }

        public override string ToString() => string.Join("; ", Errors.Select(e => e.ToString()));
    }
}
