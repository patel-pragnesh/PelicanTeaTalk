using Android.App;
using Android.Widget;
using Prism.Services.Dialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPSTORE.Languages.Texts;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPaymentMethodPage : PopupPage
    {
        private PopupPaymentMethodViewModel _context => (PopupPaymentMethodViewModel)BindingContext;
        private readonly IDialogService _dialogServiceR;
        private OrderModel OrderModelRec = new OrderModel();

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

        public PopupPaymentMethodPage(OrderModel orderModel)
        {
            InitializeComponent();
            OrderModelRec = orderModel;
        }

        async void OnButtonClickedPayByCash(object sender, EventArgs args)
        {
            Toast.MakeText(Application.Context, TextsTranslateManager.Translate("OrderCreated"), ToastLength.Short).Show();
            GC.Collect();
            Navigation.PushAsync(new OrdersPage());
            await PopupNavigation.Instance.PopAsync();           
        }

        async void OnButtonClickedPayByCard(object sender, EventArgs args)
        {
            Navigation.PushAsync(new PayByCardPage("",OrderModelRec));
            //Navigation.PushAsync(new PayByCardPage());
            await PopupNavigation.Instance.PopAsync();
        }
    }
}