using Rg.Plugins.Popup.Pages;
using System;
using System.Linq;
using System.Threading.Tasks;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupUpdateShippingInfoPage : PopupPage
    {
        private PopupUpdateShippingInfoViewModel _context => (PopupUpdateShippingInfoViewModel)BindingContext;
        
        public PopupUpdateShippingInfoPage()
        {
            InitializeComponent();
        }

        public PopupUpdateShippingInfoPage(Func<CustomerModel, bool, Task> callBack, CustomerModel customerInfo) : this()
        {
            _context.CallBack = callBack;
            _context.CustomerInfo = customerInfo;
        }
    }
}