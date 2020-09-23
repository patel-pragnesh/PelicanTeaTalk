using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    /// <summary>
    /// Page to show the no internet connection error
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionErrorPage : PopupPage
    {
        
        private ConnectionErrorViewModel _context => (ConnectionErrorViewModel)BindingContext;
        public ConnectionErrorPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    ErrorImage.IsVisible = false;
                }
            }
            else
            {
                ErrorImage.IsVisible = true;
            }
        }

        private async void OnOkButtonClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(false);
        }
    }
}