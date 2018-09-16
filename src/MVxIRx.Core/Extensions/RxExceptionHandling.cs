//https://gist.github.com/drstevens/1409829
/* This is how I am globally handling fatal exceptions thrown from Rx subscribers.
 * It is what I came up with in response to my stackoverflow question here
 * http://stackoverflow.com/questions/7210051/catching-exceptions-which-may-be-thrown-from-a-subscription-onnext-action
 * This is far from ideal. From what I understand, exception handling has been improved greately in Rx for .NET 4.5
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MVxIRx.Core
{
    /// <summary>
    /// I toyed with making this take a type param TException
    /// but decided against it because there are many places this is used.
    /// Each would have to duplicate this type parameter of the instance.
    /// I instead used Exception and require that this be used for fatal events only.
    /// Non fatal events must be caught in the action parameter
    /// </summary>
    public interface IExceptionCatcher
    {
        Action<T> Catch<T>(Action<T> action);

        void HandleException(Exception e);
    }

    /// <summary>
    /// This provides a way for exceptions to be handled from asynchronously executed code
    /// </summary>
    public class ExceptionCatcher : IExceptionCatcher, IDisposable
    {
        private readonly object _lockObject = new object();
        private readonly ManualResetEvent _resetEvent;
        private AggregateException _exceptions;
        private readonly Task _task;
        private volatile bool _isExceptionCaught;
        private readonly bool _continueOnError;

        public ExceptionCatcher(Action<AggregateException> errorAction, bool continueOnError)
        {
            _continueOnError = continueOnError;
            _resetEvent = new ManualResetEvent(false);
            _task = Task.Factory.StartNew(() =>
            {
              try
              {
                  _resetEvent.WaitOne();
                  AggregateException exceptions;
                  lock (_lockObject)
                  {
                      exceptions = _exceptions;
                  }
                  if (exceptions != null)
                  {
                      errorAction(exceptions);
                  }
              }
              catch (Exception ex)
              {
                  Debug.WriteLine("Error In Exception Action:" + ex.Message, ex);
                  throw;
              }
              finally
              {
                  Debug.WriteLine("Exiting...");
              }
            });
        }

        public Action<T> Catch<T>(Action<T> action)
        {
            return arg =>
                       {
                           try
                           {
                               if (!IsExecutionPrevented())
                                   action(arg);
                           }
                           catch (Exception e)
                           {
                               HandleException(e);
                           }
                       };
        }

        public void HandleException(Exception e)
        {
            _isExceptionCaught = true;
            lock (_lockObject)
            {
                var exceptions = new List<Exception> {e};
                if (_exceptions != null)
                {
                    exceptions.AddRange(_exceptions.InnerExceptions);
                }
                _exceptions = new AggregateException(String.Format("Exceptions handled by {0}", GetType().FullName),
                                                     exceptions);
                _resetEvent.Set(); //Signal task to wake up
            }
        }

        public void Dispose()
        {
            try
            {
                //wake up error handling task in case it hasn't been yet
                _resetEvent.Set();
                _task.Wait(TimeSpan.FromSeconds(10));
            }
            catch (Exception e)
            {
                //eat exception in Dispose
                Debug.WriteLine("Exception handling task completed exceptionally: " + e.Message, e);
            }
        }

        private bool IsExecutionPrevented()
        {
            return _isExceptionCaught && !_continueOnError;
        }
    }

    public static class Observables
    {
        /// <summary>
        /// Delegate to <see cref="System.ObservableExtensions.Subscribe{TSource}"/>
        /// but catch any exceptions resulting from calls to <paramref name="onNext"/>.
        /// Handle these exceptions using <paramref name="onError"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="onNext"></param>
        /// <param name="onError"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable SubscribeWithExceptionCatching<TSource>(this IObservable<TSource> source,
                                                                          Action<TSource> onNext,
                                                                          Action<Exception> onError,
                                                                          Action onCompleted)
        {
            return source.Subscribe(item =>
                                        {
                                            try
                                            {
                                                onNext(item);
                                            }
                                            catch (Exception e)
                                            {
                                                onError(e);
                                            }
                                        }, onError, onCompleted);
        }

        /// <summary>
        /// Delegate to <see cref="System.ObservableExtensions.Subscribe{TSource}"/>
        /// but catch any exceptions resulting from calls to <paramref name="onNext"/>.
        /// Handle these exceptions using <paramref name="exceptionCatcher"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="onNext"></param>
        /// <param name="exceptionCatcher">This is used to catch the exceptions</param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable SubscribeWithExceptionCatching<TSource>(this IObservable<TSource> source,
                                                                          Action<TSource> onNext,
                                                                          IExceptionCatcher exceptionCatcher,
                                                                          Action onCompleted)
        {
            return source.Subscribe(exceptionCatcher.Catch(onNext), exceptionCatcher.HandleException, onCompleted);
        }

        /// <summary>
        /// Delegate to <see cref="System.ObservableExtensions.Subscribe{TSource}"/>
        /// but catch any exceptions resulting from calls to <paramref name="onNext"/>.
        /// Handle these exceptions using <paramref name="exceptionCatcher"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="onNext"></param>
        /// <param name="exceptionCatcher">This is used to catch the exceptions</param>
        /// <returns></returns>
        public static IDisposable SubscribeWithExceptionCatching<TSource>(this IObservable<TSource> source,
                                                                          Action<TSource> onNext,
                                                                          IExceptionCatcher exceptionCatcher)
        {
            return source.Subscribe(exceptionCatcher.Catch(onNext), exceptionCatcher.HandleException);
        }
    }
}
