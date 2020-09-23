using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using WPSTORE.Helpers;
using WPSTORE.Models.Posts;
using WPSTORE.Models.Users;
using WPSTORE.Services.Interfaces;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    [QueryProperty("PostParam", "postParam")]
    public class PostDetailViewModel : BaseViewModel
    {
        private readonly IAppService _appService;
        public PostDetailViewModel(INavigationService navigationService, IAppService AppService) : base(navigationService)
        {
            _appService = AppService;
        }

        private async Task Processing(string postParam)
        {
            await Task.Delay(300);

            await SetBusyAsync(async () => await BindData(postParam));
        }

        private async Task BindData(string postParam)
        {
            Post = QueryStringHelpers.GetData<Post>(postParam);
            if (Post._links != null && Post._links.author != null && Post._links.author.Any())
            {
                var user = QueryStringHelpers.GetData<UserInfo>(postParam);
                UserName = user.name;
                UserAvatar = user.avatar_urls?.Avatar48;
            }
        }

        private Post _post;
        public Post Post
        {
            get { return _post; }
            set { SetProperty(ref _post, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userAvatar;
        public string UserAvatar
        {
            get { return _userAvatar; }
            set { SetProperty(ref _userAvatar, value); }
        }

        public string PostParam
        {
            set
            {
                AsyncRunner.Run(Processing(value));
            }
        }
    }
}
