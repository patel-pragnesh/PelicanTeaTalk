using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Models.Categories;
using WPSTORE.Services.Interfaces;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class RedemptionHistoryViewModel : BaseViewModel
    {
        private IAppService _appService;
        public RedemptionHistoryViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;

            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public async Task LoadTransactions()
        {
            IsBusy = true;
            ListTransactions = await _appService.GetTransactions();
            IsBusy = false;
        }
        public async Task LoadUsedVouchers()
        {
            IsBusy = true;
            ListUsedVouchers = await _appService.GetUsedVouchers();
            IsBusy = false;
        }

        private IEnumerable<Transactions> _listTransactions;
        public IEnumerable<Transactions> ListTransactions
        {
            get => _listTransactions;
            set => SetProperty(ref _listTransactions, value);
        }

        private IEnumerable<UsedVouchers> _listUsedVouchers;
        public IEnumerable<UsedVouchers> ListUsedVouchers
        {
            get => _listUsedVouchers;
            set => SetProperty(ref _listUsedVouchers, value);
        }

        public Command ItemSelectedCommand { get; set; }

        private void ItemSelected(object selectedItem)
        {
            // Do something
        }
    }
}
