using Rg.Plugins.Popup.Pages;
using System;
using System.Threading.Tasks;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAddToCartPage : PopupPage
    {
        private PopupAddToCartViewModel _context => (PopupAddToCartViewModel)BindingContext;
        
        public PopupAddToCartPage()
        {
            InitializeComponent();
        }

        public PopupAddToCartPage(Func<CartItemModel, Task> callBack, ProductModel product): this()
        {
            _context.CallBack = callBack;
            _context.Product = product;

            _context.ProductTitle = product.Name;
            _context.ProductImage = product.ProductImage;
            _context.Price = product.RegularPrice;

            if (!string.IsNullOrEmpty(product.RegularPrice))
            {
                _context.TotalPrice = decimal.Parse(product.RegularPrice) * 1;
            }
            _context.ShortDescription = product.ShortDescription;
            _context.Description = product.Description;
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