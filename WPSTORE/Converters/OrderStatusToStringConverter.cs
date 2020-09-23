using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WPSTORE.Converters
{
    public class OrderStatusToStringConverter : IValueConverter
    {
        /// <summary>
        /// This method is used to convert the bool to color.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the message.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return null;
            }
            if (value == null)
            {
                return null;
            }

            object message;
            switch (parameter.ToString())
            {
                case "0":
                    {
                        if ((string)value == "pending")
                        {
                            message = "We received your order and under checking, we will confirm the status soon";
                            break;
                        }
                        else if ((string)value == "processing")
                        {
                            message = "Your order has been confirmed and under preparation, please wait for a momment";
                            break;
                        }
                        else if ((string)value == "on-hold")
                        {
                            message = "Your order currently holding";
                            break;
                        }
                        else if ((string)value == "completed")
                        {
                            message = "Your order has been processed successfully. Enjoy your item !";
                            break;
                        }
                        else if ((string)value == "cancelled")
                        {
                            message = "Your order has been cancelled. We're sorry about this inconvenience.";
                            break;
                        }
                        else if ((string)value == "refunded")
                        {
                            message = "";
                            break;
                        }
                        else if ((string)value == "failed")
                        {
                            message = "";
                            break;
                        }
                        else
                        {
                            message = "";
                            break;
                        }
                    }

                default:
                    return null;
            }

            return message;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
