using Prism.Navigation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Models.WooCommerce.Requests;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class CouponsViewModel : BaseViewModel
    {
        private IAppService _appService;
        private readonly IWooCommerceService _wooCommerceService;

        public CouponsViewModel(INavigationService navigationService, IAppService appService, IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _appService = appService;

            _wooCommerceService = wooCommerceService;

            AsyncRunner.Run(InitDataAsync());
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        private async Task InitDataAsync()
        {
            await Task.Delay(200);

            PageIndex = 1;
            await SetBusyAsync(async () =>
            {
                await FetchCouponsAsync();
            });
        }

        private async Task FetchCouponsAsync()
        {
            var couponListRequest = new CouponListRequest();
            couponListRequest.Page = PageIndex;
            couponListRequest.PageSize = GlobalSettings.AppConst.PageSize;
            await WebRequestExecuter.Execute(async () => await _wooCommerceService.GetCoupons(couponListRequest), myCoupons =>
            {
                if (myCoupons != null && myCoupons.Any())
                {
                    MyCoupons = new ObservableCollection<CouponModel>(myCoupons);
                    HasMoreCoupons = myCoupons.Count >= couponListRequest.PageSize ? true : false;
                }
                else
                {
                    HasMoreCoupons = false;
                }
                return Task.CompletedTask;
            });
        }

        private async Task LoadMoreCoupons()
        {
            if (HasMoreCoupons)
            {
                await Task.Delay(200);

                PageIndex += 1;
                var couponListRequest = new CouponListRequest
                {
                    Page = PageIndex,
                    PageSize = GlobalSettings.AppConst.PageSize
                };

                await WebRequestExecuter.Execute(async () => await _wooCommerceService.GetCoupons(couponListRequest), myCoupons =>
                {
                    if (myCoupons != null && myCoupons.Any())
                    {
                        foreach (var coupon in myCoupons)
                            MyCoupons.Add(coupon);

                        HasMoreCoupons = myCoupons.Count >= couponListRequest.PageSize ? true : false;
                    }
                    else
                    {
                        PageIndex -= 1;
                        HasMoreCoupons = false;
                    }
                    return Task.CompletedTask;
                });
            }
        }

        public ICommand ListViewItemAppearingCommand => new Command<object>(async (data) =>
        {
            var itemEventArgs = data as ItemVisibilityEventArgs;
            if (itemEventArgs.ItemIndex + 1 == MyCoupons.Count)
            {
                if (HasMoreCoupons)
                    await SetBusyAsync(async () => await LoadMoreCoupons(), "Loading ...");
            }
        });

        public ICommand RefreshCouponsCommand => new Command(async () =>
        {
            await Task.Delay(200);

            PageIndex = 1;
            await SetRefreshingAsync(async () => await FetchCouponsAsync());
        });

        internal int PageIndex { get; set; }
        internal bool HasMoreCoupons { get; set; }

        private ObservableCollection<CouponModel> _myCoupons;
        public ObservableCollection<CouponModel> MyCoupons
        {
            get => _myCoupons;
            set => SetProperty(ref _myCoupons, value);
        }

        private string _redemptionPin;
        public string RedemptionPin
        {
            get { return _redemptionPin; }
            set { SetProperty(ref _redemptionPin, value, "RedemptionPin"); }
        }

        public ICommand CodeScanningCommand => new Command(async () => await CodeScanning());
        private async Task CodeScanning()
        {
            await NavigationService.NavigateAsync(nameof(CodeScanningPage));
        }
    }
}
