using System.Linq;
using System.Threading.Tasks;
using WPSTORE.Helpers;
using WPSTORE.Models.Categories;
using WPSTORE.ViewModels;
using Xamarin.Forms;

namespace WPSTORE.Views
{
    public partial class CategoriesPage : ContentPage
    {
        private CategoriesViewModel _context => (CategoriesViewModel)BindingContext;
        public CategoriesPage()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await _context.LoadCategories();
            });
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() != null)
            {
                var category = e.CurrentSelection.FirstOrDefault() as Category;
                await _context.GotoPostListPage(AppHelpers.GetCategoryId(category.slug));
            }
        }

        private async void categoriesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var category = e.Item as Category;
                await _context.GotoPostListPage(AppHelpers.GetCategoryId(category.slug));
            }
        }
        private void CategorySearchHandler_OnSearchTextChange(object sender, string e)
        {
            CategorySearchHandler.ItemsSource = _context.SearchCategories(e);
        }

        private async void CategorySearchHandler_OnSearchItemSelected(object sender, object e)
        {
            var selectedCategory = (Category)e;
            await _context.GotoPostListPage(selectedCategory.id);
        }
    }
}
