using Domain.Enums;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ForbiddenException :UnauthorizedAccessException
    {
        public StatusCodeEnum StatusCode { get; set; }
        public object? Data { get; set; }

        public ForbiddenException(StatusCodeEnum statusCode) : base(statusCode.GetDescription())
        {
            StatusCode = statusCode;
        }
        public ForbiddenException(StatusCodeEnum statusCode, object data) : base(statusCode.GetDescription())
        {
            StatusCode = statusCode;
            Data = data;
        }
        public ForbiddenException(StatusCodeEnum statusCode, string customMessage) : base(customMessage)
        {
            StatusCode = statusCode;
        }
        public ForbiddenException(string message) : base(message) { }
    }
}
