using System.Windows.Input;
using WPSTORE.ViewModels;
using Xamarin.Forms;

namespace WPSTORE.Views
{
    public partial class OrdersPage : ContentPage
    {
        private OrdersViewModel _context => (OrdersViewModel)BindingContext;
        public ICommand ScrollListCommand { get; set; }

        public OrdersPage()
        {
            InitializeComponent();

            ScrollListCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var selectedIndex = _context.CategoryTabItems.IndexOf(_context.CurrentTab);
                    await scrollView.ScrollToAsync((60 * selectedIndex), 0, true); //(scrollView.ContentSize.Width - scrollView.Width), true);
                }
              );
            });
        }
    }
}
