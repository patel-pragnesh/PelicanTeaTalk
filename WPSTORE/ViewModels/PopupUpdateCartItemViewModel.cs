using Acr.UserDialogs;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Services.Interfaces;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class PopupUpdateCartItemViewModel : BaseViewModel
    {
        private readonly IAppService _appService;
        
        public PopupUpdateCartItemViewModel(INavigationService navigationService, IAppService AppService) : base(navigationService)
        {
            _appService = AppService;
        }

        public void OnOrderQuantityChanged(int quantity)
        {
            if(CartItem != null)
                TotalPrice = CartItem.Price * quantity;
            UpdateCartButtonUI();
        }

        public async Task UpdateCartButtonUI()
        {
            CartButtonText = $"{TotalPrice}";

            if ((int)TotalPrice == 0)
            {
                CartButtonText = "Remove item";
            } 
        }
        public ICommand UpdateCartItemCommand => new Command(async() =>
        {
            CartItem.Quantity = Quantity;

            if (Carts.Count == 1 && (int)TotalPrice == 0)
            {
                var result = await UserDialogs.Instance.ConfirmAsync("You have to choose at least 1 item in your cart", "Info", "OK", "Cancel");
                if (result)
                {
                    await PopupNavigation.Instance.PopAsync(false);
                    await Shell.Current.Navigation.PopAsync();

                    MessagingCenter.Send<string>("", MessagingCenterKeys.CancelOrder);
                }
            }
            else
            {
                CallBack?.Invoke(CartItem);
                await PopupNavigation.Instance.PopAsync(false);
            }
        });

        internal Func<CartItemModel, Task> CallBack { get; set; }
        internal List<CartItemModel> Carts { get; set; }

        private string _cartButtonText;
        public string CartButtonText
        {
            get { return _cartButtonText; }
            set { SetProperty(ref _cartButtonText, value); }
        }

        private CartItemModel _cartItem;
        public CartItemModel CartItem
        {
            get { return _cartItem; }
            set { SetProperty(ref _cartItem, value); }
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
        private decimal _price;
        public decimal Price
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
