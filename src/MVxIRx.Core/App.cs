using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MVxIRx.Core.ViewModels;

namespace MVxIRx.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }
    }
}
