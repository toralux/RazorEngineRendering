using System;
using System.Collections.Generic;
using System.Text;

namespace Rendering.Models
{
    public class BusinessError
    {
        public int ErrorCode { get; private set; }
        public string Message { get; private set; }

        public BusinessError(string message, int errorCode = 0)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }

    public class BusinessNotFoundError : BusinessError
    {
        public string Id { get; private set; }

        public BusinessNotFoundError(string message, int errorCode, string id) : base (message, errorCode)
        {
            Id = id;
        }
    }

    public class BusinessValidationError : BusinessError
    {
        public IList<ValidationError> ValidationErrors { get; private set; }

        public BusinessValidationError(string message, int errorCode, IList<ValidationError> validationErrors) : base(message, errorCode)
        {
            ValidationErrors = validationErrors ?? new List<ValidationError>();
        }
    }

    public class ValidationError
    {
        public string Key { get; private set; }
        public string ErrorMessage { get; private set; }

        public ValidationError(string key, string errorMessage)
        {
            Key = key;
            ErrorMessage = errorMessage;
        }
    }
}

