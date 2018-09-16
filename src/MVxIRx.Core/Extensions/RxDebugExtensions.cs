using System;
using System.Reactive;
using System.Reactive.Linq;

namespace MVxIRx.Core
{
    public static class RxDebugExtensions
    {
        // Allows for breakpoints to be set inline
        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> @this, Func<(T,string), (T,string)> a)
        {
            return @this
                .Do(_ => a((_,"before")))
                .DistinctUntilChanged()
                .Do(_ => a((_,"after")))
                ;
        }

        /// <summary>
        /// Allows for breakpoints to be set inline
        /// </summary>
        /// <example>.Debug(_ => _)</example>
        /// <param name="this"></param>
        /// <param name="func">fake func that gets and returns the value</param>
        /// <typeparam name="T">the source observable type</typeparam>
        /// <returns>the source observable</returns>
        public static IObservable<T> Debug<T>(this IObservable<T> @this, Func<T, T> func, Func<T, string> printer = null)
            => @this
                .Do(_ => System.Diagnostics.Debug.WriteLine(printer?.Invoke(_)))
                .Do(_ => func?.Invoke(_));
    }
}
