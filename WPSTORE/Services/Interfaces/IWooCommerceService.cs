using System.Collections.Generic;
using System.Threading.Tasks;
using WPSTORE.Models.WooCommerce;
using WPSTORE.Models.WooCommerce.Requests;

namespace WPSTORE.Services.Interfaces
{
    public interface IWooCommerceService
    {
        Task<List<CategoryModel>> GetCategories();
        Task<List<ProductModel>> GetAllProducts(int pageIndex, int pageSize = GlobalSettings.AppConst.PageSize);
        Task<List<ProductModel>> GetProductsByCategoryId(string categoryId, int pageIndex, int pageSize = GlobalSettings.AppConst.PageSize);
        Task<List<PaymentMethodModel>> GetPaymentMethods();
        Task<OrderModel> CreateOrder(NewOrderRequest orderRequest);
        Task<List<CurrencyModel>> GetAllCurrency();
        Task<CurrencyModel> GetCurrentCurrency();
        Task<List<OrderModel>> GetOrders(OrderHistoryRequest orderHistoryRequest);
        Task<List<CountryModel>> GetCountries();
        Task<List<CouponModel>> GetCoupons(CouponListRequest couponListRequest);
        Task<List<TaxRatesModel>> GetTaxRates(TaxRatesListRequest taxRatesListRequest);
        Task<CustomerModel> GetCustomerInfo(int customerId);
    }
}
