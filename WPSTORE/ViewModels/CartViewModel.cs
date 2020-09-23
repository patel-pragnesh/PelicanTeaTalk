using MonkeyCache.FileStore;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Extensions;
using WPSTORE.Helpers;
using WPSTORE.Languages.Texts;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Models.WooCommerce.Requests;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using WPSTORE.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace WPSTORE.ViewModels
{
    [QueryProperty("CartsParam", "cartsParam")]
    public class CartViewModel : BaseViewModel
    {
        private IAppService _appService;
        private readonly IDialogService _dialogService;
        private readonly IWooCommerceService _wooCommerceService;

        public CartViewModel(INavigationService navigationService, IAppService appService, IDialogService dialogService,
            IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _appService = appService;
            _dialogService = dialogService;
            _wooCommerceService = wooCommerceService;
        }

        public string CartsParam
        {
            set
            {
                AsyncRunner.Run(Processing(value));
            }
        }

        private async Task Processing(string cartParam)
        {
            await Task.Delay(500);

            await SetBusyAsync(async () => await InitData(cartParam));
        }

        private async Task InitData(string cartParam)
        {
            ShippingAddress = "You don't have a shipping address";
            bool hasShipppingAddress = false;
            int customerId = 0;

            CustomerInfo = await _wooCommerceService.GetCustomerInfo(GlobalSettings.User.Id);
            if (CustomerInfo != null && CustomerInfo.Shipping != null && !string.IsNullOrEmpty(CustomerInfo.Shipping.Address1))
            {
                ShippingAddress = CustomerInfo.Shipping.Address1;
                hasShipppingAddress = true;
            }

            Name = GlobalSettings.User.Name;

            var taxRatesListRequest = new TaxRatesListRequest();
            await WebRequestExecuter.Execute(async () => await _wooCommerceService.GetTaxRates(taxRatesListRequest), myTaxRates =>
            {
                if (myTaxRates.Count != 0)
                {
                    TaxRates = new ObservableCollection<TaxRatesModel>(myTaxRates);
                    DefaultTaxRate = TaxRates.FirstOrDefault();
                }
                return Task.CompletedTask;
            });

            var carts = QueryStringHelpers.GetData<List<CartItemModel>>(cartParam);
            Carts = new ObservableCollection<CartItemModel>(carts);

            UpdateOrderSummaryUI();

            var paymentMethods = await _wooCommerceService.GetPaymentMethods();
            if (paymentMethods != null)
            {
                PaymentMethods = paymentMethods.Where(x => x.Enabled).OrderBy(x => x.Order).ToList();
                SelectedMethod = PaymentMethods.FirstOrDefault();

                UpdateSelectedPaymentMethod();
            }

            bool isGranted = await AppHelpers.RequestPermission(Permission.Location);
            if (isGranted && !hasShipppingAddress)
            {
                var myPosition = await AppHelpers.GetCurrentPosition();
                if (myPosition != null)
                {
                    try
                    {
                        var placemarks = await Geocoding.GetPlacemarksAsync(myPosition.Latitude, myPosition.Longitude);
                        var place = placemarks.FirstOrDefault();

                        ShippingAddress = $"{place.FeatureName} {place.Thoroughfare ?? place.SubThoroughfare}, {place.SubAdminArea}, {place.AdminArea}";
                        MapCameraInitPoint = $"{place.Location.Latitude}, {place.Location.Longitude}, 13, 30, 60";

                        var pin = new Pin
                        {
                            Type = PinType.Place,
                            Label = place.CountryName,
                            Address = place.FeatureName,
                            Icon = BitmapDescriptorFactory.FromBundle(GlobalSettings.AppConst.StorePinImage),
                            Position = new Xamarin.Forms.GoogleMaps.Position(myPosition.Latitude, myPosition.Longitude)
                        };
                        MessagingCenter.Send(pin, MessagingCenterKeys.OnDrawMapsPinSelectedStore);
                    }
                    catch (FeatureNotSupportedException fnsEx)
                    {
                        // Feature not supported on device
                    }
                    catch (Exception ex)
                    {
                        // Handle exception that may have occurred in geocoding
                    }
                }
            }
        }


        private void UpdateOrderSummaryUI()
        {
            SubTotal = Carts.Sum(x => x.Quantity * x.Price);
            TaxAmount = (SubTotal * decimal.Parse(DefaultTaxRate.Rate ?? "0"))/100;
            Total = SubTotal + TaxAmount;
        }

        public ICommand ChoosePaymentMethodCommand => new Command(async () => await ChoosePaymentMethodAsync());
        private async Task ChoosePaymentMethodAsync()
        {
            await PopupNavigation.Instance.PushAsync(new PopupPaymentMethodPage(OnChoosePaymentMethodCompleted, PaymentMethods, SelectedMethod));
        }
        public ICommand ChooseCouponCommand => new Command(async () => await ChooseCouponAsync());
        private async Task ChooseCouponAsync()
        {
            await NavigationService.NavigateAsync(nameof(CouponsPage));
        }
        public ICommand UpdateCartItemCommand => new Command<CartItemModel>(async (cartItem) =>
        {
            await PopupNavigation.Instance.PushAsync(new PopupUpdateCartItemPage(OnUpdateCartItemCompleted, Carts.ToList(), cartItem));
        });
        private async Task OnUpdateCartItemCompleted(CartItemModel cartItem)
        {
            if (cartItem.Quantity == 0)
            {
                Carts.Remove(cartItem);
            }
            else
            {
                var cart = Carts.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
                cart.Quantity = cartItem.Quantity;
            }
            UpdateOrderSummaryUI();
        }

        public ICommand CreateNewOrderCommand => new Command(async () => await CreateOrderAsync());

        private async Task CreateOrderAsync()
        {
            if (!AlreadyShowConfirm)
            {
                var customerCached = Barrel.Current.Get<CustomerModel>(key: GlobalSettings.CachedKeys.CustomerInfo);
                if (customerCached != null)
                    CustomerInfo = customerCached;

                if (CustomerInfo == null)
                {
                    CustomerInfo = new CustomerModel()
                    {
                        Billing = new Billing() { Address1 = ShippingAddress },
                        Shipping = new Shipping() { Address1 = ShippingAddress }
                    };
                }
                else
                {
                    if (string.IsNullOrEmpty(CustomerInfo.Billing.Address1))
                        CustomerInfo.Billing.Address1 = ShippingAddress;

                    if (string.IsNullOrEmpty(CustomerInfo.Shipping.Address1))
                        CustomerInfo.Shipping.Address1 = ShippingAddress;
                }
                await PopupNavigation.Instance.PushAsync(new PopupUpdateShippingInfoPage(OnUpdateBillingInfoCompleted, CustomerInfo));
                return;
            }
            else
            {
                var orderRequest = new NewOrderRequest
                {
                    CustomerId = CustomerInfo.Id,
                    PaymentMethod = SelectedMethod.Id,
                    PaymentMethodTitle = SelectedMethod.MethodTitle,
                    Billing = CustomerInfo.Billing,
                    Shipping = CustomerInfo.Shipping,
                    LineItems = Carts.Select(x => new LineItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToArray(),
                    CustomerNote = CustomerNote
                };

                await SetBusyAsync(async () => await WebRequestExecuter.Execute(async () => await _wooCommerceService.CreateOrder(orderRequest), async result =>
                {
                    if (result != null)
                    {
                        if(CustomerInfo.Id == 0)
                        {
                            //Guest User
                            var guestOrderIds = Barrel.Current.Get<List<int>>(GlobalSettings.CachedKeys.GuestOrderList) ?? new List<int>();
                            guestOrderIds.Add(result.Id);
                            Barrel.Current.Add(key: GlobalSettings.CachedKeys.GuestOrderList, data: guestOrderIds, expireIn: TimeSpan.FromDays(90));
                        }
                        MessagingCenter.Send("", MessagingCenterKeys.CreateOrderCompleted);
                        _dialogService.ShowToast(TextsTranslateManager.Translate("OrderCreated"), 800);
                        await Shell.Current.Navigation.PopAsync(animated: false);
                    }
                }));
            }
        }

        private async Task OnUpdateBillingInfoCompleted(CustomerModel customerInfo, bool shipToDifferentAddress)
        {
            CustomerInfo.Billing = customerInfo.Billing;

            var firstName = customerInfo.Shipping.FirstName;
            var lastName = customerInfo.Shipping.LastName;
            var address = customerInfo.Shipping.Address1;
            var city = customerInfo.Shipping.City ?? "";
            var state = customerInfo.Shipping.State ?? "";
            var postCode = customerInfo.Shipping.Postcode;

            CustomerInfo.Shipping = new Shipping
            {
                FirstName = customerInfo.Billing.FirstName,
                LastName = customerInfo.Billing.LastName,
                Address1 = customerInfo.Billing.Address1,
                Address2 = customerInfo.Billing.Address2 ?? "",
                City = customerInfo.Billing.City,
                Country = customerInfo.Billing.Country,
                State = customerInfo.Billing.State ?? "",
                Postcode = customerInfo.Billing.Postcode ?? "",
                Email = customerInfo.Billing.Email,
                Phone = customerInfo.Billing.Phone,
            };
            if(shipToDifferentAddress)
            {
                CustomerInfo.Shipping.FirstName = firstName;
                CustomerInfo.Shipping.LastName = lastName;
                CustomerInfo.Shipping.Address1 = address;
                CustomerInfo.Shipping.Address2 = "";
                CustomerInfo.Shipping.City = city;
                CustomerInfo.Shipping.State = state;
                CustomerInfo.Shipping.Postcode = postCode;
            }

            AlreadyShowConfirm = true;
            Barrel.Current.Add(key: GlobalSettings.CachedKeys.CustomerInfo, data: CustomerInfo, expireIn: TimeSpan.FromDays(1));

            ShippingAddress = customerInfo.Shipping.Address1 + "," + customerInfo.Shipping.City + " " + customerInfo.Shipping.Postcode;
            Name = $"{customerInfo.Shipping.LastName} {customerInfo.Shipping.FirstName}";
            PhoneNumber = customerInfo.Shipping.Phone;
        }
        private async Task OnChoosePaymentMethodCompleted(PaymentMethodModel paymentMethod)
        {
            SelectedMethod = paymentMethod;

            UpdateSelectedPaymentMethod();
        }

        private void UpdateSelectedPaymentMethod()
        {
            PaymentMethod = SelectedMethod?.Title;
        }

        internal bool AlreadyShowConfirm { get; set; }

        internal CustomerModel CustomerInfo { get; set; }

        private ObservableCollection<TaxRatesModel> _taxRates;
        public ObservableCollection<TaxRatesModel> TaxRates
        {
            get => _taxRates;
            set => SetProperty(ref _taxRates, value);
        }
        private TaxRatesModel _defaultTaxRate;
        public TaxRatesModel DefaultTaxRate
        {
            get => _defaultTaxRate;
            set => SetProperty(ref _defaultTaxRate, value);
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _shippingAddress;
        public string ShippingAddress
        {
            get => _shippingAddress;
            set => SetProperty(ref _shippingAddress, value);
        }

        private List<PaymentMethodModel> _paymentMethods;
        public List<PaymentMethodModel> PaymentMethods
        {
            get => _paymentMethods;
            set => SetProperty(ref _paymentMethods, value);
        }

        private PaymentMethodModel _selectedMethod;
        public PaymentMethodModel SelectedMethod
        {
            get => _selectedMethod;
            set => SetProperty(ref _selectedMethod, value);
        }

        private ObservableCollection<CartItemModel> _carts;
        public ObservableCollection<CartItemModel> Carts
        {
            get => _carts;
            set => SetProperty(ref _carts, value);
        }

        private decimal _subTotal;
        public decimal SubTotal
        {
            get => _subTotal;
            set => SetProperty(ref _subTotal, value);
        }

        private decimal _taxAmount;
        public decimal TaxAmount
        {
            get => _taxAmount;
            set => SetProperty(ref _taxAmount, value);
        }

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private string _paymentMethod;
        public string PaymentMethod
        {
            get => _paymentMethod;
            set => SetProperty(ref _paymentMethod, value);
        }

        private string _customerNote;
        public string CustomerNote
        {
            get => _customerNote;
            set => SetProperty(ref _customerNote, value);
        }

        private string _mapCameraInitPoint;
        public string MapCameraInitPoint
        {
            get => _mapCameraInitPoint;
            set => SetProperty(ref _mapCameraInitPoint, value);
        }
    }
}
