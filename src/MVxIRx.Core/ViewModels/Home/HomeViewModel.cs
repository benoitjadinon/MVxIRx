using System;
using System.Reactive.Linq;
using System.Threading;
using MVxIRx.Core.ViewModels.Login;

// ViewModel(ViewModelBloc(ViewModelState))
namespace MVxIRx.Core.ViewModels.Home
{
    // ViewModel : placeholder for the bloc and target for mvvmcross's viewmodel-based navigation
    public class HomeViewModel : BaseStatefulViewModel<IHomeViewModelBloc, HomeViewModelState>, IHomeViewModelBloc
    {
        //TODO : Sinks : ReactiveCommand or ReplaySubject or just a method ?
        public void OpenDetails() => Bloc.OpenDetails();

        //protected override HomeViewModelBloc CreateBloc() => new HomeViewModelBloc(xxx);
        public void LogOut() => Bloc.LogOut();
    }

    // ViewModel Bloc Interface : (not mandatory) contains the State output, and other input methods
    public interface IHomeViewModelBloc : IStatefulBloc<HomeViewModelState> //, ILogin
    {
        void OpenDetails();
        void LogOut();
    }

    // ViewModel Bloc : used to combine blocs into one testable 'viewmodel' bloc,
    // so there is no logic in the viewmodel, can be reused as an upper level bloc somewhere else
    public class HomeViewModelBloc : BaseStatefulBloc<HomeViewModelState>, IHomeViewModelBloc
    {
        private readonly ILoginBloc _loginBloc;
        //protected override HomeViewModelState CreateInitialState() => new HomeViewModelState();

        public HomeViewModelBloc(ILoginBloc loginBloc)
        {
            _loginBloc = loginBloc;

            // update the whole state
            Observable.Return(new HomeViewModelState { Title = "B" })
                .Delay(TimeSpan.FromSeconds(3))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(SetState)
                ;

            // update only one state property
            Observable.Return("C")
                .Delay(TimeSpan.FromSeconds(5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(UpdateState(s => s.Title))
                ;

            // other one-property syntax
            UpdateState(s => s.ButtonLabel, "Click Me !!!");

            // updates multiple properties at once but will only call setstate() 1 time
            /*
            SetStateProperties(state =>
            {
                state.Title = "a";
                state.ButtonLabel = "b";
                return state;
            });
            */

            //SetState(new HomeViewModelState());
            //SetState(new HomeViewModelState(error:e));
            //SetState(new HomeViewModelState(data:obj));

            /*
            // hardcoded values to test ui with states before having business logic
            SetState(new HomeViewModelState{
                Title = "A Title",
                ButtonLabel = "Click Me"
            });
            */
        }

        // send to a stateful bloc that may be listened by a global mvvmcross navigator
        public void OpenDetails() => throw new NotImplementedException();

        public void LogOut() => _loginBloc.LogOut();
    }

    // The States POCO
    // TODO consider using structs (immutable) ?
    // TODO but they can only be created (not changed) from their constructors
    // TODO that would actually force grouping multiple smaller states
    // TODO and force using real states which may not be a bad thing
    public class HomeViewModelState
    {
        public string Title { get; set; } = "...";
        public string ButtonLabel { get; set; } = "-";

        public HomeViewModelState() {}

        public HomeViewModelState(Exception error)
        {
            Title = $"Error {error?.Message ?? string.Empty}";
            ButtonLabel = "Retry";
        }

        public HomeViewModelState(object data)
        {
            Title = "OK";
            ButtonLabel = "Next";
        }

        public override string ToString() => Title;
    }
}
