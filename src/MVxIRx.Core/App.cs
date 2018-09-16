using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MVxIRx.Core.ViewModels.Home;
using MVxIRx.Core.ViewModels.Login;
using MVxIRx.Core.ViewModels.Splash;

namespace MVxIRx.Core
{
    public class App : MvxApplication//<TParameter> // for protocols
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Bloc")
                .AsInterfaces()
                //.AsTypesAndInterfaces() //TODO, see @MvxTypeExtensions
                .RegisterAsLazySingleton();

            RegisterCustomAppStart<AppNavigator>();
            //RegisterAppStart<HomeViewModel>();
        }
    }


    public class AppNavigator : MvxAppStart
    {
        private readonly IAppBloc _appBloc;

        public AppNavigator(IMvxApplication application, IMvxNavigationService navigationService, IAppBloc appBloc)
            : base(application, navigationService)
        {
            _appBloc = appBloc;

            _appBloc.StateObs
                .Select(state => state is AppHomeState
                    ? typeof(HomeViewModel)
                    : typeof(LoginViewModel)
                )
                .Subscribe(type => NavigationService.Navigate(type))
                //.SubscribeWithExceptionCatching(type => NavigationService.Navigate(type),
                //    new ExceptionCatcher(err => Debug.WriteLine(err), continueOnError:true))
                ;
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            //FIXME : ugly hack because the async code below is too slow for ios that promptly needs a view to display
            await NavigationService.Navigate<SplashViewModel>();

            /*
            IAppState state = null;
            try
            {
                var tcs = new TaskCompletionSource<IAppState>();
                Task.Run(async () => tcs.SetResult(await _appBloc.StateObs.FirstAsync()));
                state = tcs.Task.Result;

                // opening the right screen
                if (state is AppHomeState)
                    NavigationService.Navigate<HomeViewModel>().GetAwaiter().GetResult();
                else
                    NavigationService.Navigate<LoginViewModel>().GetAwaiter().GetResult();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to State {0}", state.GetType().FullName);
            }
            */
        }
    }


    public interface IAppBloc : IStatefulBloc<IAppState>{}

    public class AppBloc : BaseStatefulBloc<IAppState>, IAppBloc
    {
        public AppBloc(ILoginBloc loginBloc)
        {
            loginBloc.StateObs
                .Select(@is => (@is is LoggedOutState)
                    ? (IAppState)new AppLogInState()
                    : (IAppState)new AppHomeState()
                )
                //.DistinctUntilChanged(a => a)
                .Subscribe(SetState);
        }
    }

    public interface IAppState {}
    public class AppLogInState : IAppState {}
    public class AppHomeState : IAppState {}
}
