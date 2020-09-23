using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Services.Interfaces;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class RedemptionPinViewModel : BaseViewModel
    {
        private IAppService _appService;
        public RedemptionPinViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
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
