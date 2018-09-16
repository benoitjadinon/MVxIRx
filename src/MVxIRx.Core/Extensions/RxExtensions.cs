using System;
using System.Reactive.Linq;

namespace MVxIRx.Core
{
    public static class RxExtensions
    {
        public static IObservable<T> WhereAs<TSource, T>(this IObservable<TSource> @this)
            where TSource : class
            where T : TSource
            => @this
                .Where(a => a != null && a is T)
                .Cast<T>();
    }
}
