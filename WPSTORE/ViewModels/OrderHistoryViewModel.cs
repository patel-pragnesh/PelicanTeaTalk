using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Models.WooCommerce.Requests;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        private readonly IWooCommerceService _wooCommerceService;

        public OrderHistoryViewModel(INavigationService navigationService,
            IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _wooCommerceService = wooCommerceService;

            AsyncRunner.Run(InitDataAsync());
        }

        private async Task InitDataAsync()
        {
            await Task.Delay(200);

            PageIndex = 1;
            await SetBusyAsync(async ()=>
            {
                await FetchOrdersAsync(GlobalSettings.User.Id);
            });
        }

        private async Task FetchOrdersAsync(int customerId)
        {
            var orderHistoryRequest = new OrderHistoryRequest();
            orderHistoryRequest.CustomerId = customerId;
            orderHistoryRequest.Page = PageIndex;
            orderHistoryRequest.PageSize = GlobalSettings.AppConst.PageSize;

            await WebRequestExecuter.Execute(async () => await _wooCommerceService.GetOrders(orderHistoryRequest), myOrders =>
            {
                if (myOrders != null && myOrders.Any())
                {
                    MyOrders = new ObservableCollection<OrderModel>(myOrders);

                    HasMoreOrders = myOrders.Count >= orderHistoryRequest.PageSize ? true : false;
                }
                else
                {
                    HasMoreOrders = false;
                }
                return Task.CompletedTask;
            });
        }

        private async Task LoadMoreOrders()
        {
            if (HasMoreOrders)
            {
                await Task.Delay(200);

                PageIndex += 1;
                var orderHistoryRequest = new OrderHistoryRequest
                {
                    CustomerId = GlobalSettings.User.Id,
                    Page = PageIndex,
                    PageSize = GlobalSettings.AppConst.PageSize
                };

                await WebRequestExecuter.Execute(async () => await _wooCommerceService.GetOrders(orderHistoryRequest), myOrders =>
                {
                    if (myOrders != null && myOrders.Any())
                    {
                        foreach (var order in myOrders)
                            MyOrders.Add(order);

                        HasMoreOrders = myOrders.Count >= orderHistoryRequest.PageSize ? true : false;
                    }
                    else
                    {
                        PageIndex -= 1;
                        HasMoreOrders = false;
                    }
                    return Task.CompletedTask;
                });
            }
        }

        public ICommand ListViewItemAppearingCommand => new Command<object>(async (data) =>
        {
            var itemEventArgs = data as ItemVisibilityEventArgs;
            if (itemEventArgs.ItemIndex + 1 == MyOrders.Count)
            {
                if (HasMoreOrders)
                    await SetBusyAsync(async () => await LoadMoreOrders(), "Loading ...");
            }
        });

        public ICommand RefreshOrdersCommand => new Command(async () =>
        {
            await Task.Delay(200);

            PageIndex = 1;
            await SetRefreshingAsync(async () => await FetchOrdersAsync(GlobalSettings.User.Id));
        });

        public ICommand OrderTappedCommand => new Command(async (item) =>
        {
            var selectedOrder = item as OrderModel;
            await NavigationHelpers.GotoPageAsync(nameof(OrderDetailsPage),
                new Dictionary<string, object> { { "orderParam", QueryStringHelpers.GetQueryParam(selectedOrder) } });
        });

        internal int PageIndex { get; set; }
        internal bool HasMoreOrders { get; set; }

        private ObservableCollection<OrderModel> _myOrders;
        public ObservableCollection<OrderModel> MyOrders
        {
            get => _myOrders;
            set => SetProperty(ref _myOrders, value);
        }
    }
}
