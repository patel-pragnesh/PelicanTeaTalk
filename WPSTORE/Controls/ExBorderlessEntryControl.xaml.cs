using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExBorderlessEntryControl : ContentView
    {
        public ExBorderlessEntryControl()
        {
            InitializeComponent();
            ExEntryName.TextChanged += OnTextChanged;
            HasError = false;
        }

        public static readonly BindableProperty ExTextProperty = BindableProperty.Create(
                                                         propertyName: nameof(ExText),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(ExBorderlessEntryControl),
                                                         defaultValue: "",
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: ExTextPropertyChanged);
        public static readonly BindableProperty IconProperty = BindableProperty.Create(
                                                         propertyName: nameof(Icon),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(ExBorderlessEntryControl),
                                                         defaultValue: "",
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: IconPropertyChanged);
        public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
                                                         propertyName: nameof(HasError),
                                                         returnType: typeof(bool),
                                                         declaringType: typeof(ExBorderlessEntryControl),
                                                         defaultValue: false,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: LabelErrorPropertyChanged);

        public static readonly BindableProperty ExKeyboardProperty = BindableProperty.Create(
                                                         propertyName: nameof(ExKeyboard),
                                                         returnType: typeof(Keyboard),
                                                         declaringType: typeof(ExBorderlessEntryControl),
                                                         defaultValue: Keyboard.Default,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: KeyboardPropertyChanged);

        public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create(
                                                         propertyName: nameof(ErrorMessage),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(ExBorderlessEntryControl),
                                                         defaultValue: "",
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: ErrorMessagePropertyChanged);

        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(
                                                         propertyName: nameof(PlaceHolder),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(ExBorderlessEntryControl),
                                                         defaultValue: "",
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: PlaceHolderPropertyChanged);
        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
                                                         propertyName: nameof(IsPassword),
                                                         returnType: typeof(bool),
                                                         declaringType: typeof(ExBorderlessEntryControl),
                                                         defaultValue: false,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: IsPasswordPropertyChanged);
        public string ExText
        {
            get { return base.GetValue(ExTextProperty).ToString(); }
            set
            {
                base.SetValue(ExTextProperty, value);
            }
        }
        public string Icon
        {
            get { return base.GetValue(IconProperty).ToString(); }
            set
            {
                base.SetValue(IconProperty, value);
            }
        }
        public Keyboard ExKeyboard
        {
            get { return (Keyboard)base.GetValue(ExKeyboardProperty); }
        }
        public bool HasError
        {
            get { return (bool)base.GetValue(HasErrorProperty); }
            set
            {
                base.SetValue(HasErrorProperty, value);
                ErrorMessageLabel.IsVisible = value;
                ErrorIcon.IsVisible = value;
            }
        }
        public string ErrorMessage
        {
            get { return base.GetValue(ErrorMessageProperty).ToString(); }
        }
        public string PlaceHolder
        {
            get { return base.GetValue(PlaceHolderProperty).ToString(); }
        }

        public bool IsPassword
        {
            get { return (bool)base.GetValue(IsPasswordProperty); }
        }
        private static void ExTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExBorderlessEntryControl)bindable;
            control.ExEntryName.Text = newValue?.ToString();
        }
        private static void IconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExBorderlessEntryControl)bindable;
            control.IconLabel.Text = newValue?.ToString();
        }
        private static void LabelErrorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExBorderlessEntryControl)bindable;
            control.ErrorMessageLabel.IsVisible = Convert.ToBoolean(newValue);
            control.ErrorIcon.IsVisible = Convert.ToBoolean(newValue);
        }
        private static void KeyboardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExBorderlessEntryControl)bindable;
            control.ExEntryName.Keyboard = (Keyboard)newValue;
        }
        private static void ErrorMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExBorderlessEntryControl)bindable;
            control.ErrorMessageLabel.Text = (string)newValue;
        }
        private static void PlaceHolderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExBorderlessEntryControl)bindable;
            control.ExEntryName.Placeholder = (string)newValue;
        }
        private static void IsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExBorderlessEntryControl)bindable;
            control.ExEntryName.IsPassword = Convert.ToBoolean(newValue);
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ExText = e.NewTextValue;
        }
    }
}