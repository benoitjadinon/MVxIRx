using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Presenters.Attributes;
using MVxIRx.Core;
using MVxIRx.Core.ViewModels.Home;
using MVxIRx.Core.ViewModels.Splash;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MVxIRx.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //[MvxModalPresentation(NoHistory = true)]
    public partial class SplashPage : MvxContentPage<SplashViewModel>
    {
        public SplashPage()
        {
            InitializeComponent();
        }
    }
}

