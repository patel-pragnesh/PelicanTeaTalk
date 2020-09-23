using WPSTORE.Models;
using WPSTORE.Models.Categories;
using WPSTORE.Models.Posts;
using WPSTORE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Models.Users;
using WPSTORE.Extensions;

namespace WPSTORE.Services
{
    public class AppService : IAppService
    {
        private readonly IRequestService _requestService;
        //private readonly IDialogService _dialogService;

        public AppService(IRequestService requestService)
        {
            _requestService = requestService;
        }
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var builder = new UriBuilder(GlobalSettings.CategoriesEndpoint);
            var uri = builder.ToString();
            return await _requestService.GetAsync<IEnumerable<Category>>(uri);
        }

        public async Task<IEnumerable<Post>> GetAllPostsByCategory(int? categoryId, int pageSize = 20)
        {
            if (!categoryId.HasValue)
                return null;
            var builder = new UriBuilder(GlobalSettings.PostsByCategoryEndpoint);
            builder.Query = $"?categories= {categoryId}&per_page={pageSize}";
            var uri = builder.ToString();
            return await _requestService.GetAsync<IEnumerable<Post>>(uri);
        }
        public async Task<IEnumerable<Post>> GetAllPosts(int page, int pageSize)
        {
            try
            {
                var builder = new UriBuilder(GlobalSettings.PostsByCategoryEndpoint);
                builder.Query = $"?page={page}&per_page={pageSize}";
                var uri = builder.ToString();
                return await _requestService.GetAsync<IEnumerable<Post>>(uri);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<StoreInfo>> GetStoreInfo()
        {
            var builder = new UriBuilder(GlobalSettings.StoreInfoEndpoint);
            var uri = builder.ToString();
            return await _requestService.GetAsync<IEnumerable<StoreInfo>>(uri);
        }
        
       
        public async Task<UserInfo> GetStoreInfo(string userEndpoint)
        {
            var builder = new UriBuilder(userEndpoint);
            var uri = builder.ToString();
            return await _requestService.GetAsync<UserInfo>(uri);
        }
        public async Task<WpBasicProfileModel> GetMyInfo()
        {
            var builder = new UriBuilder(GlobalSettings.WpMyProfileEndpoint);
            var uri = builder.ToString();
            return await _requestService.GetAsync<WpBasicProfileModel>(uri, forcedRefresh: true);
        }
        public async Task<RegisterAccountResponseModel> RegisterAccount(RegisterAccountRequestModel model)
        {
            var builder = new UriBuilder(GlobalSettings.WpRegisterAccountEndpoint);
            var uri = builder.ToString();
            return await _requestService.PostAsync<RegisterAccountRequestModel, RegisterAccountResponseModel>(uri, model);
        }

        public async Task<IEnumerable<Food>> GetPopularItems()
        {
            var food = new List<Food>
            {

                new Food {
                   Image_Food="https://i.pinimg.com/originals/ae/a2/71/aea271c5f75be7aea8ccfacc6f95b36e.jpg",
                   Title_Food="Lomo saltado ipsum dolo sit amet",
                   Price_Food="$ 69.50",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/46/b8/79/46b8793aac8e9162fad20cd34195dbbe.jpg",
                   Title_Food="Ceviche",
                   Price_Food="$ 780.0",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                }
                };

            return food;
        }
        public async Task<IEnumerable<Food>> GetBeverageItems()
        {
            var food = new List<Food>
            {

                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/originals/ae/a2/71/aea271c5f75be7aea8ccfacc6f95b36e.jpg",
                   Title_Food="Lomo saltado ipsum dolo sit amet",
                   Price_Food="$ 69.50",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/46/b8/79/46b8793aac8e9162fad20cd34195dbbe.jpg",
                   Title_Food="Ceviche",
                   Price_Food="$ 780.0",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                }
                };

            return food;
        }
        public async Task<IEnumerable<Food>> GetFoodItems()
        {
            var food = new List<Food>
            {      
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/originals/ae/a2/71/aea271c5f75be7aea8ccfacc6f95b36e.jpg",
                   Title_Food="Lomo saltado ipsum dolo sit amet",
                   Price_Food="$ 69.50",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/46/b8/79/46b8793aac8e9162fad20cd34195dbbe.jpg",
                   Title_Food="Ceviche",
                   Price_Food="$ 780.0",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                }
                };

            return food;
        }
        public async Task<IEnumerable<Voucher>> GetVoucherItems()
        {
            var voucher = new List<Voucher>
            {
                new Voucher {
                   Icon="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title="Ap dung cho giao hang",
                   ExpiryDate="10/04/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title="Torta tres leches",
                   ExpiryDate="30/04/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/originals/ae/a2/71/aea271c5f75be7aea8ccfacc6f95b36e.jpg",
                   Title="Lomo saltado ipsum dolo sit amet",
                   ExpiryDate="10/05/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title="Torta tres leches",
                   ExpiryDate="30/04/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/originals/ae/a2/71/aea271c5f75be7aea8ccfacc6f95b36e.jpg",
                   Title="Lomo saltado ipsum dolo sit amet",
                   ExpiryDate="10/05/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title="Torta tres leches",
                   ExpiryDate="30/04/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/originals/ae/a2/71/aea271c5f75be7aea8ccfacc6f95b36e.jpg",
                   Title="Lomo saltado ipsum dolo sit amet",
                   ExpiryDate="10/05/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title="Torta tres leches",
                   ExpiryDate="30/04/2020",
                },
                new Voucher {
                   Icon="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title="Pollo a la braza",
                   ExpiryDate="1/06/2020",
                }
                };

            return voucher;
        }
        public async Task<IEnumerable<Restaurant>> GetRestaurant()
        {
            var restaurant = new List<Restaurant>
            {
                    new Restaurant {
                    Image_Restaurant ="https://i.pinimg.com/originals/cb/4b/18/cb4b18ecbab4a65e8dc4abc937064fd9.jpg",
                    Title_Restaurant="Central",
                    Place_Restaurant="Calle Santa Isabel 376, Miraflores-Perú",
                    Rating1_Restaurant="5",
                    Rating2_Restaurant="(967 ratings)"
                },
                    new Restaurant {
                    Image_Restaurant ="https://i.pinimg.com/originals/e4/b5/98/e4b5985bc5241cb7772525d0d34ef403.jpg",
                    Title_Restaurant="Maido",
                    Place_Restaurant="Calle San Martin 399, Miraflores-Perú",
                    Rating1_Restaurant="4.9",
                    Rating2_Restaurant="(678 ratings)"
                },
                    new Restaurant {
                    Image_Restaurant ="https://i.pinimg.com/originals/bd/db/31/bddb31854dfab067ea455e055bd8ffbe.jpg",
                    Title_Restaurant="Osso",
                    Place_Restaurant="Calle Tahiti 175, La Molina-Perú",
                    Rating1_Restaurant="4.9",
                    Rating2_Restaurant="(645 ratings)"
                },
                    new Restaurant {
                    Image_Restaurant ="https://i.pinimg.com/originals/aa/0d/e3/aa0de3ddf253a194ed4caad5a262b760.jpg",
                    Title_Restaurant="Rahat Brasserie",
                    Place_Restaurant="540 Levent-Besiktas",
                    Rating1_Restaurant="3.9",
                    Rating2_Restaurant="(348 ratings)"
                },
                    new Restaurant {
                    Image_Restaurant ="https://i.pinimg.com/originals/75/27/73/752773561732a6945a9b2484eef2f87f.jpg",
                    Title_Restaurant="Garage Bar",
                    Place_Restaurant="124 Levent-Besiktas",
                    Rating1_Restaurant="4.1",
                    Rating2_Restaurant="(545 ratings)"
                },

                };

            return restaurant;
        }
        public async Task<IEnumerable<Transactions>> GetTransactions()
        {
            var randomNum = new Random(0123456789);
            var transactions = new List<Transactions>
            {
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/cb/4b/18/cb4b18ecbab4a65e8dc4abc937064fd9.jpg",
                    OrderID="#111032",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="5"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/e4/b5/98/e4b5985bc5241cb7772525d0d34ef403.jpg",
                    OrderID="#111040",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="15"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/bd/db/31/bddb31854dfab067ea455e055bd8ffbe.jpg",
                    OrderID="#111099",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="25"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/aa/0d/e3/aa0de3ddf253a194ed4caad5a262b760.jpg",
                    OrderID="#111001",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="35"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/75/27/73/752773561732a6945a9b2484eef2f87f.jpg",
                    OrderID="#111020",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="45"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/e4/b5/98/e4b5985bc5241cb7772525d0d34ef403.jpg",
                    OrderID="#111040",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="15"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/bd/db/31/bddb31854dfab067ea455e055bd8ffbe.jpg",
                    OrderID="#111099",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="25"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/aa/0d/e3/aa0de3ddf253a194ed4caad5a262b760.jpg",
                    OrderID="#111001",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="35"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/75/27/73/752773561732a6945a9b2484eef2f87f.jpg",
                    OrderID="#111020",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="45"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/e4/b5/98/e4b5985bc5241cb7772525d0d34ef403.jpg",
                    OrderID="#111040",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="15"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/bd/db/31/bddb31854dfab067ea455e055bd8ffbe.jpg",
                    OrderID="#111099",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="25"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/aa/0d/e3/aa0de3ddf253a194ed4caad5a262b760.jpg",
                    OrderID="#111001",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="35"
                },
                    new Transactions {
                    Image ="https://i.pinimg.com/originals/75/27/73/752773561732a6945a9b2484eef2f87f.jpg",
                    OrderID="#111020",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointEarned="45"
                },
                };

            return transactions;
        }
        public async Task<IEnumerable<UsedVouchers>> GetUsedVouchers()
        {
            var randomNum = new Random(0123456789);
            var usedVoucher = new List<UsedVouchers>
            {
                    new UsedVouchers {
                    Title="01 Cherry Macchiato with 600 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="600"
                },
                    new UsedVouchers {
                    Title="01 Cherry Macchiato with 500 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="500"
                },
                    new UsedVouchers {
                    Title="01 Cold Coffee with 300 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="300"
                },
                    new UsedVouchers {
                    Title="01 Cold Brew with 400 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="400"
                },
                    new UsedVouchers {
                    Title="01 Cherry Macchiato with 600 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="600"
                },
                    new UsedVouchers {
                    Title="01 Cherry Macchiato with 600 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="600"
                },
                    new UsedVouchers {
                    Title="01 Cherry Macchiato with 600 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="600"
                },
                    new UsedVouchers {
                    Title="01 Cherry Macchiato with 600 Points",
                    Date = DateTime.Now.AddDays(randomNum.Next(-1000, 0)),
                    PointRedempt="600"
                },     
                };

            return usedVoucher;
        }
        public async Task<IEnumerable<ExchangeVoucher>> GetExchangeVouchers()
        {
            var exchangeVoucher = new List<ExchangeVoucher>
            {
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/cb/4b/18/cb4b18ecbab4a65e8dc4abc937064fd9.jpg",
                    Title="01 Cherry Macchiato with 600 Points",
                    Provider="The Coffee Store",
                    Point="600"
                },
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/75/27/73/752773561732a6945a9b2484eef2f87f.jpg",
                    Title="01 Cherry Macchiato with 500 Points",
                    Provider="The Barista",
                    Point="500"
                },
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/aa/0d/e3/aa0de3ddf253a194ed4caad5a262b760.jpg",
                    Title="01 Cold Coffee with 300 Points",
                    Provider="The Coffee Store",
                    Point="300"
                },
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/bd/db/31/bddb31854dfab067ea455e055bd8ffbe.jpg",
                    Title="01 Cold Brew with 400 Points",
                    Provider="Corretto Coffee",
                    Point="400"
                },
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/e4/b5/98/e4b5985bc5241cb7772525d0d34ef403.jpg",
                    Title="01 Cherry Macchiato with 600 Points",
                    Provider="The Coffee Store",
                    Point="600"
                },
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/75/27/73/752773561732a6945a9b2484eef2f87f.jpg",
                    Title="01 Cherry Macchiato with 600 Points",
                    Provider="The Fashionita",
                    Point="600"
                },
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/aa/0d/e3/aa0de3ddf253a194ed4caad5a262b760.jpg",
                    Title="01 Cherry Macchiato with 600 Points",
                    Provider="The Coffee Store",
                    Point="600"
                },
                    new ExchangeVoucher {
                    Image ="https://i.pinimg.com/originals/bd/db/31/bddb31854dfab067ea455e055bd8ffbe.jpg",
                    Title="01 Cherry Macchiato with 600 Points",
                    Provider="Outsider Club",
                    Point="600"
                },
                };

            return exchangeVoucher;
        }

        public async Task<NotificationResponseModel> GetArchivedNotifications()
        {
            var builder = new UriBuilder($"{GlobalSettings.BaseNotificationEnpoint}/get_archive");
            var requestParams = new
            {
                auth_key = GlobalSettings.AuthKey
            }.GetQueryString();
            builder.Query = requestParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<NotificationResponseModel>(uri);
        }
        public async Task<BaseNotificationResponseModel> AddOrUpdateDeviceSubscription(string deviceToken, string deviceType, string info)
        {
            var builder = new UriBuilder($"{GlobalSettings.BaseNotificationEnpoint}/savetoken");
            var requestParams = new
            {
                auth_key = GlobalSettings.AuthKey,
                device_token = deviceToken,
                device_type = deviceType,
                device_info = info
            }.GetQueryString();
            builder.Query = requestParams;
            var uri = builder.ToString();
            return await _requestService.GetAsync<BaseNotificationResponseModel>(uri);
        }
    }
}
