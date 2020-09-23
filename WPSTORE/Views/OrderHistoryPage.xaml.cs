using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderHistoryPage : ContentPage
    {
        private OrderHistoryViewModel _context => (OrderHistoryViewModel)BindingContext;

        public OrderHistoryPage()
        {
            InitializeComponent();
        }
    }
}