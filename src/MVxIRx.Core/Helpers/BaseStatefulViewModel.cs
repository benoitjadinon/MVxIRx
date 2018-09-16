using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Forms.Views;
using MvvmCross.IoC;

namespace MVxIRx.Core.ViewModels
{
    public interface IHasMVVMState<TState> : IHasState<TState>
        where TState : class
    {
        TState State { get; }
    }

    public abstract class BaseStatefulViewModel<TBloc, TState> : BaseViewModel, IHasMVVMState<TState>, IHasCompositeDisposable
        where TState : class
        where TBloc : class, IStatefulBloc<TState>
    {
        private readonly IMvxIoCProvider _ioCProvider;
        private TBloc _bloc;
        public TBloc Bloc => _bloc ?? (_bloc = CreateBloc());
        protected virtual TBloc CreateBloc()
        {
            if (_ioCProvider.CanResolve<TBloc>())
                // injection
                return _ioCProvider.Resolve<TBloc>();
            else
                // fallback instatiation
                // TODO : not awesome
                return (TBloc)Activator.CreateInstance(typeof(TBloc));
        }
        public TBloc Intents => Bloc;

        // for Rx Subscribes
        public IObservable<TState> WhenState { get; }

        // for MVVM Bindings
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


        public BaseStatefulViewModel(IMvxIoCProvider ioCProvider = null)
        {
            _ioCProvider = ioCProvider ?? Mvx.IoCProvider;
            WhenState = Bloc.WhenState;
        }


        public override void Prepare()
        {
            //FIXME : may be called too late ? (though inside the constructor was too early)
            WhenState
                .Subscribe(state => State = state)
                ;
        }

        public override async Task Initialize()
        {
        }

        public CompositeDisposable Disposables { get; private set; }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            Disposables = new CompositeDisposable();
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();
            Disposables?.Dispose();
        }
    }


    public interface IHasCompositeDisposable
    {
        CompositeDisposable Disposables { get; }
    }


    public static class StatefulViewModelExtensions
    {
        public static IDisposable DisposeWhenDisappearing(this IDisposable @this, IHasCompositeDisposable viewmodel)
        {
            viewmodel.Disposables.Add(@this);
            return @this;
        }
    }
}
