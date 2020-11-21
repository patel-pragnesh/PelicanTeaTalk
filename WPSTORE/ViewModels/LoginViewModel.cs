using Prism.Commands;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Models.Authentication;
using WPSTORE.Models.SocialLogin;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using WPSTORE.Extensions;
using Xamarin.Forms;
using WPSTORE.Views;
using Plugin.FirebasePushNotification;

namespace WPSTORE.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userEmail;
        private string _password;
        private bool _isNavigatedFromInternal;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDialogService _dialogService;
        private readonly IAppService _appService;

        private DelegateCommand<SocialProvider?> socialSignInCommand;
        public LoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService,
            IDialogService dialogService, IAppService appService) : base(navigationService)
        {
            _appService = appService;
            _dialogService = dialogService;
            _authenticationService = authenticationService;

            //LoginCommand = new Command(LoginClicked);
            ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            SkipLoginCommand = new Command(SkipLoginClicked);
            RememberLogin = GlobalSettings.RememberMe;

            MessagingInit();

            //Task.Run(async () =>
            //{
            //    string password = await SecureStorageHelpers.GetValue(GlobalSettings.PasswordKey);
            //    if (!string.IsNullOrEmpty(password) && GlobalSettings.RememberMe)
            //    {
            //        UserName = GlobalSettings.UserName;
            //        Password = password;
            //    }
            //});
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters["NavigatedFromInternal"] != null)
            {
                _isNavigatedFromInternal = (bool)parameters["NavigatedFromInternal"];
            }
        }
        public string Password
        {
            get => _password;
            set { SetProperty(ref _password, value); }
        }
        public string UserName
        {
            get => _userEmail;
            set { SetProperty(ref _userEmail, value); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { SetProperty(ref _email, value, "Email"); }
        }

        private bool _rememberLogin;
        public bool RememberLogin
        {
            get { return _rememberLogin; }
            set { SetProperty(ref _rememberLogin, value); }
        }

        public Command LoginCommand { get; set; }
        public Command ForgotPasswordCommand { get; set; }
        public Command SkipLoginCommand { get; set; }

        public async Task LoginClicked()
        {
            IsBusy = true;
            await WebRequestExecuter.Execute(async () => await _authenticationService.LoginAsync(UserName, Password), async (result) =>
            {
                if (result != null && result.Token != null)
                {
                    var deviceToken = CrossFirebasePushNotification.Current.Token;
                    MessagingCenter.Send(deviceToken, MessagingCenterKeys.DeviceNotificationTokenChange);
                    await SecureStorageHelpers.SetValue(GlobalSettings.SecureKeys.Token, result.Token);

                    var myProfile = await _appService.GetMyInfo();
                    if (myProfile != null)
                    {
                        var user = new UserModel
                        {
                            Id = result.UserId,
                            DisplayName = result.DisplayName,
                            Email = result.UserEmail,
                            Name = myProfile.Name,
                            FirstName = myProfile.Name,
                            LastName = result.UserNiceName,
                            PictureUrl = myProfile.AvatarUrls?.ImageUrl96,
                            LoggedInWithSocialAccount = false,
                            Provider = SocialProvider.None
                        };
                        GlobalSettings.User = user;
                    }
                    
                    if (RememberLogin)
                    {
                        GlobalSettings.RememberMe = RememberLogin;
                        await SecureStorageHelpers.SetValue(GlobalSettings.PasswordKey, Password);
                    }

                    GlobalSettings.DeviceToken = deviceToken;
                    if (!string.IsNullOrEmpty(GlobalSettings.DeviceToken))
                    {
                        string deviceType = "android";

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            deviceType = "iosfcm";
                        }
                        await _appService.AddOrUpdateDeviceSubscription(GlobalSettings.DeviceToken, deviceType, $"{UserName}");
                    }

                    //Application.Current.MainPage = new AppShell();
                    GlobalSettings.IsGuest = false;
                    await GotoApp();

                }
                else
                {
                    //await _dialogService.ShowAlertAsync(result.Errors.FirstOrDefault()?.Description, "Info", "Ok");
                }
            });
            IsBusy = false;

        }

        private async void SkipLoginClicked(object obj)
        {
            GlobalSettings.AuthRequired = false;
            GlobalSettings.IsGuest = true;
            GlobalSettings.User = new UserModel
            {
                DisplayName = "Guest",
                PictureUrl = "https://icon-library.com/images/anonymous-icon/anonymous-icon-0.jpg"
            };
            await GotoApp();
        }

        private async Task GotoApp()
        {
            if (_isNavigatedFromInternal)
                await NavigationService.GoBackAsync();
            else
                Application.Current.MainPage = new AppShell();
        }
        private async void ForgotPasswordClicked(object obj)
        {
            var label = obj as Label;
            label.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            label.BackgroundColor = Color.Transparent;
        }

        public DelegateCommand<SocialProvider?> SocialSignInCommand =>
                                    socialSignInCommand ?? (socialSignInCommand =
                                                         new DelegateCommand<SocialProvider?>
                                                             (
                                                                async (SocialProvider? socialProvider) =>
                                                                {
                                                                    IsBusy = true;
                                                                    try
                                                                    {
                                                                        //var provider = snsProvider ?? SNSProvider.None;
                                                                        if (socialProvider.HasValue)
                                                                        {
                                                                            await _authenticationService.LoginWithSocialAsync(socialProvider.Value);
                                                                        }
                                                                    }
                                                                    catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
                                                                    {
                                                                        await _dialogService.ShowAlertAsync("Network Error.", "Error", "Ok");
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Debug.WriteLine($"Error in: {ex}");
                                                                    }
                                                                    finally
                                                                    {
                                                                        IsBusy = false;
                                                                    }
                                                                }
                                                            )
                                                        );
        void MessagingInit()
        {
            MessagingCenter.Instance.SubscribeSafe<SocialUser, bool>(this, MessagingCenterKeys.AuthenticationRequested, async (socialUser, isLogged) =>
            {
                if (isLogged)
                {
                    WpLoginResponseModel loginResult = null;

                    switch (socialUser.Provider)
                    {
                        case SocialProvider.Google:
                            await SetBusyAsync(async () => await WebRequestExecuter.Execute(async () => await _authenticationService.GoogleLogin(new WPGoogleLoginRequest
                            {
                                AccessToken = socialUser.Token
                            }), async result =>
                            {
                                loginResult = result;
                            }));
                            break;
                        case SocialProvider.Facebook:
                            await SetBusyAsync(async () => await WebRequestExecuter.Execute(async () => await _authenticationService.FacebookLogin(new WPFacebookLoginRequest
                            {
                                AccessToken = socialUser.Token
                            }), async result =>
                            {
                                loginResult = result;
                            }));
                            break;
                        default:
                            break;
                    }

                    if (loginResult != null && loginResult.Success)
                    {
                        GlobalSettings.SetUserInfo(socialUser);
                        var user = GlobalSettings.User;

                        user.Id = loginResult.UserId;
                        user.Email = loginResult.UserEmail;
                        user.UserName = loginResult.UserName;
                        user.DisplayName = loginResult.DisplayName;
                        user.PictureUrl = loginResult.Avatar;
                        GlobalSettings.User = user;

                        await SecureStorageHelpers.SetValue(GlobalSettings.SecureKeys.Token, loginResult.Token);

                        Application.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        //TODO: Show error while logging...
                        await _dialogService.ShowAlertAsync("Error while ...", "Info", "OK");
                    }
                }
            });
        }

        public ICommand SmsLoginCommand => new Command(async () =>
        {

        });

        public ICommand SignUpCommand => new Command(async () =>
        {
            try
            {
                //await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
                var nav = Application.Current.MainPage.Navigation;
                await nav.PushModalAsync(new RegisterPage());
            }
            catch(Exception ex)
            {

            }
            
        });
    }
}
