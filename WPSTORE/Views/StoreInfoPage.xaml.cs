using System;
using WPSTORE.Extensions;
using WPSTORE.Helpers;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Distance = Xamarin.Forms.GoogleMaps.Distance;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoreInfoPage : ContentPage
    {
        private StoreInfoViewModel _context => (StoreInfoViewModel)BindingContext;
        public StoreInfoPage()
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

        private void GetDirection_Clicked(object sender, EventArgs e)
        {
            AsyncRunner.Run(_context.OpenGetDirections());
        }
    }
}