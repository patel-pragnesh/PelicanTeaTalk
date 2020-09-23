using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using WPSTORE.Helpers;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Services.Interfaces;

namespace WPSTORE.ViewModels
{
    //[QueryProperty("ProductParam", "productParam")]
    public class ProductDetailViewModel : BaseViewModel
    {
        private IAppService _appService;
        private readonly IWooCommerceService _wooCommerceService;
        public ProductDetailViewModel(INavigationService navigationService, IAppService appService, IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _appService = appService;
            _wooCommerceService = wooCommerceService;
        }
        //private async Task Processing(string productParam)
        //{
        //    await Task.Delay(300);

        //    await SetBusyAsync(async () => await BindData(productParam));
        //}
        //private async Task BindData(string productParam)
        //{
        //    Product = QueryStringHelpers.GetData<ProductModel>(productParam);
            
        //}

        public void OnOrderQuantityChanged(int quantity)
        {
            if (Product != null)
            {
                if (Product.OnSale)
                {
                    TotalPrice = decimal.Parse(Product.Price) * quantity;
                }
                else
                {
                    TotalPrice = decimal.Parse(Product.RegularPrice) * quantity;
                }
            }    
        }

        public ICommand AddToCartCommand => new Command(async () =>
        {
            CallBack?.Invoke(new CartItemModel
            {
                ProductId = Product.Id,
                ProductName = ProductTitle,
                ShortDescription = Product.ShortDescription,
                Description = Product.Description,
                ProductImage = Product.ProductImage,
                Quantity = Quantity,
                Price = Product.OnSale ? decimal.Parse(Product.Price ?? "0") : decimal.Parse(Product.RegularPrice ?? "0")
                //Price = decimal.Parse(Price ?? "0")
            });

            //await Shell.Current.Navigation.PopAsync(animated: false);
            await PopupNavigation.Instance.PopAsync(false);
        });

        internal Func<CartItemModel, Task> CallBack { get; set; }

        private ProductModel _product;
        public ProductModel Product
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }
        private string _productImage;
        public string ProductImage
        {
            get { return _productImage; }
            set { SetProperty(ref _productImage, value); }
        }
        private string _productTitle;
        public string ProductTitle
        {
            get { return _productTitle; }
            set { SetProperty(ref _productTitle, value); }
        }
        private string _regularPrice;
        public string RegularPrice
        {
            get { return _regularPrice; }
            set { SetProperty(ref _regularPrice, value); }
        }
        private string _price;
        public string Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private int _quantity = 1;
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }

        private string _shortDescription;
        public string ShortDescription
        {
            get { return _shortDescription; }
            set { SetProperty(ref _shortDescription, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        //public string ProductParam
        //{
        //    set
        //    {
        //        AsyncRunner.Run(Processing(value));
        //    }
        //}
    }
}
