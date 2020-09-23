using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.AppSettings;
using WPSTORE.Model;
using WPSTORE.Models.WooCommerce;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class PopupSelectThemeViewModel : BaseViewModel
    {
        public PopupSelectThemeViewModel(INavigationService navigationService) : base(navigationService)
        {
            ThemeList = Settings.Themes;
        }
        
        public ICommand CancelCommand => new Command(async () =>
        {
            await PopupNavigation.Instance.PopAsync(false);
        });

        private List<ThemeSelectItem> _themeList;
        public List<ThemeSelectItem> ThemeList
        {
            get => _themeList;
            set { SetProperty(ref _themeList, value); }
        }

        private ThemeSelectItem _themeSelected;
        public ThemeSelectItem ThemeSelected
        {
            get => _themeSelected;
            set
            {
                SetProperty(ref _themeSelected, value);
                Settings.ThemeSelected = value?.Name;
            }
        }
    }
}
