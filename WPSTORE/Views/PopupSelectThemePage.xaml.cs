using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupSelectThemePage : PopupPage
    {
        private PopupSelectThemeViewModel _context => (PopupSelectThemeViewModel)BindingContext;
        
        public PopupSelectThemePage()
        {
            InitializeComponent();

        }
    }
}