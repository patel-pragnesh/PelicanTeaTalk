using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Services.Interfaces;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class StoreRewardsViewModel : BaseViewModel
    {
        private IAppService _appService;
        public StoreRewardsViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;
            RedemptionPin = "M16602403";
            AsyncRunner.Run(LoadVoucherItems());
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //if (parameters["RedemptionPin"] != null)
            //{
            //    RedemptionPin = (string)parameters["RedemptionPin"];
            //}
        }

        public ICommand RedemptionHistoryCommand => new Command(async () => await RedemptionHistoryCommandAsync());
        private async Task RedemptionHistoryCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(RedemptionHistoryPage));
        }

        public ICommand ViewVouchersCommand => new Command(async () => await ViewVouchersCommandAsync());
        private async Task ViewVouchersCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(VouchersPage));
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
    }
}
