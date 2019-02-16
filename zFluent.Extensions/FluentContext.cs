using System;

namespace zFluent.Extensions
{
    /// <inheritdoc />
    internal class FluentContext<TArgument, TResult> : IFluentContext<TArgument, TResult>
    {
        public FluentContext(Func<TArgument, Func<TArgument, TResult>, TResult> retryFunc, TArgument argument)
        {
            RetryFunc = retryFunc ?? throw new ArgumentNullException(nameof(retryFunc));
            Argument = argument;
        }

        /// <inheritdoc />
        public Func<TArgument, Func<TArgument, TResult>, TResult> RetryFunc { get; }

        /// <inheritdoc />
        public TArgument Argument { get; }
    }

    /// <inheritdoc />
    internal class FluentContext<TArgument> : IFluentContext<TArgument>
    {
        public FluentContext(Action<TArgument, Action<TArgument>> retryFunc, TArgument argument)
        {
            RetryFunc = retryFunc ?? throw new ArgumentNullException(nameof(retryFunc));
            Argument = argument;
        }

        /// <inheritdoc />
        public Action<TArgument, Action<TArgument>> RetryFunc { get; }

        /// <inheritdoc />
        public TArgument Argument { get; }
    }
}