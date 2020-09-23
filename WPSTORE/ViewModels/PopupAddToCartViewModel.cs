using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Languages.Texts;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Services.Interfaces;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class PopupAddToCartViewModel : BaseViewModel
    {
        private readonly IAppService _appService;
        private readonly IDialogService _dialogService;
        
        public PopupAddToCartViewModel(INavigationService navigationService, IAppService AppService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _appService = AppService;
        }

        public void OnOrderQuantityChanged(int quantity)
        {
            if(Product != null)
                TotalPrice =  decimal.Parse(Product.RegularPrice) * quantity;
        }

        public ICommand AddToCartCommand => new Command(async() =>
        {
            CallBack?.Invoke(new CartItemModel
            {
                ProductId = Product.Id,
                ProductName = ProductTitle,
                ShortDescription = Product.ShortDescription,
                Description = Product.Description,
                ProductImage = Product.ProductImage,
                Quantity = Quantity,
                Price = decimal.Parse(Price ?? "0")
            });
            _dialogService.ShowToast(string.Format(TextsTranslateManager.Translate("ItemAddedToCart"), Product.Name), 800);
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
    }
}
