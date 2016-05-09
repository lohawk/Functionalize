using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Functionalize.Errors
{
    public abstract class Error
    {
        public virtual string Description { get; } = "An error has occurred";
    }

    public class ErrorFactory
    {
        protected static ErrorConfig Config { get; set; }

        public static T Create<T>(params object[] ctorParams) where T : Error
        {
            if (!Config.ErrorTypes.Contains(typeof (T)))
                throw new NotImplementedException("The error type has not been registered with this factory");

            return (T) Activator.CreateInstance(typeof (T), ctorParams);
        }

        public static ErrorConfig Configure() => Config ?? (Config = new ErrorConfig());
    }

    public class ErrorConfig
    {
        internal List<Type> ErrorTypes { get; set; } = new List<Type>();

        public ErrorConfig WithErrors(IEnumerable<Type> errors)
        {
            ErrorTypes.AddRange(errors);
            return this;
        }

        public ErrorConfig WithErrors(params Type[] errors)
        {
            ErrorTypes.AddRange(errors);
            return this;
        }

        public ErrorConfig WithErrorsFrom(params Assembly[] assemblies)
        {
            ErrorTypes.AddRange(
                assemblies.SelectMany(a => a.GetTypes().Where(t => typeof (Error).IsAssignableFrom(t))));
            return this;
        }
    }
}