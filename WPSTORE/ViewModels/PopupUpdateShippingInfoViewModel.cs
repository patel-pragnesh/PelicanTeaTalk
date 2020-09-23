using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Languages.Texts;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Models.WooCommerce.Requests;
using WPSTORE.Services.Interfaces;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class PopupUpdateShippingInfoViewModel : BaseViewModel
    {
        private readonly IWooCommerceService _wooCommerceService;

        public PopupUpdateShippingInfoViewModel(INavigationService navigationService,
            IWooCommerceService wooCommerceService) : base(navigationService)
        {
            _wooCommerceService = wooCommerceService;

            InitEntryPlaceHolder();
            InitErrMessages();

            AsyncRunner.Run(InitData());
        }

        private void InitEntryPlaceHolder()
        {
            FirstNamePld = TextsTranslateManager.Translate("FirstNamePld");
            LastNamePld = TextsTranslateManager.Translate("LastNamePld");
            AddressPld = TextsTranslateManager.Translate("AddressPld");
            StatePld = TextsTranslateManager.Translate("StatePld");
            PostCodePld = TextsTranslateManager.Translate("PostCodePld");
            EmailPld = TextsTranslateManager.Translate("EmailPld");
            PhonePld = TextsTranslateManager.Translate("PhonePld");
            CityPld = TextsTranslateManager.Translate("CityPld");
        }
        private void InitErrMessages()
        {
            string requiredMessage = "{0} is a required field";

            FirstNameErrMessage = string.Format(requiredMessage, "First name");
            LastNameErrMessage = string.Format(requiredMessage, "Last name");
            AddressErrMessage = string.Format(requiredMessage, "Address");
            CityErrMessage = string.Format(requiredMessage, "City");
            PhoneErrMessage = string.Format(requiredMessage, "Phone");
            EmailErrMessage = string.Format(requiredMessage, "Email");
            CountryErrMessage = string.Format(requiredMessage, "Country");
        }

        private async Task InitData()
        {
            await SetBusyAsync(async ()=>
            {
                await Task.Delay(200);

                var countries = await _wooCommerceService.GetCountries();
                if(countries != null)
                {
                    Countries = new ObservableCollection<CountryModel>(countries);
                    //ShippingCountries = new ObservableCollection<CountryModel>(countries);

                    if (CustomerInfo.Billing != null && !string.IsNullOrEmpty(CustomerInfo.Billing.Country))
                        SelectedCountry = countries.FirstOrDefault(x => x.Code == CustomerInfo.Billing.Country);

                    //if (CustomerInfo.Shipping != null && !string.IsNullOrEmpty(CustomerInfo.Shipping.Country))
                    //    SelectedCountryShipping = countries.FirstOrDefault(x => x.Code == CustomerInfo.Shipping.Country);
                }
            });
        }
        internal Func<CustomerModel, bool, Task> CallBack { get; set; }

        public ICommand OkCommand => new Command(async() =>
        {
            var result = Validate();
            if (!result)
                return;

            CustomerInfo.Billing.Country = SelectedCountry.Code;
            //CustomerInfo.Shipping.Country = SelectedCountryShipping.Code;

            CallBack?.Invoke(CustomerInfo, UseDifferentAddress);
            await PopupNavigation.Instance.PopAsync();
        });

        public ICommand CancelCommand => new Command(async () =>
        {
            await PopupNavigation.Instance.PopAsync();
        });

        /*
         * First name is a required field.
         * Last name is a required field.
         * Street address is a required field.
         * Town / City is a required field.
         * Phone is a required field.
         * Email address is a required field.
         * Country is a required field.
         */
        private bool Validate()
        {
            FirstNameHasError = string.IsNullOrEmpty(CustomerInfo.Billing.FirstName);
            LastNameHasError = string.IsNullOrEmpty(CustomerInfo.Billing.LastName);
            AddressHasError = string.IsNullOrEmpty(CustomerInfo.Billing.Address1);
            CityHasError = string.IsNullOrEmpty(CustomerInfo.Billing.City);
            PhoneHasError = string.IsNullOrEmpty(CustomerInfo.Billing.Phone);
            EmailHasError = string.IsNullOrEmpty(CustomerInfo.Billing.Email);
            CountryHasError = SelectedCountry == null;

            bool billingInfoInvalid = FirstNameHasError || LastNameHasError || AddressHasError || CityHasError || PhoneHasError || CountryHasError || EmailHasError;

            bool shippingInfoInvalid = false;
            if (UseDifferentAddress)
            {
                ShippingFirstNameHasError = string.IsNullOrEmpty(CustomerInfo.Shipping.FirstName);
                ShippingLastNameHasError = string.IsNullOrEmpty(CustomerInfo.Shipping.LastName);
                ShippingAddressHasError = string.IsNullOrEmpty(CustomerInfo.Shipping.Address1);
                ShippingCityHasError = string.IsNullOrEmpty(CustomerInfo.Shipping.City);

                shippingInfoInvalid = ShippingFirstNameHasError || ShippingLastNameHasError || ShippingAddressHasError || ShippingCityHasError;
            }
            if (billingInfoInvalid || shippingInfoInvalid)
            {
                return false;
            }
            return true;
        }

        private CustomerModel _customerInfo;
        public CustomerModel CustomerInfo
        {
            get { return _customerInfo; }
            set { SetProperty(ref _customerInfo, value); }
        }

        private CountryModel _selectedCountry;
        public CountryModel SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetProperty(ref _selectedCountry, value); }
        }

        //private CountryModel _selectedCountryShipping;
        //public CountryModel SelectedCountryShipping
        //{
        //    get { return _selectedCountryShipping; }
        //    set { SetProperty(ref _selectedCountryShipping, value); }
        //}

        private ObservableCollection<CountryModel> _countries;
        public ObservableCollection<CountryModel> Countries
        {
            get { return _countries; }
            set { SetProperty(ref _countries, value); }
        }

        //private ObservableCollection<CountryModel> _shippingCountries;
        //public ObservableCollection<CountryModel> ShippingCountries
        //{
        //    get { return _shippingCountries; }
        //    set { SetProperty(ref _shippingCountries, value); }
        //}

        private bool _useDifferentAddress;
        public bool UseDifferentAddress
        {
            get { return _useDifferentAddress; }
            set { SetProperty(ref _useDifferentAddress, value); }
        }

        /// <summary>
        /// Entry Placeholder Translation Property 
        /// </summary>
        private string _firstNamePld;
        public string FirstNamePld
        {
            get { return _firstNamePld; }
            set { SetProperty(ref _firstNamePld, value); }
        }
        private string _lastNamePld;
        public string LastNamePld
        {
            get { return _lastNamePld; }
            set { SetProperty(ref _lastNamePld, value); }
        }
        private string _addressPld;
        public string AddressPld
        {
            get { return _addressPld; }
            set { SetProperty(ref _addressPld, value); }
        }
        private string _statePld;
        public string StatePld
        {
            get { return _statePld; }
            set { SetProperty(ref _statePld, value); }
        }
        private string _postCodePld;
        public string PostCodePld
        {
            get { return _postCodePld; }
            set { SetProperty(ref _postCodePld, value); }
        }
        private string _cityPld;
        public string CityPld
        {
            get { return _cityPld; }
            set { SetProperty(ref _cityPld, value); }
        }
        private string _emailPld;
        public string EmailPld
        {
            get { return _emailPld; }
            set { SetProperty(ref _emailPld, value); }
        }
        private string _phonelPld;
        public string PhonePld
        {
            get { return _phonelPld; }
            set { SetProperty(ref _phonelPld, value); }
        }

        private bool _firstNameHasError;
        public bool FirstNameHasError
        {
            get { return _firstNameHasError; }
            set { SetProperty(ref _firstNameHasError, value); }
        }

        private string _firstNameErrMessage;
        public string FirstNameErrMessage
        {
            get { return _firstNameErrMessage; }
            set { SetProperty(ref _firstNameErrMessage, value); }
        }

        private bool _lastNameHasError;
        public bool LastNameHasError
        {
            get { return _lastNameHasError; }
            set { SetProperty(ref _lastNameHasError, value); }
        }

        private string _lastNameErrMessage;
        public string LastNameErrMessage
        {
            get { return _lastNameErrMessage; }
            set { SetProperty(ref _lastNameErrMessage, value); }
        }

        private bool _addressHasError;
        public bool AddressHasError
        {
            get { return _addressHasError; }
            set { SetProperty(ref _addressHasError, value); }
        }

        private string _addressErrMessage;
        public string AddressErrMessage
        {
            get { return _addressErrMessage; }
            set { SetProperty(ref _addressErrMessage, value); }
        }

        private bool _cityHasError;
        public bool CityHasError
        {
            get { return _cityHasError; }
            set { SetProperty(ref _cityHasError, value); }
        }

        private string _cityErrMessage;
        public string CityErrMessage
        {
            get { return _cityErrMessage; }
            set { SetProperty(ref _cityErrMessage, value); }
        }

        private bool _stateHasError;
        public bool StateHasError
        {
            get { return _stateHasError; }
            set { SetProperty(ref _stateHasError, value); }
        }

        private string _stateErrMessage;
        public string StateErrMessage
        {
            get { return _stateErrMessage; }
            set { SetProperty(ref _stateErrMessage, value); }
        }

        private bool _postCodeHasError;
        public bool PostCodeHasError
        {
            get { return _postCodeHasError; }
            set { SetProperty(ref _postCodeHasError, value); }
        }

        private string _postCodeErrMessage;
        public string PostCodeErrMessage
        {
            get { return _postCodeErrMessage; }
            set { SetProperty(ref _postCodeErrMessage, value); }
        }

        private bool _emailHasError;
        public bool EmailHasError
        {
            get { return _emailHasError; }
            set { SetProperty(ref _emailHasError, value); }
        }

        private string _emailErrMessage;
        public string EmailErrMessage
        {
            get { return _emailErrMessage; }
            set { SetProperty(ref _emailErrMessage, value); }
        }

        private bool _phoneHasError;
        public bool PhoneHasError
        {
            get { return _phoneHasError; }
            set { SetProperty(ref _phoneHasError, value); }
        }

        private string _phoneErrMessage;
        public string PhoneErrMessage
        {
            get { return _phoneErrMessage; }
            set { SetProperty(ref _phoneErrMessage, value); }
        }

        private bool _countryHasError;
        public bool CountryHasError
        {
            get { return _countryHasError; }
            set { SetProperty(ref _countryHasError, value); }
        }

        private string _countryErrMessage;
        public string CountryErrMessage
        {
            get { return _countryErrMessage; }
            set { SetProperty(ref _countryErrMessage, value); }
        }

        private bool _shippingFirstNameHasError;
        public bool ShippingFirstNameHasError
        {
            get { return _shippingFirstNameHasError; }
            set { SetProperty(ref _shippingFirstNameHasError, value); }
        }

        private bool _shippingLastNameHasError;
        public bool ShippingLastNameHasError
        {
            get { return _shippingLastNameHasError; }
            set { SetProperty(ref _shippingLastNameHasError, value); }
        }

        private bool _shippingAddressHasError;
        public bool ShippingAddressHasError
        {
            get { return _shippingAddressHasError; }
            set { SetProperty(ref _shippingAddressHasError, value); }
        }

        private bool _shippingCityHasError;
        public bool ShippingCityHasError
        {
            get { return _shippingCityHasError; }
            set { SetProperty(ref _shippingCityHasError, value); }
        }
    }
}
