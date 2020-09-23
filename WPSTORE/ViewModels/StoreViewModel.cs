using Plugin.Messaging;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Services;
using WPSTORE.Services.Interfaces;
using WPSTORE.Views;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace WPSTORE.ViewModels
{
    public class StoreViewModel : BaseViewModel
    {
        private IAppService _appService;
        private readonly GoogleMapsApiService _googleMapsApiService;

        public StoreViewModel(INavigationService navigationService, IAppService appService, GoogleMapsApiService googleMapsApiService) : base(navigationService)
        {
            _appService = appService;
            _googleMapsApiService = googleMapsApiService;

            AsyncRunner.Run(Processing());
        }

        private async Task Processing()
        {
            await SetBusyAsync(LoadData, "Loading ...");
        }

        public async Task LoadData()
        {
            await Task.Delay(200);

            var stores = await _appService.GetStoreInfo();
            if (stores != null)
            {
                StoreList = new ObservableCollection<StoreInfo>(stores);

                foreach (var store in stores)
                {
                    if (store.Latitude.HasValue && store.Longitude.HasValue)
                    {
                        Pins.Add(new Pin()
                        {
                            Type = PinType.Place,
                            Label = store.Store_Name,
                            Address = store.Address,
                            Icon = BitmapDescriptorFactory.FromBundle(GlobalSettings.AppConst.StorePinImage),
                            Position = new Xamarin.Forms.GoogleMaps.Position(store.Latitude.Value, store.Longitude.Value)
                        });
                    }
                    MessagingCenter.Send(Pins, MessagingCenterKeys.OnDrawMapsPin);
                }
            }
        }
        internal List<Pin> Pins { get; set; } = new List<Pin>();

        public void Call(string mobileNumber)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(mobileNumber);
        }

        public ICommand StoreTappedCommand => new Command(async (item) =>
        {
            var selectedStore = item as StoreInfo;

            await NavigationHelpers.GotoPageAsync(nameof(StoreInfoPage),
                new Dictionary<string, object> { { "storeParam", QueryStringHelpers.GetQueryParam(selectedStore) } });
        });

        private ObservableCollection<StoreInfo> _storeList;
        public ObservableCollection<StoreInfo> StoreList
        {
            get { return _storeList; }
            set { SetProperty(ref _storeList, value); }
        }
    }
}
