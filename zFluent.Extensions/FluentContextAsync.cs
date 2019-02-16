using System;
using System.Threading.Tasks;

namespace zFluent.Extensions
{
    /// <inheritdoc />
    internal class FluentContextAsync<TArgument, TResult> : IFluentContextAsync<TArgument, TResult>
    {
        public FluentContextAsync(Func<TArgument, Func<TArgument, Task<TResult>>, Task<TResult>> retryFunc,
            TArgument argument)
        {
            RetryFunc = retryFunc ?? throw new ArgumentNullException(nameof(retryFunc));
            Argument = argument;
        }

        /// <inheritdoc />
        public Func<TArgument, Func<TArgument, Task<TResult>>, Task<TResult>> RetryFunc { get; }

        /// <inheritdoc />
        public TArgument Argument { get; }
    }
    
    /// <inheritdoc />
    internal class FluentContextAsync<TArgument> : IFluentContextAsync<TArgument>
    {
        public FluentContextAsync(Func<TArgument, Func<TArgument, Task>, Task> retryFunc, TArgument argument)
        {
            RetryFunc = retryFunc ?? throw new ArgumentNullException(nameof(retryFunc));
            Argument = argument;
        }

        /// <inheritdoc />
        public Func<TArgument, Func<TArgument, Task>, Task> RetryFunc { get; }

        /// <inheritdoc />
        public TArgument Argument { get; }
    }
}