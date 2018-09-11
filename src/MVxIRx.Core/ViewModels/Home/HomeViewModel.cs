using System;
using System.Reactive.Linq;
using System.Threading;
using MVxIRx.Core.ViewModels;

// ViewModel(ViewModelBloc(ViewModelState))
namespace MVxIRx.Core.ViewModels
{
    // ViewModel : placeholder for the bloc and target for mvvmcross's viewmodel-based navigation
    public class HomeViewModel : BaseStatefulViewModel<HomeViewModelBloc, HomeViewModelState>, IHomeViewModelBloc
    {
        public void OpenDetails() => Bloc.OpenDetails();

        //protected override HomeViewModelBloc CreateBloc() => new HomeViewModelBloc(xxx);
    }

    // ViewModel Bloc Interface : (not mandatory) contains the State output, and other input methods
    public interface IHomeViewModelBloc : IStatefulBloc<HomeViewModelState>
    {
        void OpenDetails();
    }

    // ViewModel Bloc : use to combine blocs into one testable 'viewmodel' bloc,
    // so there is no logic in the viewmodel
    public class HomeViewModelBloc : StatefulBloc<HomeViewModelState>, IHomeViewModelBloc
    {
        //protected override HomeViewModelState CreateFirstState() => new HomeViewModelState();

        public HomeViewModelBloc()
        {
            // update the whole state
            Observable.Return(new HomeViewModelState("B", "Click"))
                .Delay(TimeSpan.FromSeconds(3))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(SetState)
                ;

            // update only one state property
            /*Observable.Return("C")
                .Delay(TimeSpan.FromSeconds(5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(SetStateProperty(s => s.Title))
                ;*/

            // other one-property syntax
            //SetStateProperty(s => s.ButtonLabel, "Click Me !!!");

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
    }

    // The States POCO
    // TODO consider using structs (immutable) ?
    // TODO but they can only be created (not changed) from their constructors
    // TODO that would actually force grouping multiple smaller states
    // TODO and force using real states which may not be a bad thing
    public struct HomeViewModelState
    {
        public readonly string Title, ButtonLabel;

        public HomeViewModelState(Exception error)
        {
            Title = $"Error {error?.Message ?? string.Empty}";
            ButtonLabel = "Retry";
        }

        public HomeViewModelState(string title, string butt)
        {
            Title = title;
            ButtonLabel = butt;
        }

        public override string ToString() => Title;
    }
}
