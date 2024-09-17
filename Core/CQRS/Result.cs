using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CQRS
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
    }

    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public T Value { get; private set; }
        public string Error { get; }
        public object? Data { get; set; }

        protected Result(bool isSuccess,T value, string error, object data)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            Data = data;
        }

        public static Result<T> Success(T? value) => new(true, value, null,null);
        public static Result<T> Success(object? value) => new(true, default, null, value);
        public static Result<T> Failure(string error) => new(false, default, error,null);

    }

}
