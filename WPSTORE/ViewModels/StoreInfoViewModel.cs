using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace WPSTORE.ViewModels
{
    [QueryProperty("StoreParam", "storeParam")]
    public class StoreInfoViewModel : BaseViewModel
    {
        private IAppService _appService;
        public StoreInfoViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;

            AsyncRunner.Run(LoadVoucherItems());
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public async Task LoadVoucherItems()
        {
            IsBusy = true;
            await Task.Delay(300);
            ListVouchers = await _appService.GetVoucherItems();

            IsBusy = false;
        }
        private string _redemptionPin;
        public string RedemptionPin
        {
            get { return _redemptionPin; }
            set { SetProperty(ref _redemptionPin, value, "RedemptionPin"); }
        }

        private IEnumerable<Voucher> _listVouchers;
        public IEnumerable<Voucher> ListVouchers
        {
            get => _listVouchers;
            set => SetProperty(ref _listVouchers, value);
        }
        private StoreInfo _store;
        public StoreInfo Store
        {
            get { return _store; }
            set { SetProperty(ref _store, value); }
        }

        public string StoreParam
        {
            set
            {
                AsyncRunner.Run(Processing(value));
            }
        }

        private async Task Processing(string storeParam)
        {
            await Task.Delay(200);
            await SetBusyAsync(async() =>
            {
                Store = QueryStringHelpers.GetData<StoreInfo>(storeParam);
                if (Store != null)
                {
                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Label = Store.Store_Name,
                        Address = Store.Address,
                        Icon = BitmapDescriptorFactory.FromBundle(GlobalSettings.AppConst.StorePinImage),
                        Position = new Xamarin.Forms.GoogleMaps.Position(Store.Latitude.Value, Store.Longitude.Value)
                    };
                    MessagingCenter.Send(pin, MessagingCenterKeys.OnDrawMapsPinSelectedStore);
                }
            }, "Loading ...");
        }

        public async Task OpenGetDirections()
        {
            bool isGranted = await AppHelpers.RequestPermission(Permission.Location);
            MapPoint myPoint = null;
            if (isGranted)
            {
                var position = await AppHelpers.GetCurrentPosition();
                myPoint = new MapPoint { Latitude = (float)position.Latitude, Longitude = (float)position.Longitude };
            }

            Uri uri = null;
            if (Store.Latitude.HasValue && Store.Longitude.HasValue)
            {
                if (myPoint != null)
                    uri = new Uri($"https://maps.google.com?saddr={myPoint.Latitude},{myPoint.Longitude}&daddr={Store.Latitude.Value},{Store.Longitude.Value}");
                else
                    uri = new Uri($"https://maps.google.com?daddr={Store.Latitude.Value},{Store.Longitude.Value}");
            }
            else
                uri = new Uri("https://maps.google.com");

            if (await Launcher.CanOpenAsync(uri))
                await Launcher.OpenAsync(uri);
        }
    }
}
