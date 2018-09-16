using MvvmCross.Forms.Views;
using MVxIRx.Core.ViewModels.Login;
using Xamarin.Forms.Xaml;

namespace MVxIRx.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : MvxContentPage<LoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            loginButt.Clicked += (a,b) => ViewModel.LogIn(loginEditor.Text, passEditor.Text);

            /*if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
                navigationPage.BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            }*/

            /*
            // bindings

            var set = this.CreateBindingSet<LoginPage, LoginViewModel>();
            set.Bind(loginButt)
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
            */
        }
    }
}
