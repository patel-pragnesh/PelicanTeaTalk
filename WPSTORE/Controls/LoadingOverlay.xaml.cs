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
    public partial class LoadingOverlay : ContentView
    {
        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(LoadingOverlay));

        public bool IsRunning
        {
            get => (bool)this.GetValue(LoadingOverlay.IsRunningProperty);
            set
            {
                SetValue(IsRunningProperty, (object)value);
            }
        }

        public LoadingOverlay()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.3);
        }
    }
}