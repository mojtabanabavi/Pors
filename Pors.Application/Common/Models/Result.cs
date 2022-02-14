using System;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class Result
    {
        public bool IsSucceeded { get; set; }
        public string[] Errors { get; set; }

        public Result(bool isSucceeded)
        {
            IsSucceeded = isSucceeded;
        }

        public Result(bool isSucceeded, IEnumerable<string> errors) : this(isSucceeded)
        {
            Errors = errors.ToArray();
        }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(params string[] errors)
        {
            return new Result(false, errors);
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }

    public class Result<TData> : Result
    {
        public TData Data { get; set; }

        public Result(bool success, TData data) : base(success)
        {
            Data = data;
        }

        public Result(bool success, TData data, IEnumerable<string> errors) : base(success, errors)
        {
            Data = data;
        }

        public static Result<TData> Success(TData data)
        {
            return new Result<TData>(true, data);
        }

        public new static Result<TData> Failure(params string[] errors)
        {
            return new Result<TData>(false, default, errors);
        }

        public new static Result<TData> Failure(IEnumerable<string> errors)
        {
            return new Result<TData>(false, default, errors);
        }
    }
}