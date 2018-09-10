using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;

namespace MVxIRx.Core.ViewModels
{
    public class BaseStateViewModel<TState> : BaseViewModel
        where TState : class
    {
        readonly int _statesHistoryLength;
        private readonly Queue<TState> _statesHistory; //TODO: thread safe
        public readonly IObservable<TState> StateObservable;

        public TState State
        {
            get
            {
                if (_statesHistory.Count == 0)
                {
                    var state = (TState)Activator.CreateInstance(typeof(TState));
                    _statesHistory.Enqueue(state);
                    return state;
                }
                return _statesHistory.Last();
            }
            private set
            {
                if (_statesHistory.Count >= _statesHistoryLength)
                    _statesHistory.Dequeue();
                _statesHistory.Enqueue(value);
                RaisePropertyChanged(() => State);
            }
        }

        protected BaseStateViewModel(int statesHistoryLength = 4)
        {
            _statesHistoryLength = statesHistoryLength;
            _statesHistory = new Queue<TState>(statesHistoryLength);

            StateObservable = Observable.Defer(() => Observable.Return(State))
                .Merge(Observable
                    .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        h => this.PropertyChanged += h,
                        h => this.PropertyChanged -= h)
                    .Where(args => args.EventArgs.PropertyName == nameof(State))
                    .Select(_ => State)
                );
        }


        public void SetState(TState state)
            => State = state;

        public void SetState<TO>(Expression<Func<TState, TO>> stateProperty, TO @value)
            => SetState(stateProperty)(@value);

        public Action<TO> SetState<TO>(Expression<Func<TState, TO>> stateProperty)
        {
            return @value =>
            {
                var memberExpression = (MemberExpression)stateProperty.Body;
                var property = (PropertyInfo)memberExpression.Member;

                TState newState = State.Copy();

                property.SetValue(newState, @value, null);

                State = newState;
            };
        }

        public IEnumerable<TState> GetLastStates(int lastStatesCount = 4)
            => _statesHistory.Skip(Math.Max(0, _statesHistory.Count - Math.Min(lastStatesCount, Math.Min(_statesHistoryLength, _statesHistory.Count))));
    }
}
