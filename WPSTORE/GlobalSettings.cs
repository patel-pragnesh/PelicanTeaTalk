using Plugin.Settings;
using Plugin.Settings.Abstractions;
using WPSTORE.Models.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using WPSTORE.Models.Users;
using WPSTORE.Helpers;
using WPSTORE.Models.Authentication;
using WPSTORE.Models.SocialLogin;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Models;
using System.Text.RegularExpressions;

namespace WPSTORE
{
    public static class GlobalSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;
        static string defaultServerEndpoint;
        public static string BaseNotificationEnpoint;
        public const string AuthKey = "32GIQFeAwPDULfbF80PDOk624";
        public static string CacheApplicationId = "XStore-e5572938-afb5-43bf-b993-7cd2019dd78d";
        public static Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.None);
        public const string PasswordKey = "WPSTOREPassword";
        static readonly string WpApiPrefix = "wp-json/wp/v2";
        static readonly string JwtPrefix = "wp-json/simple-jwt-authentication/v1";
        static readonly string XStorePrefix = "wp-json/api/xstore";
        public const string WooCommerncePrefix = "wp-json/wc/v3";

        public const string AndroidClientId = "877130854950-fjai37crgsccjmbp8nbhlh49e4tuhsc5.apps.googleusercontent.com";
        public const string AndroidUrlSchema = "com.googleusercontent.apps.877130854950-fjai37crgsccjmbp8nbhlh49e4tuhsc5";

        public const string iOSClientId = "877130854950-j9ettaot9go1jfmk0ut6o1ef1os0s9jb.apps.googleusercontent.com";
        public const string iOSUrlSchema = "com.googleusercontent.apps.877130854950-j9ettaot9go1jfmk0ut6o1ef1os0s9jb";

        public const string FBClientId = "3441019375913751";
        public const string FBClientSecret = "8e061eeb3f3572af9fa2d0b2d9bd666f";

        /// <summary>
        /// Google Map API Key value
        /// </summary>
        public const string GoogleMapsApiKey = "AIzaSyCoJLnIioJVjrjIBY4G5QP8cMu2C6tdV-A";
        //public const string GoogleMapsApiKey_iOS = "AIzaSyDkppZUiFJkcgjN5aNaox6yBuq6us4HhaU";

        /// <summary>
        /// Help Page URL config
        /// Help Center Phone Number of Store
        /// </summary>
        public const string HelpPageUrl = "https://woocommerce.tlssoftwarevn.com/contact-us/";
        public const string HelpCenterContact = "0123456789";

        /// <summary>
        /// Social Floating Button
        /// </summary>
        public const string Question = "https://woocommerce.tlssoftwarevn.com/contact-us/";
        public const string FacebookPageId = "1896702303969748";
        public const string MailBox = "thanhlongsolutions4u@gmail.com";
        public const string Call = "0123456789";

        //woocommerce.tlssoftwarevn.com
        public static string WooCommerceConsumerKey = "ck_78bc345b8685523a6f7504e006bb31f5f6a75139";
        public static string WooCommerceConsumerSecret = "cs_788dd6189b850fdf9ab8a9e4a751651fe0898003";
        // public static string WooCommerceConsumerKey = "ck_30901359285383af1dc8a6aacbd131f88870f165";
        // public static string WooCommerceConsumerSecret = "cs_98d26fe7cbb014b48d3ada90bbfc9c80d45911e8";

        static GlobalSettings()
        {
            // Wordpress site URL
            defaultServerEndpoint = "https://teatalkonline.com";
            BaseNotificationEnpoint = "https://woocommerce.tlssoftwarevn.com/push";
        }
        public static int NotificationCounts
        {
            get => Preferences.Get(nameof(NotificationCounts), 0);
            set => Preferences.Set(nameof(NotificationCounts), value);
        }
        public static class MessagingKey
        {
            public static string NotificationCountKey = "NotificationCount";
        }
        public static bool IsFirstLoad
        {
            get => AppSettings.GetValueOrDefault(nameof(IsFirstLoad), true);
            set => AppSettings.AddOrUpdateValue(nameof(IsFirstLoad), value);
        }
        public static bool RememberMe
        {
            get => Preferences.Get(nameof(RememberMe), false);
            set => Preferences.Set(nameof(RememberMe), value);
        }
        public static string DeviceToken
        {
            get => AppSettings.GetValueOrDefault(nameof(DeviceToken), "");
            set => AppSettings.AddOrUpdateValue(nameof(DeviceToken), value);
        }

        public static DateTime ExpiresDate
        {
            get => Preferences.Get(nameof(ExpiresDate), DateTime.UtcNow);
            set => Preferences.Set(nameof(ExpiresDate), value);
        }

        public static CurrencyModel CurrentCurrency
        {
            get => PreferencesHelpers.Get(nameof(CurrentCurrency), default(CurrencyModel));
            set => PreferencesHelpers.Set(nameof(CurrentCurrency), value);
        }
        public static void SetUserInfo(SocialUser socialUser)
        {
            var user = new UserModel
            {
                Id = socialUser.Id,
                SocialId = socialUser.SocialId,
                Email = socialUser.Email,
                //ExpiresIn = socialUser.ExpiresIn,
                Name = socialUser.Name,
                FirstName = "",
                LastName = socialUser.LastName,
                PictureUrl = socialUser.PictureUrl,
                LoggedInWithSocialAccount = socialUser.LoggedInWithSocialAccount,
                Provider= socialUser.Provider
            };
            User = user;
        }

        public static (string Token, string RefreshToken) GetTokenSecureInfo()
        {
            return ("", "");
        }
        public static UserModel User
        {
            get => PreferencesHelpers.Get(nameof(User), default(UserModel));
            set => PreferencesHelpers.Set(nameof(User), value);
        }


        public static List<Category> WpCategories;

        public static string LoginEndpoint => $"{defaultServerEndpoint}/{JwtPrefix}/token";
        public static string CategoriesEndpoint => $"{defaultServerEndpoint}/{WpApiPrefix}/categories";
        public static string PostsByCategoryEndpoint => $"{defaultServerEndpoint}/{WpApiPrefix}/posts";
        public static string StoreInfoEndpoint => $"{defaultServerEndpoint}/wp-content/uploads/2020/06/Store.txt";
        public static string WpMyProfileEndpoint => $"{defaultServerEndpoint}/{WpApiPrefix}/users/me";

        public static string WpGoogleLoginEndpoint => $"{defaultServerEndpoint}/{XStorePrefix}/google-login";
        public static string WpFacebookLoginEndpoint => $"{defaultServerEndpoint}/{XStorePrefix}/facebook-login";
        public static string WpRegisterAccountEndpoint => $"{defaultServerEndpoint}/{XStorePrefix}/register";

        public static string WooCommerceCategoryEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/products/categories";
        public static string WooCommerceAllProductEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/products";
        public static string WooCommerceProductByCategoryEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/products";
        public static string WooCommercePaymentMethodEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/payment_gateways";
        public static string WooCommerceOrderEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/orders";
        public static string WooCommerceCouponEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/coupons";
        public static string WooCommerceTaxRatesListRequestEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/taxes";
        public static string WooCommerceAllCurrencyEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/data/currencies";
        public static string WooCommerceCurrentCurrencyEndpoint => $"{WooCommerceAllCurrencyEndpoint}/current";
        public static string WooCommerceCountryEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/data/countries";
        public static string WooCommerceCustomerEndpoint => $"{defaultServerEndpoint}/{WooCommerncePrefix}/customers";

        /// <summary>
        /// Define Name and Slug of Wordpress Post Categories
        /// for Horizontal List view items at Home Screen
        /// </summary>
        public static class Slug
        {
            public static string Slider = "slider";
            public static string WhatsNews = "whatsnews";
            public static string News = "news";
            public static string CoffeeLover = "coffeelover";

            public static string Featured = "featured";
            public static string Events = "events";
            public static string Postcategory = "postcategory";
            public static string Promotion = "promotion";
            public static string Walkthrough = "walkthrough";
            public static string Uncategorized = "uncategorized";
        }
        public static class SecureKeys
        {
            public static string Token = "XStoreToken";
            public static string RefreshToken = "XStoreRefreshToken";
        }
        public static class AppConst
        {
            public const string StorePinImage = "store_pin.png";
            public const int PageSize = 50;
        }

        public static class CachedKeys
        {
            public const string CustomerInfo = "CustomerInfo";
            public const string GuestOrderList = "GuestOrderList";
        }
    }
}
