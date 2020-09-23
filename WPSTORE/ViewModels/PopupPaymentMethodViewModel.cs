using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Models.WooCommerce;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class PopupPaymentMethodViewModel : BaseViewModel
    {
        public PopupPaymentMethodViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public ICommand SelectPaymentMethodCommand => new Command<PaymentMethodModel>(async (selectedMethod) =>
        {
            CallBack?.Invoke(selectedMethod);
            await PopupNavigation.Instance.PopAsync(false);
        });
        
        public ICommand CancelCommand => new Command(async () =>
        {
            await PopupNavigation.Instance.PopAsync(false);
        });

        internal Func<PaymentMethodModel, Task> CallBack { get; set; }

        private ObservableCollection<PaymentMethodModel> _paymentMethods;
        public ObservableCollection<PaymentMethodModel> PaymentMethods
        {
            get { return _paymentMethods; }
            set { SetProperty(ref _paymentMethods, value); }
        }

        private PaymentMethodModel _selectedMethod;
        public PaymentMethodModel SelectedMethod
        {
            get { return _selectedMethod; }
            set { SetProperty(ref _selectedMethod, value); }
        }
    }
}
