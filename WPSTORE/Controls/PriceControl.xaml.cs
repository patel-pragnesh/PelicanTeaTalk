using WPSTORE.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PriceControl : StackLayout
    {
        public PriceControl()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty PriceProperty = BindableProperty.Create(nameof(Price), typeof(string), typeof(PriceControl), null);
        public static readonly BindableProperty PriceFontSizeProperty = BindableProperty.Create(nameof(PriceFontSize), typeof(double), typeof(PriceControl), defaultValue:14.0, BindingMode.OneWay, null);
        public static readonly BindableProperty PriceTextColorProperty = BindableProperty.Create(nameof(PriceTextColor), typeof(Color), typeof(PriceControl), null);
        public static readonly BindableProperty CurrencySymbolProperty = BindableProperty.Create(nameof(CurrencySymbol), typeof(string), typeof(PriceControl), null, propertyChanged: CurrencySymbolPropertyChanged);
        public static readonly BindableProperty CurrencySymbolTextColorProperty = BindableProperty.Create(nameof(CurrencySymbolTextColor), typeof(Color), typeof(PriceControl), null);
        public static readonly BindableProperty PriceFontFamilyProperty = BindableProperty.Create(nameof(PriceFontFamily), typeof(string), typeof(PriceControl), null);
        public static readonly BindableProperty SymbolFontFamilyProperty = BindableProperty.Create(nameof(SymbolFontFamily), typeof(string), typeof(PriceControl), null);

        public string Price
        {
            get { return (string)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }
        public Color PriceTextColor
        {
            get { return (Color)GetValue(PriceTextColorProperty); }
            set { SetValue(PriceTextColorProperty, value); }
        }
        public string CurrencySymbol
        {
            get { return (string)GetValue(CurrencySymbolProperty); }
            set { SetValue(CurrencySymbolProperty, value); }
        }
        public Color CurrencySymbolTextColor
        {
            get { return (Color)GetValue(CurrencySymbolTextColorProperty); }
            set { SetValue(CurrencySymbolTextColorProperty, value); }
        }
        public string PriceFontFamily
        {
            get { return (string)GetValue(PriceFontFamilyProperty); }
            set { SetValue(PriceFontFamilyProperty, value); }
        }
        public string SymbolFontFamily
        {
            get { return (string)GetValue(SymbolFontFamilyProperty); }
            set { SetValue(SymbolFontFamilyProperty, value); }
        }

        public double PriceFontSize
        {
            get { return (double)GetValue(PriceFontSizeProperty); }
            set { SetValue(PriceFontSizeProperty, value); }
        }

        private static void CurrencySymbolPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PriceControl)bindable;

            if (newValue != null && GlobalSettings.CurrentCurrency != null && !string.IsNullOrEmpty(GlobalSettings.CurrentCurrency.Code))
            {
                string symbol;
                if(CurrencyHelpers.TryGetCurrencySymbol(GlobalSettings.CurrentCurrency.Code, out symbol))
                {
                    control.CurrencySymbol = symbol;
                }
            }
        }
    }
}