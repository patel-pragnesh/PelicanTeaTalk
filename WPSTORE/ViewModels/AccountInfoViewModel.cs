using Prism.Navigation;
using System.Threading.Tasks;
using WPSTORE.Helpers;
using WPSTORE.Models;
using WPSTORE.Services.Interfaces;
using WPSTORE.Threading;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    public class AccountInfoViewModel : BaseViewModel
    {
        private IAppService _appService;
        public AccountInfoViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;

            AsyncRunner.Run(Processing());
        }

        private async Task Processing()
        {
            await Task.Delay(100);

            await SetBusyAsync(async () => await WebRequestExecuter.Execute(async () => await _appService.GetMyInfo(), async result =>
              {
                  if (result != null)
                  {
                      MyProfile = result;
                      MyProfile.Email = GlobalSettings.User.Email;

                      if(string.IsNullOrEmpty(MyProfile.FirstName))
                        MyProfile.FirstName = GlobalSettings.User.FirstName;

                      if(string.IsNullOrEmpty(MyProfile.LastName))
                          MyProfile.LastName = GlobalSettings.User.LastName;

                      ProfileImageUrl = result.AvatarUrls?.ImageUrl96;
                  }
              }));
        }

        private WpBasicProfileModel _myProfile;
        public WpBasicProfileModel MyProfile
        {
            get => _myProfile;
            set => SetProperty(ref _myProfile, value);
        }

        private string _profileImageUrl;
        public string ProfileImageUrl
        {
            get => _profileImageUrl;
            set => SetProperty(ref _profileImageUrl, value);
        }
    }
}
