using Domain.Enums;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ApiModels
{
    public class ApiResponseModel
    {
        private StatusCodeEnum StatusEnum { get; set; }
        public string StatusCode { get; set; }
        public string Message {
            get
            {
                return StatusEnum.GetDescription();
            }
        }

        public object? Data { get; set; }
        public ApiResponseModel()
        {
            StatusCode = StatusCodeEnum.Success.ToString();
        }
        public ApiResponseModel(object data)
        {
            StatusCode = StatusCodeEnum.Success.ToString();
            Data = data;
        }

        public ApiResponseModel(StatusCodeEnum statusCode)
        {
            StatusCode = statusCode.ToString().Split("_")[0]; //example: ER01_Not_Fround -> ER01
            StatusEnum = statusCode;
        }

        public ApiResponseModel(StatusCodeEnum statusCode, object data)
        {
            StatusCode = statusCode.ToString().Split("_")[0]; //example: ER01_Not_Fround -> ER01
            StatusEnum = statusCode;
            Data = data;
        }
    }

    public class ApiResponseModel<T>
    {
        private StatusCodeEnum StatusEnum { get; set; }
        public string StatusCode { get; set; }
        public string Message
        {
            get
            {
                return StatusEnum.GetDescription();
            }
        }

        public T Data { get; set; }

        public ApiResponseModel(T data)
        {
            StatusCode = StatusCodeEnum.Success.ToString();
            Data = data;
        }
    }
}
