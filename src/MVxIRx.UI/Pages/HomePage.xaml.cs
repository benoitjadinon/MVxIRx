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
using MVxIRx.Core;
using MVxIRx.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MVxIRx.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : MvxContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            /*if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
                navigationPage.BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            }*/

            // MVVM bindings

            var set = this.CreateBindingSet<HomePage, HomeViewModel>();
            set.Bind(butt)
                .For(v => v.Text)
                .To(vm => vm.State.ButtonLabel); // bindings don't work with fields in states, only with public properties
            set.Apply();

            // or rx updates
            /*
            ViewModel.StateObservable
                .Do(state => Debug.WriteLine(state))
                .Subscribe(state =>
                {
                    Title = state.Title;
                    butt.Text = state.ButtonLabel;
                });
            */
        }
    }
}
