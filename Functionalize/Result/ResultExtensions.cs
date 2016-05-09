using System;
using Functionalize.Errors;

namespace Functionalize.Result
{
    public static class ResultExtensions
    {
        public static Result OnSuccess(this Result result, Func<Result> func) => result.Failure ? result : func();

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.Failure)
                return result;

            action();
            return Result.Ok();
        }

        public static Result OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Failure)
                return result;

            action(result.Value);

            return Result.Ok();
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
            => result.Failure ? Result.Fail<T>(result.Error) : Result.Ok(func());

        public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
            => result.Failure ? Result.Fail<T>(result.Error) : func();

        public static Result<TOut> OnSuccess<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
            => result.Failure ? Result.Fail<TOut>(result.Error) : func(result.Value);

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.Failure)
            {
                action();
            }

            return result;
        }

        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action(result);
            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func) => func(result);

        public static TOut OnBoth<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> success, Func<Error, TOut> failure)
            => result.Failure ? failure(result.Error) : success(result.Value);
    }
}