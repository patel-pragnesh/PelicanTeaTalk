using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class NotificationViewModel: BaseViewModel
    {
        private readonly IAppService _appService;
        public NotificationViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;
            ItemSelectedCommand = new Command(ItemSelected);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            AsyncRunner.Run(GetNotifications());
        }

        private async Task GetNotifications()
        {
            try
            {
                IsBusy = true;
                int totalItems = 0;
                var notificationResponse = await _appService.GetArchivedNotifications();
                if (notificationResponse != null && notificationResponse.HasData)
                {
                    Notifications = new ObservableCollection<NotificationItem>(notificationResponse.Notifications);
                    totalItems = Notifications.Count;
                }
                GlobalSettings.NotificationCounts = totalItems;
                //MessagingCenter.Send<string>(totalItems > 0 ? totalItems.ToString() : "", GlobalSettings.MessagingKey.NotificationCountKey);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private ObservableCollection<NotificationItem> _notifications;
        public ObservableCollection<NotificationItem> Notifications
        {
            get { return _notifications; }
            set { SetProperty(ref _notifications, value); }
        }

        public Command ItemSelectedCommand { get; set; }


        private void ItemSelected(object selectedItem)
        {
            // Do something
        }
    }
}
