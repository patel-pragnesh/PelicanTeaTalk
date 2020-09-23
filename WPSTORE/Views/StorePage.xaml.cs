using System.Collections.Generic;
using System.Linq;
using WPSTORE.Extensions;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Distance = Xamarin.Forms.GoogleMaps.Distance;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StorePage : ContentPage
    {
        private StoreViewModel _context => (StoreViewModel)BindingContext;
        public StorePage()
        {
            InitializeComponent();

            WatchDrawMapsPin();

            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
        }

        private void WatchDrawMapsPin()
        {
            MessagingCenter.Instance.SubscribeSafe<List<Pin>>(this, MessagingCenterKeys.OnDrawMapsPin, async pins =>
            {
                map.Pins.Clear();
                foreach (var pin in pins)
                {
                    map.Pins.Add(pin);
                }
                var defaultPlace = pins.FirstOrDefault();

                map.MoveToRegion(MapSpan.FromCenterAndRadius(defaultPlace.Position, Distance.FromMeters(5000)), true);
            });
        }

        private void OnCall_Tapped(object sender, System.EventArgs e)
        {
            if (e is TappedEventArgs tappedEventArgs)
            {
                _context.Call(tappedEventArgs.Parameter.ToString());
            }
        }
    }
}