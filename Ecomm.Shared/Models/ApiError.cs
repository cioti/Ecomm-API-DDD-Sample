using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecomm.Shared.Models
{
    public class ApiError
    {
        public ApiError() { }

        public ApiError(object message)
        {
            Message = message;
        }

        public ApiError(string message, IEnumerable<ValidationError> validationErrors)
        {
            Message = message;
            ValidationErrors = validationErrors;
        }

        public object Message { get; set; }
        public string Details { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }

        public override string ToString()
        {
            if (!ValidationErrors.Any())
            {
                return Message.ToString();
            }
            var msg = Message + Environment.NewLine;
            foreach (var error in ValidationErrors)
            {
                msg += error.ToString();
            }
            return msg;
        }
    }
}
