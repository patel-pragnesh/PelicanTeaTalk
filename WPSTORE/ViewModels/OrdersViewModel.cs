using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Extensions;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using WPSTORE.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    [QueryProperty("ProductCategoryId", "productCategoryId")]
    public class OrdersViewModel : BaseViewModel
    {
        private IAppService _appService;
        private readonly IWooCommerceService _wooCommerceService;

        public OrdersViewModel(INavigationService navigationService, IAppService appService,
            IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _appService = appService;
            _wooCommerceService = wooCommerceService;

            WatchCreateOrderCompleted();
            WatchCancelOrder();

            AsyncRunner.Run(Processing());
        }

        private async Task Processing()
        {
            await SetBusyAsync(InitData);
        }

        public string ProductCategoryId
        {
            set
            {
                CategoryId = Convert.ToInt32(value);
            }
        }
        internal int CategoryId;

        private async Task InitData()
        {
            await Task.Delay(300);

            //Load current currency
            var currentCurrency = await _wooCommerceService.GetCurrentCurrency().ConfigureAwait(false);
            GlobalSettings.CurrentCurrency = currentCurrency ?? new CurrencyModel();
            CurrentCurrency = GlobalSettings.CurrentCurrency;

            await WebRequestExecuter.Execute(async () => await _wooCommerceService.GetCategories(), async (catagories) =>
            {
                if (catagories != null && catagories.Any())
                {
                    CategoryTabItems = new ObservableCollection<TabModel>(catagories.Where(x => x.Parent == 0).Select(x => new TabModel
                    //CategoryTabItems = new ObservableCollection<TabModel>(catagories.Select(x => new TabModel
                    {
                        ItemThreshold = -1,
                        ImageUrl = x.Image.ImageUrl,
                        Title = x.Name,
                        CategoryId = x.Id
                    }));

                    CurrentTab = CategoryTabItems[0];

                    //await Task.Delay(300);
                    if (CategoryId > 0)
                    {
                        var categoriesData = catagories.Where(x => x.Parent == 0).ToList();
                        //var categoriesData = catagories.ToList();
                        var index = categoriesData.FindIndex(x => x.Id == CategoryId);
                        CurrentTab = CategoryTabItems[index];
                    }

                    foreach (var tab in CategoryTabItems)
                    {
                        await LoadTabData(tab);
                    }
                }
            }, showConnectionError: false);

            UserInfo = GlobalSettings.User;

            bool isGranted = await AppHelpers.RequestPermission(Permission.Location);
            if (isGranted)
            {
                var myPosition = await AppHelpers.GetCurrentPosition();
                if (myPosition != null)
                {
                    try
                    {
                        var placemarks = await Geocoding.GetPlacemarksAsync(myPosition.Latitude, myPosition.Longitude);
                        var place = placemarks.FirstOrDefault();

                        CurrentAddress = $"{place.FeatureName} {place.Thoroughfare ?? place.SubThoroughfare}, {place.SubAdminArea}, {place.AdminArea}";
                    }
                    catch (FeatureNotSupportedException fnsEx)
                    {
                        // Feature not supported on device
                    }
                    catch (Exception ex)
                    {
                        // Handle exception that may have occurred in geocoding
                    }
                }
            }
        }

        private async Task LoadTabData(TabModel tab)
        {
            var products = await _wooCommerceService.GetProductsByCategoryId($"{tab.CategoryId}", tab.CurrentPage, tab.PageSize).ConfigureAwait(false);
            if (products != null && products.Any())
            {
                tab.ItemThreshold = products.Count >= tab.PageSize ? 0 : -1;
                tab.Products.AddRange(products);
            }
            else
            {
                tab.ItemThreshold = -1;
            }
        }

        private void WatchCreateOrderCompleted()
        {
            MessagingCenter.Instance.SubscribeSafe<string>(this, MessagingCenterKeys.CreateOrderCompleted, async sender =>
            {
                Carts.Clear();
                UpdateCart();
            });
        }
        private void WatchCancelOrder()
        {
            MessagingCenter.Instance.SubscribeSafe<string>(this, MessagingCenterKeys.CancelOrder, async sender =>
            {
                Carts.Clear();
                UpdateCart();
            });
        }
        public ICommand ProductTapCommand => new Command<ProductModel>(async (product) =>
        {
            //await GotoPopupPage(product);
            await GotoProductDetailPage(product);
        });

        //public async Task GotoPopupPage(ProductModel product)
        //{
        //    await PopupNavigation.Instance.PushAsync(new PopupAddToCartPage(OnAddProductToCartCompleted, product));
        //}
        public async Task GotoProductDetailPage(ProductModel product)
        {
            await PopupNavigation.Instance.PushAsync(new ProductDetailPage(OnAddProductToCartCompleted, product));
        }

        private async Task OnAddProductToCartCompleted(CartItemModel cartItem)
        {
            var cart = Carts.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
            if (cart == null)
            {
                Carts.Add(cartItem);
            }
            else
            {
                cart.Quantity = cartItem.Quantity;
            }
            UpdateCart();
        }

        private void UpdateCart()
        {
            TotalItems = Carts.Sum(x => x.Quantity);
            TotalPrice = Carts.Sum(x => x.Quantity * x.Price);
            CartsHasItem = TotalItems > 0;
        }

        public ICommand ViewCartCommand => new Command(async () => await ViewCartCommandAsync());
        private async Task ViewCartCommandAsync()
        {
            if (Carts.Any())
            {
                await NavigationHelpers.GotoPageAsync(nameof(CartPage),
                  new Dictionary<string, object> { { "cartsParam", QueryStringHelpers.GetQueryParam(Carts) } });
            }
        }

        public ICommand ItemThresholdReachedCommand => new Command<TabModel>(async (selectedTab) =>
        {
            await SetBusyAsync(async () => await LoadMoreItems(selectedTab));
        });

        ICommand _refreshCommand = null;

        public ICommand RefreshItemsCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new Xamarin.Forms.Command(async (object obj) => await RefreshingData()));
            }
        }

        private async Task RefreshingData()
        {
            try
            {
                if (CurrentTab.IsRefreshing)
                {
                    CurrentTab.IsRefreshing = false;
                    return;
                }

                CurrentTab.CurrentPage = 1;
                CurrentTab.IsRefreshing = true;
                var products = await _wooCommerceService.GetProductsByCategoryId($"{CurrentTab.CategoryId}", CurrentTab.CurrentPage, CurrentTab.PageSize);

                if (products != null && products.Any())
                {
                    CurrentTab.ItemThreshold = products.Count >= CurrentTab.PageSize ? 0 : -1;
                    CurrentTab.Products.ReplaceRange(products);
                }
                else
                {
                    CurrentTab.ItemThreshold = -1;
                }
            }
            finally
            {
                CurrentTab.IsRefreshing = false;
            }
        }
        private async Task LoadMoreItems(TabModel tab)
        {
            await Task.Delay(200);
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (tab.ItemThreshold == 0)
                {
                    tab.CurrentPage += 1;

                    var products = await _wooCommerceService.GetProductsByCategoryId($"{tab.CategoryId}", tab.CurrentPage, tab.PageSize);
                    if (products != null && products.Any())
                    {
                        tab.ItemThreshold = products.Count >= tab.PageSize ? 0 : -1;
                        tab.Products.AddRange(products);
                    }
                    else
                    {
                        tab.CurrentPage -= 1;
                        tab.ItemThreshold = -1;
                    }
                }
            });
        }

        private UserModel _userInfo;
        public UserModel UserInfo
        {
            get { return _userInfo; }
            set { SetProperty(ref _userInfo, value); }
        }

        private ObservableCollection<ProductModel> _listPopulars;
        public ObservableCollection<ProductModel> ListPopulars
        {
            get => _listPopulars;
            set => SetProperty(ref _listPopulars, value);
        }
        private ObservableCollection<ProductModel> _listBeverages;
        public ObservableCollection<ProductModel> ListBeverages
        {
            get => _listBeverages;
            set => SetProperty(ref _listBeverages, value);
        }
        private ObservableCollection<ProductModel> _listFoods;
        public ObservableCollection<ProductModel> ListFoods
        {
            get => _listFoods;
            set => SetProperty(ref _listFoods, value);
        }

        public List<CartItemModel> Carts { get; set; } = new List<CartItemModel>();

        private int _totalItems;
        public int TotalItems
        {
            get => _totalItems;
            set => SetProperty(ref _totalItems, value);
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private bool _cartsHasItem;
        public bool CartsHasItem
        {
            get => _cartsHasItem;
            set => SetProperty(ref _cartsHasItem, value);
        }

        private string _currentAddress;
        public string CurrentAddress
        {
            get => _currentAddress;
            set => SetProperty(ref _currentAddress, value);
        }

        private CurrencyModel _currentCurrency;
        public CurrencyModel CurrentCurrency
        {
            get => _currentCurrency;
            set => SetProperty(ref _currentCurrency, value);
        }
        /// <summary>
        /// TabItems that will be displayed in the UI
        /// </summary>
        private ObservableCollection<TabModel> _categoryTabItems = new ObservableCollection<TabModel>();
        public ObservableCollection<TabModel> CategoryTabItems
        {
            get => _categoryTabItems;
            set => SetProperty(ref _categoryTabItems, value);
        }

        private TabModel _currentTab;
        public TabModel CurrentTab
        {
            get => _currentTab;
            set => SetProperty(ref _currentTab, value);
        }
    }
}
