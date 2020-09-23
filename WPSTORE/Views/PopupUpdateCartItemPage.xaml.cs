using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupUpdateCartItemPage : PopupPage
    {
        private PopupUpdateCartItemViewModel _context => (PopupUpdateCartItemViewModel)BindingContext;
        
        public PopupUpdateCartItemPage()
        {
            InitializeComponent();
        }

        public PopupUpdateCartItemPage(Func<CartItemModel, Task> callBack, List<CartItemModel> carts, CartItemModel cartItem): this()
        {
            _context.CallBack = callBack;
            _context.Carts = carts;
            _context.CartItem = cartItem;

            _context.ProductTitle = cartItem.ProductName;
            _context.ProductImage = cartItem.ProductImage;
            _context.Price = cartItem.Price;
            _context.Quantity = cartItem.Quantity;
            _context.ShortDescription = cartItem.ShortDescription;
            _context.Description = cartItem.Description;

            _context.TotalPrice = cartItem.TotalPrice;

            _context.UpdateCartButtonUI();
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        private void NumericUpDown_OnValueChanged(object sender, int e)
        {
            _context.OnOrderQuantityChanged(e);
        }
    }
}