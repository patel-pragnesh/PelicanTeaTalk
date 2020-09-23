using WPSTORE.Extensions;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        private CartViewModel _context => (CartViewModel)BindingContext;
        public CartPage()
        {
            InitializeComponent();

            DrawStoreMapsPin();

            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
        }

        private void DrawStoreMapsPin()
        {
            MessagingCenter.Instance.SubscribeSafe<Pin>(this, MessagingCenterKeys.OnDrawMapsPinSelectedStore, async pin =>
            {
                map.Pins.Clear();
                map.Pins.Add(pin);

                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(1000)), true);
            });
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    map.UiSettings.ZoomControlsEnabled = false;
        //    map.UiSettings.MyLocationButtonEnabled = true;
        //}
    }
}