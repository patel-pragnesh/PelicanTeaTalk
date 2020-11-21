using WPSTORE.Models.Authentication;
using WPSTORE.Models.Authentication.Base;
using System.Threading.Tasks;
using WPSTORE.Models.SocialLogin;

namespace WPSTORE.Services.Interfaces
{
    public interface IAuthenticationService
    {
        //Wordpress
        bool IsAuthenticated { get; }

        //Social
        //bool IsSocialAuthenticated { get; }
        //SocialUser SocialAuthenticatedUser { get; }

        //Wordpress
        Task<WpLoginResponseModel> LoginAsync(string userName, string password);
        //Task<BaseResponse<string>> ChangePassword(ChangePasswordViewModel model);
        Task<bool> Logout();


        //Social
        Task LoginWithSocialAsync(SocialProvider provider);
        //Task<bool> UserIsAuthenticatedAndValidAsync();
        Task SocialLogoutAsync();

        Task<WpLoginResponseModel> GoogleLogin(WPGoogleLoginRequest wpGoogleLoginRequest);
        Task<WpLoginResponseModel> FacebookLogin(WPFacebookLoginRequest wpFacebookLoginRequest);
    }
}
