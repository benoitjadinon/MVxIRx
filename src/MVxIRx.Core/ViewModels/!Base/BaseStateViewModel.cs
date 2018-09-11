using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;

namespace MVxIRx.Core.ViewModels
{
    public abstract class BaseStatefulViewModel<TBloc, TState> : BaseViewModel, IHasState<TState>
        where TState : class
        where TBloc : class, IStatefulBloc<TState>
    {
        private TBloc _bloc;
        protected TBloc Bloc => _bloc ?? (_bloc = CreateBloc());
        protected virtual TBloc CreateBloc() => (TBloc)Activator.CreateInstance(typeof(TBloc));

        public IObservable<TState> StateObservable { get; }

        private TState _state;
        public TState State
        {
            get => _state;
            protected set
            {
                _state = value;
                RaisePropertyChanged(() => State);
            }
        }

        public BaseStatefulViewModel()
        {
            StateObservable = Bloc.StateObservable;

            //TODO : only subscribe if state is bound ? possible ?
            StateObservable
                .Subscribe(state => State = state)
                ;
        }
    }
}
