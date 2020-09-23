using Acr.UserDialogs;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using WPSTORE.Animations;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _context => (LoginViewModel)BindingContext;
        protected INavigationService NavigationService { get; private set; }
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(async () =>
            {
                await ViewAnimations.FadeAnimY(Logo);
                await ViewAnimations.FadeAnimY(MainStack);
            });
        }
        private async void Login(object sender, EventArgs e)
        {
            if ((tbEmail_Login.Text ?? "") == "" || (tbPassword_Login.Text ?? "") == "")
            {
                await DisplayAlert("ERROR", "Please enter your email and password to continue.", "OK");
                return;
            }
            var dial = UserDialogs.Instance.Loading("Please wait...");
            dial.Show();
            //await Task.Delay(3000);
            await _context.LoginClicked();
            dial.Hide();
        }
    }
}