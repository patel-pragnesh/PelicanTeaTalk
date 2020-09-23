using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPSTORE.Helpers;
using WPSTORE.Models.Categories;
using WPSTORE.Models.Posts;
using WPSTORE.Services.Interfaces;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class ExploringPostViewModel : BaseViewModel
    {
        private ObservableCollection<Post> _posts;
        private IEnumerable<Category> _categories;
        private IAppService _appService;
        private const int PageLimit = 20;
        public int CurrentPage { get; set; }

        public ExploringPostViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;

            ListViewItemAppearingCommand = new Command<object>(ListViewItemAppearing);
            ListViewRefreshCommand = new Command(LoadPosts);
            LoadPosts();
        }
        public async Task LoadCategories()
        {
            var categories = await _appService.GetAllCategories();
            if (categories != null)
            {
                GlobalSettings.WpCategories = categories.ToList();
                Categories = categories.ToList();
            }
        }
        public void LoadPosts()
        {
            IsBusy = true;
            CurrentPage = 1;
            Task.Run(async () =>
            {
                //Get all posts
                var posts = await _appService.GetAllPosts(CurrentPage, PageLimit);
                if (posts != null)
                {
                    Posts = new ObservableCollection<Post>(posts.ToList());
                }
                IsBusy = false;
            });
        }
        private async Task LoadMorePosts()
        {
            IsBusy = true;
            CurrentPage += 1;
            var posts = await _appService.GetAllPosts(CurrentPage, PageLimit);
            if (posts != null)
            {
                foreach(var item in posts)
                {
                    Posts.Add(item);
                }
            }
            else
            {
                CurrentPage -= 1;
            }
            IsBusy = false;
        }
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set => SetProperty(ref _posts, value);
        }
        public IEnumerable<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        public async Task GotoDetailPage(Post posts)
        {
            await NavigationHelpers.GotoPageAsync(nameof(PostDetailPage),
                new Dictionary<string, object> { { "postParam", QueryStringHelpers.GetQueryParam(posts) } });
        }

        public List<Post> SearchPosts(string searchText)
        {
            return Posts?.Where(x => (x.title.rendered ?? "").ToLower().Contains(searchText?.ToLower())).ToList();
        }
        public ICommand ListViewItemAppearingCommand { get; private set; }
        public ICommand ListViewRefreshCommand { get; private set; }


        async void ListViewItemAppearing(object e)
        {
            var s = e as ItemVisibilityEventArgs;
            if (s.ItemIndex + 1 == Posts.Count())
            {
                await LoadMorePosts();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
