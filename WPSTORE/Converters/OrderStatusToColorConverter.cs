using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Globalization;
using System.Text;

namespace WPSTORE.Converters
{
    public class OrderStatusToColorConverter : IValueConverter
    {
        /// <summary>
        /// This method is used to convert the bool to color.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return Color.Default;
            }
            if (value == null)
            {
                return Color.Default;
            }
            switch (parameter.ToString())
            {
                case "0":
                    {
                        if ((string)value == "pending")
                        {
                            Application.Current.Resources.TryGetValue("colPen", out var retRed);
                            return retRed;
                        }
                        else if ((string)value == "processing")
                        {
                            Application.Current.Resources.TryGetValue("colProc", out var retOrange);
                            return retOrange;
                        }
                        else if ((string)value == "on-hold")
                        {
                            Application.Current.Resources.TryGetValue("colHold", out var retOrange);
                            return retOrange;
                        }
                        else if ((string)value == "completed")
                        {
                            Application.Current.Resources.TryGetValue("colComp", out var retGreen);
                            return retGreen;
                        }
                        else if ((string)value == "cancelled")
                        {
                            Application.Current.Resources.TryGetValue("colCan", out var retRed);
                            return retRed;
                        }
                        else if ((string)value == "refunded")
                        {
                            Application.Current.Resources.TryGetValue("colRef", out var retYellow);
                            return retYellow;
                        }
                        else if ((string)value == "failed")
                        {
                            Application.Current.Resources.TryGetValue("colFail", out var retRed);
                            return retRed;
                        }
                        else
                        {
                            Application.Current.Resources.TryGetValue("colPrim", out var retRed);
                            return retRed;
                        }
                    }

                default:
                    return Color.Transparent;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
