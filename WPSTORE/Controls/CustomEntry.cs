using System;
using Xamarin.Forms;

namespace WPSTORE.Controls
{
    public class CustomEntry: Entry
    {
        public CustomEntry()
        {
        }
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntry), Color.LightGray);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
    }
}
