using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

namespace MVxIRx.Core
{
    public interface IHasState<TState>
        where TState : struct
    {
        //TState State { get; }
        IObservable<TState> StateObservable { get; }
    }

    public interface IStatefulBloc<TState> : IHasState<TState>
        where TState : struct
    {
    }

    public abstract class StatefulBloc<TState> : IStatefulBloc<TState>
        where TState : struct
    {
        private readonly int _statesHistoryLength;
        private readonly Queue<TState> _statesHistory; //TODO: thread safe

        private readonly ReplaySubject<TState> _stateSubject;
        public IObservable<TState> StateObservable { get; private set; }

        protected StatefulBloc(int statesHistoryLength = 4)
        {
            _statesHistoryLength = statesHistoryLength;
            _statesHistory = new Queue<TState>(statesHistoryLength);

            _stateSubject = new ReplaySubject<TState>(1);
            StateObservable = _stateSubject.AsObservable();

            _stateSubject.Subscribe(state =>
            {
                if (_statesHistory.Count >= _statesHistoryLength)
                    _statesHistory.Dequeue();
                _statesHistory.Enqueue(state);
            });

            //TODO : what if there would be none set by default ?
            SetState(CreateFirstState());
        }

        protected virtual TState CreateFirstState()
            => (TState)Activator.CreateInstance(typeof(TState));

        /*
        public void SetStateProperties(Func<TState, TState> stateAction)
            => SetState(stateAction(GetLastState().Copy()));

        public void SetStateProperty<TO>(Expression<Func<TState, TO>> stateProperty, TO @value)
            => SetStateProperty(stateProperty)(@value);

        public Action<TO> SetStateProperty<TO>(Expression<Func<TState, TO>> stateProperty)
        {
            return @value =>
            {
                var memberExpression = (MemberExpression)stateProperty.Body;
                var property = (PropertyInfo)memberExpression.Member;

                TState newState = GetLastState().Copy();

                property.SetValue(newState, @value, null);

                SetState(newState);
            };
        }
        */

        public void SetState(TState state)
            => _stateSubject.OnNext(state);

        public TState GetLastState()
            => _statesHistory.Last();// ?? CreateFirstState();

        public IEnumerable<TState> GetLastStates(int lastStatesCount = 4)
            => _statesHistory.Skip(Math.Max(0, _statesHistory.Count - Math.Min(lastStatesCount, Math.Min(_statesHistoryLength, _statesHistory.Count))));
    }
}
