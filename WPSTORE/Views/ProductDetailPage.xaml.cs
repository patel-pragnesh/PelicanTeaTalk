using Rg.Plugins.Popup.Pages;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WPSTORE.Models.WooCommerce;
using WPSTORE.ViewModels;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : PopupPage
    {
        private ProductDetailViewModel _context => (ProductDetailViewModel)BindingContext;
        public ProductDetailPage()
        {
            InitializeComponent();
        }

        public ProductDetailPage(Func<CartItemModel, Task> callBack, ProductModel product) : this()
        {
            _context.CallBack = callBack;
            _context.Product = product;

            _context.ProductTitle = product.Name;
            _context.ProductImage = product.ProductImage;
            _context.RegularPrice = product.RegularPrice;
            _context.Price = product.Price;

            //if (!string.IsNullOrEmpty(product.Price))
            //{
            //    _context.TotalPrice = decimal.Parse(product.Price) * 1;
            //}
            if (product.OnSale)
            {
                _context.TotalPrice = decimal.Parse(product.Price) * 1;
            }
            else
            {
                _context.TotalPrice = decimal.Parse(product.RegularPrice) * 1;
            }

            _context.ShortDescription = product.ShortDescription;
            _context.Description = product.Description;
        }
        private void NumericUpDown_OnValueChanged(object sender, int e)
        {
            _context.OnOrderQuantityChanged(e);
        }

        protected void OnImageViewTapped(object sender, EventArgs e)
        {
            try
            {
                string url = ((TappedEventArgs)e).Parameter.ToString();
                new PhotoBrowser
                {
                    Photos = new List<Photo>
                {
                    new Photo
                    {
                        URL = url,
                    }
                },
                    ActionButtonPressed = (index) =>
                    {
                        Debug.WriteLine($"Clicked {index}");
                        PhotoBrowser.Close();
                    },
                    EnableGrid = false,
                }.Show();
            }
            catch (Exception ex)
            {
            }
        }
    }
}