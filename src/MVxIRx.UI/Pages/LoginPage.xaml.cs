using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MVxIRx.Core.ViewModels.Login;
using Xamarin.Forms.Xaml;

namespace MVxIRx.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = true)]
    public partial class LoginPage : MvxContentPage<LoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnLoginClick(object sender, EventArgs e)
            => ViewModel.LogIn(LoginEntry.Text, PassEntry.Text);
    }
}
