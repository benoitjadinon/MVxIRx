using System;
using System.Reactive.Linq;
using System.Threading;
using MVxIRx.Core.ViewModels.Login;

// ViewModel(ViewModelBloc(ViewModelState))
namespace MVxIRx.Core.ViewModels.Home
{
    // ViewModel : placeholder for the bloc and target for mvvmcross's viewmodel-based navigation
    public class HomeViewModel : BaseStatefulViewModel<IHomeViewModelBloc, HomeViewModelState> { }


    // ViewModel Bloc Interface : (not mandatory) contains the State output, and other intent methods
    public interface IHomeViewModelBloc : IStatefulBloc<HomeViewModelState> //, ILogin
    {
        void OpenUserDetails();
        void LogOut();
    }


    // ViewModel Bloc : used to combine blocs into one testable 'viewmodel' bloc,
    // so there is no logic in the viewmodel, can be reused as an upper level bloc somewhere else
    public class HomeViewModelBloc : BaseStatefulBloc<HomeViewModelState>, IHomeViewModelBloc
    {
        private readonly IAuthBloc _authBloc;
        //protected override HomeViewModelState CreateInitialState() => new HomeViewModelState();

        public HomeViewModelBloc(IAuthBloc authBloc)
        {
            _authBloc = authBloc;

            _authBloc.WhenState
                .WhereAs<IAuthState, LoggedInState>()
                .Select(s => s.Username)
                .Select(s => $"{s} detail page")
                .Subscribe(UpdateStateProperty(s => s.ButtonLabel));

            /*
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
                .Subscribe(UpdateStateProperty(s => s.Title))
                ;

            // other one-property syntax
            UpdateStateProperty(s => s.ButtonLabel, "Click Me !!!");

            // updates multiple properties at once but will only call setstate() 1 time
            UpdateStateProperties(state =>
            {
                state.Title = "a";
                state.ButtonLabel = "b";
                return state;
            });

            SetState(new HomeViewModelState());
            SetState(new HomeViewModelState(error:e));
            SetState(new HomeViewModelState(data:obj));

            // hardcoded values to test ui with states before having business logic
            SetState(new HomeViewModelState{
                Title = "A Title",
                ButtonLabel = "Click Me"
            });
            */
        }

        //TODO : Sinks/Intents : ReactiveCommand or ReplaySubject.OnNext or just a method ?
        public void OpenUserDetails() { /*TODO*/ }

        //protected override HomeViewModelBloc CreateBloc() => new HomeViewModelBloc(xxx);
        public void LogOut() => _authBloc.LogOut();
    }


    // The States POCOs
    // TODO consider using structs (immutable) ? (see immutable git branch)
    // TODO but they can only be created (not changed) from their constructors
    // TODO that would actually force grouping multiple smaller states
    // TODO and force using real states which may not be a bad thing
    public class HomeViewModelState
    {
        public string Title { get; set; } = "Home";
        public string ButtonLabel { get; set; } = "loading...";

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
    }
}
