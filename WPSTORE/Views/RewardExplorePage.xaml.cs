using System.Threading.Tasks;
using WPSTORE.Controls;
using WPSTORE.Models.Posts;
using WPSTORE.ViewModels;
using Xamarin.Forms;

namespace WPSTORE.Views
{
    public partial class RewardExplorePage : ContentPage
    {
        private RewardExploreViewModel _context => (RewardExploreViewModel)BindingContext;
        public RewardExplorePage()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await _context.LoadExchangeRewards();
            });
        }

        private async void listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var post = e.Item as Post;
                await _context.GotoDetailPage(post);
            }
        }

        private void WpSearchHandler_OnSearchTextChange(object sender, string e)
        {
            WpSearchHandler control = sender as WpSearchHandler;
            control.ItemsSource = _context.SearchPosts(e);
        }

        private async void WpSearchHandler_OnSearchItemSelected(object sender, object e)
        {
            var post = e as Post;
            await _context.GotoDetailPage(post);
        }
    }
}
