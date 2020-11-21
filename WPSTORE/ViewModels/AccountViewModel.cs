using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.AppSettings;
using WPSTORE.Helpers;
using WPSTORE.Model;
using WPSTORE.Models;
using WPSTORE.Themes;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        private bool isFirsLoaded = true;

        public AccountViewModel(INavigationService navigationService ) : base(navigationService)
        {
            UserInfo = GlobalSettings.User;
            MemberRelated = !GlobalSettings.IsGuest;

            LanguageList = Settings.Languages;
            if (!string.IsNullOrEmpty(Settings.LanguageSelected))
            {
                LanguageSelected = LanguageList.FirstOrDefault(x => x.Code == Settings.LanguageSelected);
            }
            else
            {
                LanguageSelected = LanguageList.FirstOrDefault(x => x.Code == Settings.DefaultLanguage);
            }

            _isNightModeEnabled = Settings.IsNightModeEnabled;
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value, "UserName");
        }

        private UserModel _userInfo;
        public UserModel UserInfo
        {
            get { return _userInfo; }
            set { SetProperty(ref _userInfo, value); }
        }

        private bool _isNightModeEnabled;
        public bool IsNightModeEnabled
        {
            get => _isNightModeEnabled;
            set
            {
                SetProperty(ref _isNightModeEnabled, value);
                Settings.IsNightModeEnabled = value;
                SetTheme();
            }
        }

        void SetTheme()
        {
            if (IsNightModeEnabled)
            {
                Application.Current.Resources.Clear();
                Application.Current.Resources = new DarkTheme();
            }
            else
            {
                Application.Current.Resources.Clear();
                Application.Current.Resources = new LightTheme();
            }
        }

        private List<LanguageSelectItem> _languageList;
        public List<LanguageSelectItem> LanguageList
        {
            get => _languageList;
            set { SetProperty(ref _languageList, value); }
        }

        private LanguageSelectItem _languageSelected;
        public LanguageSelectItem LanguageSelected
        {
            get => _languageSelected;
            set
            {
                SetProperty(ref _languageSelected, value);
                Settings.LanguageSelected = value?.Code;
            }
        }

        private bool _memberRelated;
        public bool MemberRelated
        {
            get => _memberRelated;
            set { SetProperty(ref _memberRelated, value); }
        }

        public ICommand StoreRewardCommand => new Command(async () => await StoreRewardCommandAsync());
        private async Task StoreRewardCommandAsync()
        {
            if (GlobalSettings.IsGuest)
                await NavigationService.NavigateAsync(nameof(LoginPage));
            else
                await NavigationService.NavigateAsync(nameof(StoreRewardsPage));
        }
        public ICommand AccountInfoCommand => new Command(async () => await AccountInfoCommandAsync());
        private async Task AccountInfoCommandAsync()
        {
            if (GlobalSettings.IsGuest)
                await NavigationService.NavigateAsync(nameof(LoginPage));
            else
                await NavigationService.NavigateAsync(nameof(AccountInfoPage));
        }
        public ICommand LoginCommand => new Command(async () => await LoginCommandAsync());
        private async Task LoginCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(LoginPage));
        }
        public ICommand SettingsCommand => new Command(async () => await SettingsCommandAsync());
        private async Task SettingsCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(SettingsPage));
        }
        public ICommand HelpCommand => new Command(async () => await HelpCommandAsync());
        private async Task HelpCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(HelpPage));
        }
        public ICommand LogoutCommand => new Command(() =>
        {
            GlobalSettings.User = null;
            SecureStorageHelpers.RemoveKey(GlobalSettings.SecureKeys.Token);

            Application.Current.MainPage = new LoginPage();
        });

        public ICommand OrderHistoryCommand => new Command(async () =>
        {
            if (GlobalSettings.IsGuest)
                await NavigationService.NavigateAsync(nameof(LoginPage));
            else
                await NavigationHelpers.GotoPageAsync(nameof(OrderHistoryPage));
        });
    }
}
