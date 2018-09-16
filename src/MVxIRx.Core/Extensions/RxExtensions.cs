using System;
using System.Reactive;
using System.Reactive.Linq;

namespace MVxIRx.Core
{
    public static class RxExtensions
    {
        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> @this, Func<(T,string), (T,string)> a)
        {
            return @this
                .Do(_ => a((_,"before")))
                .DistinctUntilChanged()
                .Do(_ => a((_,"after")))
                ;
        }
    }
}
