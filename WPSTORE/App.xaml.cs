using Plugin.Multilingual;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Prism.Plugin.Popups;
using System;
using System.Globalization;
using System.Linq;
using WPSTORE.AppSettings;
using WPSTORE.Languages.Texts;
using WPSTORE.Services;
using WPSTORE.Services.Interfaces;
using WPSTORE.Themes;
using WPSTORE.ViewModels;
using WPSTORE.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;
using Plugin.FirebasePushNotification;
using Newtonsoft.Json;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.FileStore;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Net.Http;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WPSTORE
{
    public partial class App : PrismApplication
    {
        public static OAuth2Authenticator Authenticator { get; set; }

        public App() : this(null)
        {

        }
        public App(IPlatformInitializer initializer) : base(initializer) { }
        protected override void OnInitialized()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjg1ODQ0QDMxMzgyZTMyMmUzMFBCWHpIVTQwdVIyWkZSWTZZazczYkVQdWlXVVZINzYzcWVIQnR2UE5GMFE9");

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Push Notification Received");
                System.Diagnostics.Debug.WriteLine($"DATA: {JsonConvert.SerializeObject(p.Data)}");
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Push Notification Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Push Notification Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }
                }
            };

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                String refreshedToken = p.Token;
                String deviceType = "android";

                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    deviceType = "iosfcm";
                }

                String url = $"{GlobalSettings.BaseNotificationEnpoint}/savetoken/?";
                url += "device_token=" + refreshedToken;
                url += "&device_type=" + deviceType;
                url += "&auth_key=32GIQFeAwPDULfbF80PDOk624";
                String myXMLstring = "";
                Task task = new Task(() =>
                {
                    myXMLstring = AccessTheWebAsync(url).Result;
                });
                task.Start();
                task.Wait();
                GlobalSettings.DeviceToken = p.Token;
                System.Diagnostics.Debug.WriteLine(myXMLstring);
            };

            async Task<String> AccessTheWebAsync(String url)
            {
                // You need to add a reference to System.Net.Http to declare client.
                HttpClient client = new HttpClient();

                // GetStringAsync returns a Task<string>. That means that when you await the 
                // task you'll get a string (urlContents).
                Task<string> getStringTask = client.GetStringAsync(url);

                // You can do work here that doesn't rely on the string from GetStringAsync.
                //DoIndependentWork();

                // The await operator suspends AccessTheWebAsync. 
                //  - AccessTheWebAsync can't continue until getStringTask is complete. 
                //  - Meanwhile, control returns to the caller of AccessTheWebAsync. 
                //  - Control resumes here when getStringTask is complete.  
                //  - The await operator then retrieves the string result from getStringTask. 
                string urlContents = await getStringTask;

                // The return statement specifies an integer result. 
                // Any methods that are awaiting AccessTheWebAsync retrieve the length value. 
                return urlContents;
            }

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            var appLanguageCode = string.Empty;
            if (string.IsNullOrEmpty(AppSettings.Settings.LanguageSelected))
            {
                var systemLanguageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (AppSettings.Settings.Languages.Any(x => x.Code.Equals(systemLanguageCode, StringComparison.OrdinalIgnoreCase)))
                    appLanguageCode = systemLanguageCode;
                else
                    appLanguageCode = AppSettings.Settings.DefaultLanguage;
            }
            else
                appLanguageCode = AppSettings.Settings.LanguageSelected;

            CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo(appLanguageCode);
            AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;

            Barrel.ApplicationId = GlobalSettings.CacheApplicationId;

            if (Settings.IsNightModeEnabled == true)
            {
                Application.Current.Resources.Clear();
                Application.Current.Resources = new DarkTheme();
            }
            else
            {
                Application.Current.Resources.Clear();
                Application.Current.Resources = new LightTheme();
            }

            if (GlobalSettings.IsFirstLoad)
            {
                GlobalSettings.IsFirstLoad = false;
                MainPage = new NavigationPage(new OnboardingPage());
            }
            else if (GlobalSettings.User != null)
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () =>
                {
                    var container = ((WPSTORE.App)Application.Current).Container;
                    var dialogService = (IDialogService)container.Resolve(typeof(IDialogService));
                    dialogService.ShowToast(TextsTranslateManager.Translate("ConnectionErrorToast"));
                });
            }
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=5f56f6fe-727d-40e2-b765-3a86d4ab0bd6;",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
            containerRegistry.RegisterForNavigation<NotificationViewPage, NotificationViewModel>();
            containerRegistry.RegisterForNavigation<RedemptionPinPage, RedemptionPinViewModel>();
            containerRegistry.RegisterForNavigation<RedemptionHistoryPage, RedemptionHistoryViewModel>();
            containerRegistry.RegisterForNavigation<VouchersPage, VouchersViewModel>();
            containerRegistry.RegisterForNavigation<CouponsPage, CouponsViewModel>();
            containerRegistry.RegisterForNavigation<CodeScanningPage, CodeScanningViewModel>();
            containerRegistry.RegisterForNavigation<RewardsPage, RewardsViewModel>();
            containerRegistry.RegisterForNavigation<RewardExplorePage, RewardExploreViewModel>();
            containerRegistry.RegisterForNavigation<OnboardingPage, OnboardingViewModel>();
            containerRegistry.RegisterForNavigation<CategoriesPage, CategoriesViewModel>();
            containerRegistry.RegisterForNavigation<OrdersPage, OrdersViewModel>();
            containerRegistry.RegisterForNavigation<OrderDetailsPage, OrderDetailsViewModel>();
            containerRegistry.RegisterForNavigation<CartPage, CartViewModel>();
            containerRegistry.RegisterForNavigation<PopupAddToCartPage, PopupAddToCartViewModel>();
            containerRegistry.RegisterForNavigation<StorePage, StoreViewModel>();
            containerRegistry.RegisterForNavigation<StoreInfoPage, StoreInfoViewModel>();
            containerRegistry.RegisterForNavigation<SliderDetailPage, SliderDetailViewModel>();
            containerRegistry.RegisterForNavigation<PostDetailPage, PostDetailViewModel>();
            containerRegistry.RegisterForNavigation<ProductDetailPage, ProductDetailViewModel>();
            containerRegistry.RegisterForNavigation<ExploringPostPage, ExploringPostViewModel>();
            containerRegistry.RegisterForNavigation<PostListPage, PostListViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<HelpPage, HelpViewModel>();
            containerRegistry.RegisterForNavigation<AccountPage, AccountViewModel>();
            containerRegistry.RegisterForNavigation<StoreRewardsPage, StoreRewardsViewModel>();
            containerRegistry.RegisterForNavigation<AccountInfoPage, AccountInfoViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<PopupPaymentMethodPage, PopupPaymentMethodViewModel>();
            containerRegistry.RegisterForNavigation<PopupSelectThemePage, PopupSelectThemeViewModel>();
            containerRegistry.RegisterForNavigation<PopupUpdateCartItemPage, PopupUpdateCartItemViewModel>();
            containerRegistry.RegisterForNavigation<OrderHistoryPage, OrderHistoryViewModel>();
            containerRegistry.RegisterForNavigation<PopupUpdateShippingInfoPage, PopupUpdateShippingInfoViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterViewModel>();

            containerRegistry.Register<IAppService, AppService>();
            containerRegistry.Register<IWooCommerceService, WooCommerceService>();
            containerRegistry.Register<GoogleMapsApiService>();
            containerRegistry.Register<IRequestService, RequestService>();
            containerRegistry.Register<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<IAuthenticationService, AuthenticationService>();
        }

    }
}
