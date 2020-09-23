using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.AppSettings;
using WPSTORE.Model;
using WPSTORE.Services.Interfaces;
using WPSTORE.Themes;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private bool isFirsLoaded = true;

        public SettingsViewModel(INavigationService navigationService ) : base(navigationService)
        {
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

        public ICommand ChooseAppThemeCommand => new Command(async () => await ChooseAppThemeAsync());
        private async Task ChooseAppThemeAsync()
        {
            await PopupNavigation.Instance.PushAsync(new PopupSelectThemePage());
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

        public string VersionNumber
        {
            get
            {
                return DependencyService.Get<IAppVersion>().GetVersionNumber();
            }
        }
    }
}
