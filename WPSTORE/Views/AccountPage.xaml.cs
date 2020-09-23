using System;
using WPSTORE.ViewModels;
using Xamarin.Forms;

namespace WPSTORE.Views
{
    public partial class AccountPage : ContentPage
    {
        private SettingsViewModel _context => (SettingsViewModel)BindingContext;
        public AccountPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {

            }
        }

    }
}
