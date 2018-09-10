using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVxIRx.Core.ViewModels.Home
{
    public class HomeViewModel : BaseStateViewModel<HomeViewModelState>
    {
        public override async Task Initialize()
        {
            // update whole state
            Observable.Return(new HomeViewModelState { Title = "B" })
                .Delay(TimeSpan.FromSeconds(3))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(SetState)
                ;

            // update state property
            Observable.Return("C")
                .Delay(TimeSpan.FromSeconds(5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(SetState(s => s.Title))
                ;

            //SetState(s => s.Title, "yo");

            //SetState(new HomeState());
        }
    }

    public class HomeViewModelState
    {
        public string Title { get; set; } = "A";

        public override string ToString() => Title;
    }
}
