using System;
using System.Threading.Tasks;

namespace zFluent.Extensions
{
    /// <summary>
    /// Context of async retry fucntion
    /// </summary>
    /// <typeparam name="TArgument">Base class argument of extension</typeparam>
    /// <typeparam name="TResult">Result of the function</typeparam>
    public interface IFluentContextAsync<TArgument, TResult>
    {
        /// <summary>
        /// Retry function
        /// </summary>
        Func<TArgument, Func<TArgument, Task<TResult>>, Task<TResult>> RetryFunc { get; }

        /// <summary>
        /// Argument of extention
        /// </summary>
        TArgument Argument { get; }
    }

    /// <summary>
    /// Context of async retry action
    /// </summary>
    /// <typeparam name="TArgument">Base class argument of extension</typeparam>
    public interface IFluentContextAsync<TArgument>
    {
        /// <summary>
        /// Retry function
        /// </summary>
        Func<TArgument, Func<TArgument, Task>, Task> RetryFunc { get; }

        /// <summary>
        /// Argument of extention
        /// </summary>
        TArgument Argument { get; }
    }
}