using WPSTORE.Models;
using WPSTORE.Models.Categories;
using WPSTORE.Models.Posts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Models.Users;

namespace WPSTORE.Services.Interfaces
{
    public interface IAppService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<IEnumerable<Post>> GetAllPosts(int page, int pageSize);
        Task<IEnumerable<Post>> GetAllPostsByCategory(int? categoryId, int pageSize = 20);
        Task<IEnumerable<StoreInfo>> GetStoreInfo();
        Task<WpBasicProfileModel> GetMyInfo();
        Task<RegisterAccountResponseModel> RegisterAccount(RegisterAccountRequestModel model);
        Task<UserInfo> GetStoreInfo(string userEndpoint);
        Task<IEnumerable<Food>> GetPopularItems();
        Task<IEnumerable<Food>> GetBeverageItems();
        Task<IEnumerable<Food>> GetFoodItems();
        Task<IEnumerable<Voucher>> GetVoucherItems();
        Task<IEnumerable<Restaurant>> GetRestaurant();
        Task<IEnumerable<Transactions>> GetTransactions();
        Task<IEnumerable<UsedVouchers>> GetUsedVouchers();
        Task<IEnumerable<ExchangeVoucher>> GetExchangeVouchers();

        Task<NotificationResponseModel> GetArchivedNotifications();
        Task<BaseNotificationResponseModel> AddOrUpdateDeviceSubscription(string deviceToken, string deviceType, string info);
    }
}
