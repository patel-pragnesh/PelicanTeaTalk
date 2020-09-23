using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Models;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IAppService _appService;
        private readonly IDialogService _dialogService;
        private const int MinimumPasswordLength = 6;

        public RegisterViewModel(INavigationService navigationService, IAppService appService, IDialogService dialogService) : base(navigationService)
        {
            _appService = appService;
            _dialogService = dialogService;

            string requiredErrMessage = "{0} is required";
            UserNameErrMessage = string.Format(requiredErrMessage, "Username");
            EmailErrMessage = string.Format(requiredErrMessage, "Email");
            PasswordErrMessage = string.Format(requiredErrMessage, "Password");
        }

        public ICommand RegisterCommand => new Command(async() =>
        {
            if (!Validate())
                return;

            await SetBusyAsync(async () => await WebRequestExecuter.Execute(async() => await _appService.RegisterAccount(Account), async result =>
            {
                if(result != null && string.IsNullOrEmpty(result.Message))
                {
                    await GoBack();
                }
                else
                {
                    await _dialogService.ShowAlertAsync(result?.Message, "Info", "OK");
                }
            }));
        });

        public ICommand CancelCommand => new Command(async () =>
        {
            await GoBack();
        });

        private async Task GoBack()
        {
            var nav = Application.Current.MainPage.Navigation;
            await Task.Delay(200);
            await nav.PopModalAsync(true);
        }

        private bool Validate()
        {
            UserNameHasError = string.IsNullOrEmpty(Account.UserName);
            EmailHasError = string.IsNullOrEmpty(Account.Email);
            PasswordHasError = string.IsNullOrEmpty(Account.Password);

            if (!string.IsNullOrEmpty(Account.Email))
            {
                var isEmailValid = GlobalSettings.EmailRegex.IsMatch(Account.Email);
                if (!isEmailValid)
                {
                    EmailHasError = !isEmailValid;
                    EmailErrMessage = "Malformed email address";
                }
            }
            if (!string.IsNullOrEmpty(Account.Password))
            {
                var isPasswordValid = Account.Password.Length >= MinimumPasswordLength;
                if (!isPasswordValid)
                {
                    PasswordHasError = !isPasswordValid;
                    PasswordErrMessage = $"Password must be at least {MinimumPasswordLength} characters in length";
                }
            }

            if (UserNameHasError || EmailHasError || PasswordHasError)
                return false;

            return true;
        }

        private bool _userNameHasError;
        public bool UserNameHasError
        {
            get { return _userNameHasError; }
            set { SetProperty(ref _userNameHasError, value); }
        }

        private string _userNameErrMessage;
        public string UserNameErrMessage
        {
            get { return _userNameErrMessage; }
            set { SetProperty(ref _userNameErrMessage, value); }
        }

        private bool _emailHasError;
        public bool EmailHasError
        {
            get { return _emailHasError; }
            set { SetProperty(ref _emailHasError, value); }
        }

        private string _emailErrMessage;
        public string EmailErrMessage
        {
            get { return _emailErrMessage; }
            set { SetProperty(ref _emailErrMessage, value); }
        }

        private bool _passwordHasError;
        public bool PasswordHasError
        {
            get { return _passwordHasError; }
            set { SetProperty(ref _passwordHasError, value); }
        }

        private string _passwordErrMessage;
        public string PasswordErrMessage
        {
            get { return _passwordErrMessage; }
            set { SetProperty(ref _passwordErrMessage, value); }
        }

        private RegisterAccountRequestModel _account = new RegisterAccountRequestModel();
        public RegisterAccountRequestModel Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }
    }
}
