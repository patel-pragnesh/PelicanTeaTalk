using System;
using System.Collections.Generic;
using System.Windows.Input;
using WPSTORE.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WPSTORE
{
    public partial class AppShell : Shell
    {
        Random rand = new Random();
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }

        public ICommand HelpCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public ICommand FacebookPageCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public AppShell()
        {
            try 
            {
                InitializeComponent();
            } 
            catch ( Exception e)
            {

            }
            RegisterRoutes();
            BindingContext = this;
        }

        void RegisterRoutes()
        {
            routes.Add(nameof(SliderDetailPage), typeof(SliderDetailPage));
            routes.Add(nameof(VouchersPage), typeof(VouchersPage));
            routes.Add(nameof(PostDetailPage), typeof(PostDetailPage));
            routes.Add(nameof(CategoriesPage), typeof(CategoriesPage));
            routes.Add(nameof(PostListPage), typeof(PostListPage));
            routes.Add(nameof(OrdersPage), typeof(OrdersPage));
            routes.Add(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
            routes.Add(nameof(PopupAddToCartPage), typeof(PopupAddToCartPage));
            routes.Add(nameof(CartPage), typeof(CartPage));
            routes.Add(nameof(StoreInfoPage), typeof(StoreInfoPage));
            routes.Add(nameof(PopupPaymentMethodPage), typeof(PopupPaymentMethodPage));
            routes.Add(nameof(PopupUpdateCartItemPage), typeof(PopupUpdateCartItemPage));
            routes.Add(nameof(OrderHistoryPage), typeof(OrderHistoryPage));
            routes.Add(nameof(PopupUpdateShippingInfoPage), typeof(PopupUpdateShippingInfoPage));
            routes.Add(nameof(RegisterPage), typeof(RegisterPage));
            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

        void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            // Cancel any back navigation
            //if (e.Source == ShellNavigationSource.Pop)
            //{
            //    e.Cancel();
            //}
        }

        void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
        }
    }
}
