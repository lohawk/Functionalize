using System;
using System.Linq;
using Functionalize.Errors;
using NullGuard;

namespace Functionalize.Result
{
    public class Result
    {
        [AllowNull]
        public bool Success { get; private set; }

        [AllowNull]
        public Error Error { get; private set; }

        public bool Failure => !Success;

        protected Result(bool success, [AllowNull] Error error)
        {
            Success = success;
            Error = error;
        }

        [Obsolete]
        public static Result Fail(Error error) => new Result(false, error);

        public static Result Fail<TErr>(params object[] ctorParams) where TErr : Error
            => new Result(false, ErrorFactory.Create<TErr>(ctorParams));

        [Obsolete]
        public static Result<T> Fail<T>(Error error) => new Result<T>(default(T), false, error);

        public static Result Fail<TSucc, TErr>(params object[] ctorParams) where TErr : Error
            => new Result<TSucc>(default(TSucc), false, ErrorFactory.Create<TErr>(ctorParams));

        public static Result Ok() => new Result(true, null);

        public static Result<T> Ok<T>(T value) => new Result<T>(value, true, null);

        public static Result Combine(params Result[] results) => results.FirstOrDefault(r => r.Failure) ?? Ok();
    }

    public class Result<T> : Result
    {
        private T _value;

        [AllowNull]
        public T Value
        {
            get { return _value; }
            private set { _value = value; }
        }

        protected internal Result([AllowNull] T value, bool success, [AllowNull] Error error)
            : base(success, error)
        {
            Value = value;
        }
    }
}