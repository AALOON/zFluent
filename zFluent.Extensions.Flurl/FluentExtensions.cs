using System;
using System.Threading.Tasks;
using Flurl.Http;

namespace zFluent.Extensions.Flurl
{
    public static class FluentExtensions
    {
        /// <summary>
        /// Executes retry function with argument type IFlurlRequest
        /// </summary>
        /// <typeparam name="TResult">Type of Argument of extension</typeparam>
        /// <param name="context">Context which proceeded by WithRetryFunc</param>
        /// <param name="func">The function which will be retried</param>
        /// <returns>Result of the function or throw some exception</returns>
        public static TResult Retry<TResult>(this IFluentContext<IFlurlRequest, TResult> context,
            Func<IFlurlRequest, TResult> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            return context.RetryFunc(context.Argument, func);
        }

        /// <summary>
        /// Setups retry function
        /// </summary>
        /// <typeparam name="TResult">Type of Result of the function</typeparam>
        /// <param name="argument">Base argument from which extension started</param>
        /// <param name="func">Retry function</param>
        /// <returns>Retry retry context</returns>
        /// <exception cref="ArgumentNullException">Func cannot be null</exception>
        public static IFluentContext<IFlurlRequest, TResult> WithRetryFunc<TResult>(this IFlurlRequest argument,
            Func<IFlurlRequest, Func<IFlurlRequest, TResult>, TResult> func)
        {
            return argument.WithRetryFunc<IFlurlRequest, TResult>(func);
        }

        /// <summary>
        /// Executes async retry function with argument type IFlurlRequest
        /// </summary>
        /// <typeparam name="TResult">Type of Result of the function</typeparam>
        /// <param name="context">Context which proceeded by WithRetryFuncAsync</param>
        /// <param name="func">The function which will be retried</param>
        /// <returns>Result of the function or throw some exception</returns>
        public static Task<TResult> RetryAsync<TResult>(this IFluentContextAsync<IFlurlRequest, TResult> context,
            Func<IFlurlRequest, Task<TResult>> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            return context.RetryFunc(context.Argument, func);
        }

        /// <summary>
        /// Setups async retry function with argument type IFlurlRequest
        /// </summary>
        /// <typeparam name="TResult">Type of Result of the function</typeparam>
        /// <param name="argument">Base argument from which extension started</param>
        /// <param name="func">Retry function</param>
        /// <returns>Retry async retry context</returns>
        public static IFluentContextAsync<IFlurlRequest, TResult> WithRetryFuncAsync<TResult>(this IFlurlRequest argument,
            Func<IFlurlRequest, Func<IFlurlRequest, Task<TResult>>, Task<TResult>> func)
        {
            return argument.WithRetryFuncAsync<IFlurlRequest, TResult>(func);
        }
    }
}
