using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;
using Xamarin.Forms;

namespace WPSTORE.Converters
{
    public class SelectedPaymentMethodToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            var bindingContext = ((ContentPage)parameter).BindingContext as PopupPaymentMethodViewModel;
            var selectedPaymentMethod = bindingContext?.SelectedMethod;

            if (selectedPaymentMethod == null)
                return false;

            var currentPaymentMethod = (PaymentMethodModel)value;
            if (currentPaymentMethod.Id.Equals(selectedPaymentMethod.Id, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
