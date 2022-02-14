using System;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class Result
    {
        public bool IsSucceeded { get; set; }
        public string[] Errors { get; set; }

        public Result(bool isSucceeded, IEnumerable<string> errors)
        {
            Errors = errors.ToArray();
            IsSucceeded = isSucceeded;
        }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}
