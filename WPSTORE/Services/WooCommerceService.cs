using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPSTORE.Extensions;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Models.WooCommerce.Requests;
using WPSTORE.Services.Interfaces;

namespace WPSTORE.Services
{
    public class WooCommerceService : IWooCommerceService
    {
        private readonly IRequestService _requestService;

        public WooCommerceService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceCategoryEndpoint);
            var queryParams = new BaseRequest().GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<CategoryModel>>(uri);
        }

        //Get All Products
        public async Task<List<ProductModel>> GetAllProducts(int pageIndex, int pageSize = GlobalSettings.AppConst.PageSize)
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceAllProductEndpoint);
            var request = new AllProductsRequest()
            {
                Page = pageIndex,
                PageSize = pageSize
            };
            var queryParams = request.GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<ProductModel>>(uri);
        }

        public async Task<List<ProductModel>> GetProductsByCategoryId(string categoryId, int pageIndex, int pageSize = GlobalSettings.AppConst.PageSize)
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceProductByCategoryEndpoint);
            var request = new ProductByCategoryRequest()
            {
                Category = categoryId,
                Page = pageIndex,
                PageSize = pageSize
            };
            var queryParams = request.GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<ProductModel>>(uri);
        }

        public async Task<List<PaymentMethodModel>> GetPaymentMethods()
        {
            var builder = new UriBuilder(GlobalSettings.WooCommercePaymentMethodEndpoint);
            var queryParams = new BaseRequest().GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<PaymentMethodModel>>(uri);
        }

        public async Task<OrderModel> CreateOrder(NewOrderRequest orderRequest)
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceOrderEndpoint);
            builder.Query = orderRequest.GetQueryString();
            var uri = builder.ToString();
            return await _requestService.PostAsync<NewOrderRequest, OrderModel>(uri, orderRequest);
        }
        public async Task<List<CurrencyModel>> GetAllCurrency()
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceAllCurrencyEndpoint);
            var queryParams = new BaseRequest().GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<CurrencyModel>>(uri);
        }
        public async Task<CurrencyModel> GetCurrentCurrency()
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceCurrentCurrencyEndpoint);
            var queryParams = new BaseRequest().GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<CurrencyModel>(uri);
        }

        public async Task<List<OrderModel>> GetOrders(OrderHistoryRequest orderHistoryRequest)
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceOrderEndpoint);
            var queryParams = orderHistoryRequest.GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<OrderModel>>(uri, forcedRefresh: true);
        }

        public async Task<List<CouponModel>> GetCoupons(CouponListRequest couponListRequest)
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceCouponEndpoint);
            var queryParams = couponListRequest.GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<CouponModel>>(uri, forcedRefresh: true);
        }

        public async Task<List<TaxRatesModel>> GetTaxRates(TaxRatesListRequest taxRatesListRequest)
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceTaxRatesListRequestEndpoint);
            var queryParams = taxRatesListRequest.GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<TaxRatesModel>>(uri, forcedRefresh: true);
        }

        public async Task<List<CountryModel>> GetCountries()
        {
            var builder = new UriBuilder(GlobalSettings.WooCommerceCountryEndpoint);
            var queryParams = new BaseRequest().GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<List<CountryModel>>(uri);
        }

        public async Task<CustomerModel> GetCustomerInfo(int customerId)
        {
            var url = $"{GlobalSettings.WooCommerceCustomerEndpoint}/{customerId}";
            var builder = new UriBuilder(url);
            var queryParams = new BaseRequest().GetQueryString();
            builder.Query = queryParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<CustomerModel>(uri);
        }
    }
}
