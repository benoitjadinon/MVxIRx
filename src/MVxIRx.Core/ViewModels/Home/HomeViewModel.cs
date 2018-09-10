using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MVxIRx.Core.ViewModels.Home
{
    public class HomeViewModel : BaseStateViewModel<HomeViewModelState>
    {
        public override async Task Initialize()
        {
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
                .Subscribe(SetStateProperty(s => s.Title))
                ;

            // other one-property syntax
            SetStateProperty(s => s.ButtonLabel, "Click Me !!!");

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
    }

    public class HomeViewModelState
    {
        public string Title { get; set; } = "...";
        public string ButtonLabel { get; set; } = "Click Me !";

        public HomeViewModelState() {}

        public HomeViewModelState(Exception error)
        {
            Title = "Error";
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
