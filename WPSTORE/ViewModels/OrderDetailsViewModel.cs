using Plugin.Messaging;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Services.Interfaces;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    [QueryProperty("OrderParam", "orderParam")]
    public class OrderDetailsViewModel : BaseViewModel
    {
        private IAppService _appService;
        private readonly IWooCommerceService _wooCommerceService;
        public OrderDetailsViewModel(INavigationService navigationService, IAppService appService, IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _appService = appService;
            _wooCommerceService = wooCommerceService;

            PhoneCenter = GlobalSettings.HelpCenterContact;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public ICommand RedemptionHistoryCommand => new Command(async () => await RedemptionHistoryCommandAsync());
        private async Task RedemptionHistoryCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(RedemptionHistoryPage));
        }

        public string OrderParam
        {
            set
            {
                AsyncRunner.Run(Processing(value));
            }
        }

        private async Task Processing(string orderParam)
        {
            await Task.Delay(300);

            await SetBusyAsync(async () =>
            {
                Order = QueryStringHelpers.GetData<OrderModel>(orderParam);
                if (Order != null)
                {
                    ReceiverName = Order.Shipping.FirstName + " " + Order.Shipping.LastName;
                    ShippingAddress = Order.Shipping.Address1 + " " + Order.Shipping.Address2 + " " + Order.Shipping.City + " " + Order.Shipping.Postcode;
                }
            });
        }

        public void CallHelp(string mobileNumber)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(mobileNumber);
        }

        private OrderModel _order;
        public OrderModel Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        private string _phoneCenter;
        public string PhoneCenter
        {
            get => _phoneCenter;
            set => SetProperty(ref _phoneCenter, value);
        }

        private string _receiverName;
        public string ReceiverName
        {
            get => _receiverName;
            set => SetProperty(ref _receiverName, value);
        }

        private string _shippingAddr;
        public string ShippingAddress
        {
            get => _shippingAddr;
            set => SetProperty(ref _shippingAddr, value);
        }
    }
}
