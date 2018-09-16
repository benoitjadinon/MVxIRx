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
                .Debug(_ => _, _ => $"HomePage state : {_}")
                .Select(s => s.ButtonLabel)
                .Subscribe(s => DetailsButt.Text = s)
                .DisposeWhenDisappearing(ViewModel)
                ;

            // intents

            DetailsButt.Events().Clicked.Subscribe(_ => ViewModel.OpenDetails()).DisposeWhenDisappearing(ViewModel);
        }

        private void OnLogoutClick(object sender, EventArgs e) => ViewModel.LogOut();
    }
}
