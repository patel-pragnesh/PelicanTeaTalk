using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Models.Categories;
using WPSTORE.Services.Interfaces;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class RewardsViewModel : BaseViewModel
    {
        private IAppService _appService;
        public RewardsViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
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
            ListMyVouchers = await _appService.GetVoucherItems();
            IsBusy = false;
        }

        public async Task LoadExchangeVouchers()
        {
            IsBusy = true;
            ExchangeVoucherList = await _appService.GetExchangeVouchers();
            IsBusy = false;
        }

        public ICommand ViewAllRewardCommand => new Command(async () => await ViewAllRewardCommandAsync());
        private async Task ViewAllRewardCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(RewardExplorePage), null);
        }

        private IEnumerable<Voucher> _listVouchers;
        public IEnumerable<Voucher> ListMyVouchers
        {
            get => _listVouchers;
            set => SetProperty(ref _listVouchers, value);
        }
        private IEnumerable<ExchangeVoucher> _listExchangeVoucher;
        public IEnumerable<ExchangeVoucher> ExchangeVoucherList
        {
            get => _listExchangeVoucher;
            set => SetProperty(ref _listExchangeVoucher, value);
        }
    }
}
