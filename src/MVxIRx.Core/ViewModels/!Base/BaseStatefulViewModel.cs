using System;
using System.Threading.Tasks;
using MvvmCross;

namespace MVxIRx.Core.ViewModels
{
    public abstract class BaseStatefulViewModel<TBloc, TState> : BaseViewModel, IHasState<TState>
        where TState : class
        where TBloc : class, IStatefulBloc<TState>
    {
        private TBloc _bloc;
        protected TBloc Bloc => _bloc ?? (_bloc = CreateBloc());
        protected virtual TBloc CreateBloc()
        {
            if (Mvx.IoCProvider.CanResolve<TBloc>())
                return Mvx.IoCProvider.Resolve<TBloc>();
            else
                return (TBloc)Activator.CreateInstance(typeof(TBloc));
        }

        public IObservable<TState> StateObs { get; }

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
            StateObs = Bloc.StateObs;
        }

        public override void Prepare()
        {
            //FIXME : this is too late, but inside the constructor was too early
            StateObs
                .Subscribe(state => State = state)
                ;
        }

        public override async Task Initialize()
        {
        }
    }
}
