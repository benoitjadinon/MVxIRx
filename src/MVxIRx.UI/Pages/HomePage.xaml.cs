using System;
using System.Diagnostics;
using System.Reactive.Linq;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MVxIRx.Core;
using MVxIRx.Core.ViewModels;
using MVxIRx.Core.ViewModels.Home;
using Xamarin.Forms;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // bindings

            var set = this.CreateBindingSet<HomePage, HomeViewModel>();
            set.Bind(this)
                .For(v => v.Title)
                .To(vm => vm.State.Title);
            set.Apply();

            // or rx updates

            ViewModel.WhenState
                .Debug(st => st/*, st => $"HomePage state : {st.PrettyPrint()}"*/)
                .Select(st => st.ButtonLabel)
                .Subscribe(t => DetailsButt.Text = t)
                .DisposeWhenDisappearing(ViewModel)
                ;

            // intents

            DetailsButt.Events().Clicked
                .Subscribe(_ => ViewModel.Intents.OpenUserDetails())
                .DisposeWhenDisappearing(ViewModel);
        }

        // from xaml
        private void OnLogoutClick(object sender, EventArgs e)
            => ViewModel.Intents.LogOut();
    }
}
