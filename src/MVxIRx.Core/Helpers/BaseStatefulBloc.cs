using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

namespace MVxIRx.Core
{
    public interface IHasState<TState>
        where TState : class
    {
        IObservable<TState> WhenState { get; }
    }

    public interface IStatefulBloc<TState> : IHasState<TState>
        where TState : class
    {
    }

    //public abstract class BaseStatefulBloc : BaseStatefulBloc<object>{}

    public abstract class BaseStatefulBloc<TState> : IStatefulBloc<TState>
        where TState : class
    {
        private readonly int _statesHistoryLength;
        private readonly Queue<TState> _statesHistory; //TODO: thread safe

        private readonly ReplaySubject<TState> _stateSubject;
        public IObservable<TState> WhenState { get; private set; }

        protected BaseStatefulBloc(TState initialState = null, int statesHistoryLength = 4)
        {
            _statesHistoryLength = statesHistoryLength;
            _statesHistory = new Queue<TState>(statesHistoryLength);

            _stateSubject = new ReplaySubject<TState>(1);
            WhenState = _stateSubject.AsObservable();

            _stateSubject.Subscribe(state =>
            {
                Debug.WriteLine($"State Added in {this.GetType().Name} : {state.GetType().Name} => {state.PrettyPrint(indented:false)}");
                if (_statesHistory.Count >= _statesHistoryLength)
                    _statesHistory.Dequeue();
                _statesHistory.Enqueue(state);
            });

            //TODO : what if there would be none set by default ? bindings would fail ?
            //SetState(CreateInitialState());

            if (initialState != null)
                SetState(initialState);
        }

        public void UpdateStateProperties(Func<TState, TState> stateAction)
            => SetState(stateAction(GetLastState().Copy()));

        public void UpdateStateProperty<TO>(Expression<Func<TState, TO>> stateProperty, TO @value)
            => UpdateStateProperty(stateProperty)(@value);

        public Action<TO> UpdateStateProperty<TO>(Expression<Func<TState, TO>> stateProperty)
        {
            return value =>
            {
                var memberExpression = (MemberExpression)stateProperty.Body;
                var property = (PropertyInfo)memberExpression.Member;

                TState newState = GetLastState().Copy();

                property.SetValue(newState, value, null);

                SetState(newState);
            };
        }

        public void SetState()
            => SetState<TState>();

        public void SetState<T>(params object[] @params)
            where T:TState
            => SetState((T)Activator.CreateInstance(typeof(T), @params));

        public void SetState(TState state)
            => _stateSubject.OnNext(state);

        public TState GetLastState()
            => _statesHistory.LastOrDefault() ?? CreateInitialState();

        protected virtual TState CreateInitialState()
        {
            SetState<TState>();
            return _statesHistory.LastOrDefault();
        }

        public IEnumerable<TState> GetLastStates(int lastStatesCount = 4)
            => _statesHistory.Skip(Math.Max(0, _statesHistory.Count - Math.Min(lastStatesCount, Math.Min(_statesHistoryLength, _statesHistory.Count))));
    }
}
