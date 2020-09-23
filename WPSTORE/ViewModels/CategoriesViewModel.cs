using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Helpers;
using WPSTORE.Models.Categories;
using WPSTORE.Models.Posts;
using WPSTORE.Services.Interfaces;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        private IEnumerable<Category> _categories;
        private IAppService _appService;
        public CategoriesViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;
        }
        public async Task LoadCategories()
        {
            IsBusy = true;
            var categories = await _appService.GetAllCategories();
            if (categories != null)
            {
                //GlobalSettings.WpCategories = categories.ToList();
                Categories = categories.ToList();
            }
            IsBusy = false;
        }
        public IEnumerable<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        public List<Category> SearchCategories(string searchText)
        {
            var categories = Categories?.Where(x => (x.name ?? "").ToLower().Contains(searchText?.ToLower())).ToList();
            return categories;
        }

        public async Task GotoPostListPage(int? wpCategoryId)
        {
            string categoryName = AppHelpers.GetCategoryName(wpCategoryId);
            await NavigationHelpers.GotoPageAsync(nameof(PostListPage), new Dictionary<string, object> {
                { "WpCatId", wpCategoryId },
                { "WpCategoryName", categoryName }
            });
        }
    }
}
