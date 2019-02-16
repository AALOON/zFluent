using System;

namespace zFluent.Extensions
{
    /// <summary>
    /// Context of retry fucntion
    /// </summary>
    /// <typeparam name="TArgument">Base class argument of extension</typeparam>
    /// <typeparam name="TResult">Result of the function</typeparam>
    public interface IFluentContext<TArgument, TResult>
    {
        /// <summary>
        /// Retry function
        /// </summary>
        Func<TArgument, Func<TArgument, TResult>, TResult> RetryFunc { get; }

        /// <summary>
        /// Argument of extention
        /// </summary>
        TArgument Argument { get; }
    }

    /// <summary>
    /// Context of retry action
    /// </summary>
    /// <typeparam name="TArgument">Base class argument of extension</typeparam>
    public interface IFluentContext<TArgument>
    {
        /// <summary>
        /// Retry function
        /// </summary>
        Action<TArgument, Action<TArgument>> RetryFunc { get; }

        /// <summary>
        /// Argument of extention
        /// </summary>
        TArgument Argument { get; }
    }
}