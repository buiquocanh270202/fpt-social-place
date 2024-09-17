using Domain.Enums;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UnauthorizedException : UnauthorizedAccessException
    {
        public StatusCodeEnum StatusCode { get; set; }
        public object? Data { get; set; }

        public UnauthorizedException(StatusCodeEnum statusCode) : base(statusCode.GetDescription())
        {
            StatusCode = statusCode;
        }
        public UnauthorizedException(StatusCodeEnum statusCode, object data) : base(statusCode.GetDescription())
        {
            StatusCode = statusCode;
            Data = data;
        }
        public UnauthorizedException(StatusCodeEnum statusCode, string customMessage) : base(customMessage)
        {
            StatusCode = statusCode;
        }
        public UnauthorizedException(string message) : base(message) { }
    }
}
