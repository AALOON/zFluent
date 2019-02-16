using System;
using System.Threading.Tasks;

namespace zFluent.Extensions
{
    /// <summary>
    /// Helpful extensions of retry
    /// </summary>
    public static class FluentExtensions
    {
        /// <summary>
        /// Executes retry function
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <typeparam name="TResult">Type of Result of the function</typeparam>
        /// <param name="context">Context which proceeded by WithRetryFunc</param>
        /// <param name="func">The function which will be retried</param>
        /// <returns>Result of the function or throw some exception</returns>
        /// <exception cref="ArgumentNullException">Context or func cannot be null</exception>
        public static TResult Retry<TArgument, TResult>(this IFluentContext<TArgument, TResult> context,
            Func<TArgument, TResult> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            return context.RetryFunc(context.Argument, func);
        }

        /// <summary>
        /// Setups retry function
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <typeparam name="TResult">Type of Result of the function</typeparam>
        /// <param name="argument">Base argument from which extension started</param>
        /// <param name="func">Retry function</param>
        /// <returns>Retry retry context</returns>
        /// <exception cref="ArgumentNullException">Func cannot be null</exception>
        public static IFluentContext<TArgument, TResult> WithRetryFunc<TArgument, TResult>(this TArgument argument,
            Func<TArgument, Func<TArgument, TResult>, TResult> func)
        {
            return new FluentContext<TArgument, TResult>(func, argument);
        }

        /// <summary>
        /// Executes async retry function
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <typeparam name="TResult">Type of Result of the function</typeparam>
        /// <param name="context">Context which proceeded by WithRetryFuncAsync</param>
        /// <param name="func">The function which will be retried</param>
        /// <returns>Result of the function or throw some exception</returns>
        public static Task<TResult> RetryAsync<TArgument, TResult>(this IFluentContextAsync<TArgument, TResult> context,
            Func<TArgument, Task<TResult>> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            return context.RetryFunc(context.Argument, func);
        }

        /// <summary>
        /// Setups async retry function
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <typeparam name="TResult">Type of Result of the function</typeparam>
        /// <param name="argument">Base argument from which extension started</param>
        /// <param name="func">Retry function</param>
        /// <returns>Retry async retry context</returns>
        public static IFluentContextAsync<TArgument, TResult> WithRetryFuncAsync<TArgument, TResult>(this TArgument argument,
            Func<TArgument, Func<TArgument, Task<TResult>>, Task<TResult>> func)
        {
            return new FluentContextAsync<TArgument, TResult>(func, argument);
        }

        /// <summary>
        /// Executes retry action
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <param name="context">Context which proceeded by WithRetryFunc</param>
        /// <param name="func">The function which will be retried</param>
        public static void Retry<TArgument>(this IFluentContext<TArgument> context, Action<TArgument> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            context.RetryFunc(context.Argument, func);
        }

        /// <summary>
        /// Setups retry action
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <param name="argument">Base argument from which extension started</param>
        /// <param name="func">Retry function</param>
        /// <returns>Retry retry context</returns>
        public static IFluentContext<TArgument> WithRetryFunc<TArgument>(this TArgument argument,
            Action<TArgument, Action<TArgument>> func)
        {
            return new FluentContext<TArgument>(func, argument);
        }

        /// <summary>
        /// Executes async retry action
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <param name="context">Context which proceeded by WithRetryFuncAsync</param>
        /// <param name="func">The function which will be retried</param>
        public static Task RetryAsync<TArgument>(this IFluentContextAsync<TArgument> context,
            Func<TArgument, Task> func)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (func == null) throw new ArgumentNullException(nameof(func));
            return context.RetryFunc(context.Argument, func);
        }

        /// <summary>
        /// Setups async retry action
        /// </summary>
        /// <typeparam name="TArgument">Type of Argument of extension</typeparam>
        /// <param name="argument">Base argument from which extension started</param>
        /// <param name="func">Retry function</param>
        /// <returns>Retry retry context</returns>
        public static IFluentContextAsync<TArgument> WithRetryFuncAsync<TArgument>(this TArgument argument,
            Func<TArgument, Func<TArgument, Task>, Task> func)
        {
            return new FluentContextAsync<TArgument>(func, argument);
        }
    }
}
