using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPSTORE.Helpers;
using WPSTORE.Models.Params;
using WPSTORE.Models.Posts;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using WPSTORE.Views;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    [QueryProperty("PostListParam", "postListParam")]
    public class PostListViewModel : BaseViewModel
    {
        private IAppService _appService;

        public PostListViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;
        }

        private async Task Processing(string postListParam)
        {
            await Task.Delay(300);

            var postListParamRequest = QueryStringHelpers.GetData<PostListParamModel>(postListParam);

            PageTitle = postListParamRequest?.WpCategoryName;

            await SetBusyAsync(async () => await WebRequestExecuter.Execute(async () => await _appService.GetAllPostsByCategory(postListParamRequest.WpCategoryId), async articles =>
            {
                if (articles != null)
                {
                    Articles = new ObservableCollection<Post>(articles);
                }
            }));
        }

        public async Task GotoArticleDetailPage(Post posts)
        {
            await NavigationHelpers.GotoPageAsync(nameof(PostDetailPage),
                new Dictionary<string, object> { {"postParam", QueryStringHelpers.GetQueryParam(posts) } });
        }

        public string PostListParam
        {
            set
            {
                AsyncRunner.Run(Processing(value));
            }
        }

        private ObservableCollection<Post> _articles;
        public ObservableCollection<Post> Articles
        {
            get => _articles;
            set => SetProperty(ref _articles, value);
        }
    }
}
