using WPSTORE.Models.Posts;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostListPage : ContentPage
    {
        private PostListViewModel _context => (PostListViewModel)BindingContext;
        public PostListPage()
        {
            InitializeComponent();
        }

        private async void postsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item;
            if (selectedItem != null && selectedItem is Post post)
            {
                await _context.GotoArticleDetailPage(post);
            }
        }
    }
}