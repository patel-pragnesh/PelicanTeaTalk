using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPaymentMethodPage : PopupPage
    {
        private PopupPaymentMethodViewModel _context => (PopupPaymentMethodViewModel)BindingContext;
        
        public PopupPaymentMethodPage()
        {
            InitializeComponent();
        }

        public PopupPaymentMethodPage(Func<PaymentMethodModel, Task> callBack, List<PaymentMethodModel> paymentMethods, PaymentMethodModel selectedMethod): this()
        {
            _context.CallBack = callBack;
            _context.SelectedMethod = selectedMethod;
            _context.PaymentMethods = new ObservableCollection<PaymentMethodModel>(paymentMethods);
        }
    }
}