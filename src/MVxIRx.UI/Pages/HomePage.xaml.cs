using System;
using System.Diagnostics;
using System.Reactive.Linq;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MVxIRx.Core.ViewModels.Home;
using Xamarin.Forms.Xaml;

namespace MVxIRx.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = true)]
    public partial class HomePage : MvxContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            /*if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
                navigationPage.BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            }*/

            // bindings

            var set = this.CreateBindingSet<HomePage, HomeViewModel>();
            set.Bind(OpenDetailsButt)
                .For(v => v.Text)
                .To(vm => vm.State.ButtonLabel);
            set.Apply();

            // or rx updates

            ViewModel.StateObs
                .Do(state => Debug.WriteLine(state))
                .Subscribe(state =>
                {
                    Title = state.Title;
                    //butt.Text = state.ButtonLabel;
                });

            //

            OpenDetailsButt.Clicked += (a,b) => ViewModel.OpenDetails();
            LogOutButt.Clicked += (a,b) => ViewModel.LogOut();
        }
    }
}
