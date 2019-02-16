using System;
using System.Threading.Tasks;
using Flurl.Http;

namespace zFluent.Extensions.Flurl
{
    public static class FluentExtensions
    {
        public static TResult Retry<TResult>(this IFluentContext<IFlurlRequest, TResult> context,
            Func<IFlurlRequest, TResult> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            return context.RetryFunc(context.Argument, func);
        }

        public static IFluentContext<IFlurlRequest, TResult> WithRetryFunc<TResult>(this IFlurlRequest argument,
            Func<IFlurlRequest, Func<IFlurlRequest, TResult>, TResult> func)
        {
            return argument.WithRetryFunc<IFlurlRequest, TResult>(func);
        }

        public static Task<TResult> RetryAsync<TResult>(this IFluentContextAsync<IFlurlRequest, TResult> context,
            Func<IFlurlRequest, Task<TResult>> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            return context.RetryFunc(context.Argument, func);
        }

        public static IFluentContextAsync<IFlurlRequest, TResult> WithRetryFuncAsync<TResult>(this IFlurlRequest argument,
            Func<IFlurlRequest, Func<IFlurlRequest, Task<TResult>>, Task<TResult>> func)
        {
            return argument.WithRetryFuncAsync<IFlurlRequest, TResult>(func);
        }
    }
}
