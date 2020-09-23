using System;
using WPSTORE.Controls;
using WPSTORE.ViewModels;
using Xamarin.Forms;

namespace WPSTORE.Views
{
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _context => (SettingsViewModel)BindingContext;
        public SettingsPage()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {

            }
            
        }

        private void languagePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try{
                CustomPicker picker = sender as CustomPicker;
                var selectedItem = picker.SelectedItem;
            }
            catch(Exception ex)
            {

            }

        }
    }
}
