using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        private OrderDetailsViewModel _context => (OrderDetailsViewModel)BindingContext;
        public OrderDetailsPage()
        {
            InitializeComponent();
        }

        private void OnHelp_Tapped(object sender, System.EventArgs e)
        {
            if (e is TappedEventArgs tappedEventArgs)
            {
                _context.CallHelp(tappedEventArgs.Parameter.ToString());
            }
        }
    }
}