using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Languages.Texts;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class BaseViewModel : BindableBase, INotifyPropertyChanged, INavigationAware, IDestructible
    {
        public System.DateTime? lastBackKeyDownTime;
        protected INavigationService NavigationService { get; private set; }
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            GoBackCommand = new DelegateCommand(async () =>
            {
                await Shell.Current.Navigation.PopAsync(animated: false);
            });

            BackCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }
        public virtual void Destroy()
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }
        public async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            if (loadingMessage == null)
            {
                loadingMessage = TextsTranslateManager.Translate("Loading");
            }
            
            UserDialogs.Instance.ShowLoading(loadingMessage, MaskType.None);
            await Task.Delay(200);
            IsBusy = true;

            try
            {
                await func();
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        public async Task SetRefreshingAsync(Func<Task> func)
        {
            Refreshing = true;
            try
            {
                await func();
            }
            finally
            {
                Refreshing = false;
            }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private bool _refreshing;
        public bool Refreshing
        {
            get => _refreshing;
            set => SetProperty(ref _refreshing, value);
        }

        bool isVisible = false;
        public bool IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref isVisible, value, "IsVisible"); }
        }
        string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetProperty(ref _pageTitle, value, "PageTitle"); }
        }
        public bool _connection;
        public bool Connection
        {
            get => _connection;
            set { SetProperty(ref _connection, value); }
        }
        public bool BackKeyPressed()
        {
            if (!lastBackKeyDownTime.HasValue || System.DateTime.Now - lastBackKeyDownTime.Value > new System.TimeSpan(0, 0, 2))
            {
                lastBackKeyDownTime = System.DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task UnloadAsync()
        {
            return Task.CompletedTask;
        }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
    }
}
