using Plugin.Messaging;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Models.Categories;
using WPSTORE.Models.Params;
using WPSTORE.Models.Posts;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using WPSTORE.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private IAppService _appService;
        private readonly IWooCommerceService _wooCommerceService;

        private IEnumerable<Category> _categories;
        private ObservableCollection<Post> _sliders;
        private ObservableCollection<Post> _whatsNews;
        private ObservableCollection<Post> _coffeeLover;
        private ObservableCollection<Post> _xstoreLover;
        private ObservableCollection<Post> _news;
        public int CurrentPage { get; set; }

        private int _notificationCounts;
        private Song _song;
        private string _title;
        private Countdown _countdown;
        private TimeSpan _startTime;
        private TimeSpan _remainTime;
        private bool _isPlaying;
        private double _progress;
        private string _icon;

        public HomeViewModel(INavigationService navigationService, IAppService appService, IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _appService = appService;
            _wooCommerceService = wooCommerceService;

            NotificationCounts = GlobalSettings.NotificationCounts;
            NotificationCommand = new Command(NotificationClicked);

            UserInfo = GlobalSettings.User;
            FacebookId = GlobalSettings.FacebookPageId;

            AsyncRunner.Run(Processing());
        }

        public Command NotificationCommand { get; set; }
        private async void NotificationClicked()
        {
            await NavigationService.NavigateAsync("NotificationViewPage");
        }

        public async Task LoadNotifications()
        {
            try
            {
                IsBusy = true;
                int totalItems = 0;
                var notificationResponse = await _appService.GetArchivedNotifications();
                if (notificationResponse != null && notificationResponse.HasData)
                {
                    Notifications = new ObservableCollection<NotificationItem>(notificationResponse.Notifications);
                    totalItems = Notifications.Count;
                }
                NotificationCounts = totalItems;
                //MessagingCenter.Send<string>(totalItems > 0 ? totalItems.ToString() : "", GlobalSettings.MessagingKey.NotificationCountKey);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Processing()
        {
            await SetBusyAsync(InitData);
        }

        public async Task LoadNotification()
        {
            await Task.Delay(100);
            await WebRequestExecuter.Execute(async () => await _appService.GetArchivedNotifications(), async (result) =>
            {
                int totalItems = 0;
                if (result != null && result.HasData)
                {
                    Notifications = new ObservableCollection<NotificationItem>(result.Notifications);
                    totalItems = Notifications.Count;
                }
                NotificationCounts = totalItems;
            });
        }

        private async Task InitData()
        {
            CurrentPage = 1;
            await Task.Delay(100);

            await SetBusyAsync(async () =>
            {
                var wordpressCategoryTask = WebRequestExecuter.Execute(async () => await _appService.GetAllCategories(), async (result) =>
                {
                    if (result != null && result.Any())
                    {
                        Categories = GlobalSettings.WpCategories = result.ToList();

                        ShowButton = false;
                        var slidersTask = _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.Slider));
                        var whatNewsTask = _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.WhatsNews));
                        var newsTask = _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.News));
                        var coffeeLoverTask = _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.CoffeeLover));
                        await Task.WhenAll(slidersTask, whatNewsTask, newsTask, coffeeLoverTask);

                        // <summary>
                        /// Gets/sets the all Slider posts from WP to Banner slider.
                        /// </summary>
                        //var sliders = await _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.Slider));
                        var sliders = slidersTask.Result;
                        if (sliders != null)
                        {
                            Sliders = new ObservableCollection<Post>(sliders.ToList());
                            //Sliders = new ObservableCollection<Post>(posts.Result.ToList());
                        }

                        /// <summary>
                        /// Gets/sets the all What's News posts from WP to Horizontal List Layout.
                        /// </summary>
                        //var whatnews = await _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.WhatsNews));

                        var whatnews = whatNewsTask.Result;
                        if (whatnews != null)
                        {
                            WhatsNews = new ObservableCollection<Post>(whatnews.ToList());
                        }

                        /// <summary>
                        /// Gets/sets the all News posts from WP to Horizontal List Layout.
                        /// </summary>
                        //var news = await _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.News));

                        var news = newsTask.Result;
                        if (news != null)
                        {
                            News = new ObservableCollection<Post>(news.ToList());
                            SelectedNews = News.FirstOrDefault();
                        }

                        /// <summary>
                        /// Gets/sets the all Coffee Lover posts from WP to Horizontal List Layout.
                        /// </summary>
                        //var events = await _appService.GetAllPostsByCategory(AppHelpers.GetCategoryId(GlobalSettings.Slug.Events));

                        var coffeeLover = coffeeLoverTask.Result;
                        if (coffeeLover != null)
                        {
                            CoffeeLover = new ObservableCollection<Post>(coffeeLover.ToList());
                            SelectedCoffeeLover = CoffeeLover.FirstOrDefault();
                        }
                        ShowButton = true;
                    }
                });

                var woocommerceTask = WebRequestExecuter.Execute(async () => await _wooCommerceService.GetCategories(), async (wcCatagories) =>
                {
                    if (wcCatagories != null && wcCatagories.Any())
                    {
                        ProductCategories = new ObservableCollection<CategoryModel>(wcCatagories.Where(x => x.Parent == 0).ToList());
                    }
                }, showConnectionError: false);


                //var featureProductTask = WebRequestExecuter.Execute(async () => await _wooCommerceService.GetAllProducts(CurrentPage, GlobalSettings.AppConst.PageSize), async (allProducts) =>
                //{
                //    if (allProducts != null && allProducts.Any())
                //    {
                //        FeaturedProducts = new ObservableCollection<ProductModel>(allProducts.Where(x => x.Featured == true).ToList());
                //    }
                //}, showConnectionError: false);

                await Task.WhenAll(wordpressCategoryTask, woocommerceTask);


            });
        }

        private ObservableCollection<CategoryModel> _wcCatagories;
        public ObservableCollection<CategoryModel> ProductCategories
        {
            get { return _wcCatagories; }
            set { SetProperty(ref _wcCatagories, value); }
        }

        private ObservableCollection<ProductModel> _allProducts;
        public ObservableCollection<ProductModel> FeaturedProducts
        {
            get { return _allProducts; }
            set { SetProperty(ref _allProducts, value); }
        }
        private ObservableCollection<NotificationItem> _notifications;
        public ObservableCollection<NotificationItem> Notifications
        {
            get { return _notifications; }
            set { SetProperty(ref _notifications, value); }
        }

        public int NotificationCounts
        {
            get => _notificationCounts;
            set => SetProperty(ref _notificationCounts, value);
        }

        private int _currentFeaturedIndex;
        public int CurrentFeaturedIndex
        {
            get => _currentFeaturedIndex;
            set => SetProperty(ref _currentFeaturedIndex, value);
        }

        //Wordpress user
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private UserModel _userInfo;
        public UserModel UserInfo
        {
            get { return _userInfo; }
            set { SetProperty(ref _userInfo, value); }
        }

        private int _currentIndex;
        public int CurrentIndex
        {
            get => _currentIndex;
            set => SetProperty(ref _currentIndex, value);
        }
        public ObservableCollection<Post> Sliders
        {
            get => _sliders;
            set => SetProperty(ref _sliders, value);
        }
        public ObservableCollection<Post> WhatsNews
        {
            get => _whatsNews;
            set => SetProperty(ref _whatsNews, value);
        }

        public ObservableCollection<Post> News
        {
            get => _news;
            set => SetProperty(ref _news, value);
        }
        private Post _selectedNews;
        public Post SelectedNews
        {
            get => _selectedNews;
            set => SetProperty(ref _selectedNews, value);
        }

        public ObservableCollection<Post> CoffeeLover
        {
            get => _coffeeLover;
            set => SetProperty(ref _coffeeLover, value);
        }

        private Post _selectedCoffeeLover;
        public Post SelectedCoffeeLover
        {
            get => _selectedCoffeeLover;
            set => SetProperty(ref _selectedCoffeeLover, value);
        }
        public ObservableCollection<Post> XStoreLover
        {
            get => _xstoreLover;
            set => SetProperty(ref _xstoreLover, value);
        }
        private Post _selectedXStoreLover;
        public Post SelectedXStoreLover
        {
            get => _selectedXStoreLover;
            set => SetProperty(ref _selectedXStoreLover, value);
        }

        public IEnumerable<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private bool _showButton;
        public bool ShowButton
        {
            get => _showButton;
            set => SetProperty(ref _showButton, value);
        }
        private string _facebookId;
        public string FacebookId
        {
            get => _facebookId;
            set => SetProperty(ref _facebookId, value);
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

        //Goto clicked slider detail page
        public async Task GotoDetailPage()
        {
            if (Sliders != null && Sliders.Any())
            {
                string url = Sliders.ElementAt(CurrentIndex)?.content?.rendered?.RemoveHtmlTag();

                await NavigationHelpers.GotoPageAsync(nameof(SliderDetailPage),
                new Dictionary<string, object> { { "articleUrl", url } });
            }
        }

        public ICommand RedemptionPinCommand => new Command(async () => await RedemptionPin());
        private async Task RedemptionPin()
        {
            await NavigationService.NavigateAsync(nameof(RedemptionPinPage));
        }

        public ICommand OrderCommand => new Command(async () => await OrderCommandAsync());
        private async Task OrderCommandAsync()
        {
            await Shell.Current.GoToAsync(nameof(OrdersPage), true);
        }

        public ICommand ProductTapCommand => new Command<ProductModel>(async (product) =>
        {
            await GotoProductDetailPage(product);
        });

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


        public ICommand VouchersCommand => new Command(async () => await VouchersCommandAsync());
        private async Task VouchersCommandAsync()
        {

            await NavigationService.NavigateAsync(nameof(VouchersPage));
        }

        public ICommand RewardsCommand => new Command(async () => await RewardsCommandAsync());
        private async Task RewardsCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(RewardsPage));
        }

        public ICommand CategoriesTapCommand => new Command(async (item) =>
        {
            var category = item as CategoryModel;
            await NavigationHelpers.GotoPageAsync(nameof(OrdersPage),
                new Dictionary<string, object> { { "productCategoryId", category.Id } });
        });

        public ICommand PostTapCommand => new Command<Post>(async (item) =>
        {
            await GotoDetailPage(item);
        });
        public ICommand XStoreLoverTapCommand => new Command(async (item) =>
        {
            var post = item as Post;
            await GotoDetailPage(post);
        });

        public ICommand SlidersTapCommand => new Command(async (item) =>
        {
            var post = item as Post;
            await GotoDetailPage(post);
        });

        public async Task GotoCategoriesPage()
        {
            await NavigationHelpers.GotoPageAsync(nameof(CategoriesPage));
        }

        public async Task GotoSlideDetailPage()
        {
            if (Sliders !=null  && Sliders.Any())
            {
                var currentSlide = Sliders.ElementAt(CurrentFeaturedIndex);
                await NavigationHelpers.GotoPageAsync(nameof(PostDetailPage),
                new Dictionary<string, object> { { "postParam", QueryStringHelpers.GetQueryParam(currentSlide) } });
            }
        }
        public async Task GotoDetailPage(Post posts)
        {
            await NavigationHelpers.GotoPageAsync(nameof(PostDetailPage),
                new Dictionary<string, object> { { "postParam", QueryStringHelpers.GetQueryParam(posts) } });
        }

        public async Task GotoPostListPage(int? wpCategoryId)
        {
            string categoryName = AppHelpers.GetCategoryName(wpCategoryId);
            var postListParam = new PostListParamModel
            {
                WpCategoryId = wpCategoryId,
                WpCategoryName = categoryName
            };

            await NavigationHelpers.GotoPageAsync(nameof(PostListPage),
                new Dictionary<string, object> { { "postListParam", QueryStringHelpers.GetQueryParam(postListParam) } });
        }

        public void CallHelp(string mobileNumber)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            try
            {
                if (phoneDialer.CanMakePhoneCall)
                    phoneDialer.MakePhoneCall(mobileNumber);
            }
            catch (Exception e)
            {

            }
        }
        public async Task OpenFacebookChat()
        {
            string uri;
            if (Device.RuntimePlatform == Device.iOS)
            {
                uri = $"fb://page/?id={FacebookId}";
            }
            else
            {
                uri = $"fb://page/{FacebookId}";
            }

            if (await Launcher.CanOpenAsync(uri))
            {
                await Launcher.OpenAsync(uri);
            }
        }

        public void SendEmail(string emailUrl)
        {
            Launcher.OpenAsync(new Uri("mailto:" + emailUrl));
        }
    }
}
