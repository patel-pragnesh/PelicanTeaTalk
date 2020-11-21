using WPSTORE.Models.Authentication;
using WPSTORE.Models.Authentication.Base;
using WPSTORE.Services.Interfaces;
using System;
using System.Threading.Tasks;
using WPSTORE.Models.SocialLogin.Providers;
using WPSTORE.Models.SocialLogin;
using Xamarin.Auth;
using Xamarin.Forms;
using System.Diagnostics;

namespace WPSTORE.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestService _requestService;
        OAuth2Base oAuth2;

        //WP User
        //public bool IsAuthenticated => false;// GlobalSettings.UserInfo != null && DateTime.UtcNow <= GlobalSettings.ExpiresDate;
        public bool IsAuthenticated => GlobalSettings.UserInfo != null;

        //Social account user
        //public bool IsSocialAuthenticated => GlobalSettings.User != null;
        //public SocialUser SocialAuthenticatedUser => GlobalSettings.User;

        public AuthenticationService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        //WP Login
        public async Task<WpLoginResponseModel> LoginAsync(string username, string password)
        {
            var builder = new UriBuilder(GlobalSettings.LoginEndpoint);
            var uri = builder.ToString();
            var loginModel = new LoginModel
            {
                UserName = username,
                Password = password
            };
            try
            {
                WpLoginResponseModel loginResult = await _requestService.PostAsync<LoginModel, WpLoginResponseModel>(uri, loginModel);
                return loginResult;
                //if (loginResult != null && loginResult.Token != null)
                //{
                //    GlobalSettings.UserInfo = loginResult;
                //    GlobalSettings.AccessToken = loginResult.Token;
                //    GlobalSettings.ExpiresDate = DateTime.UtcNow.AddMilliseconds(loginResult.ExpireTime);
                //    return loginResult;
                //}
                //return loginResult;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
            }
        }

        //public async Task<BaseResponse<string>> ChangePassword(ChangePasswordViewModel model)
        //{
        //    var builder = new UriBuilder(GlobalSettings.ChangePasswordEndpoint);
        //    var uri = builder.ToString();

        //    return await _requestService.PostAsync<ChangePasswordViewModel, BaseResponse<string>>(uri, model);
        //}
        public Task<bool> Logout()
        {
            return Task.FromResult(true);
        }

        public Task LoginWithSocialAsync(SocialProvider provider)
        {
            try
            {
                oAuth2 = OAuth2ProviderFactory.CreateProvider(provider);
                var authenticator = new OAuth2Authenticator(
                    oAuth2.ClientId,
                    oAuth2.ClientSecret,
                    oAuth2.Scope,
                    oAuth2.AuthorizationUri,
                    oAuth2.RedirectUri,
                    oAuth2.RequestTokenUri,
                    null,
                    oAuth2.IsUsingNativeUI);

                App.Authenticator = authenticator;

                authenticator.Completed += async (s, e) =>
                {
                    if (e.IsAuthenticated)
                    {
                        // If the user is authenticated, request their basic user data from Google
                        // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                        var user = await oAuth2.GetUserInfoAsync(e.Account);

                        MessagingCenter.Send(user, MessagingCenterKeys.AuthenticationRequested, true);
                        Debug.WriteLine("Authentication Success");
                    }
                };
                authenticator.Error += (s, e) =>
                {
                    Debug.WriteLine("Authentication error: " + e.Message);
                };

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Login Error : " + ex.Message);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
        //public async Task<bool> UserIsAuthenticatedAndValidAsync()
        //{
        //    if (!IsAuthenticated)
        //    {
        //        return false;
        //    }
        //    else if (!SocialAuthenticatedUser.LoggedInWithSocialAccount)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        bool refreshSucceded = false;
        //        oAuth2 = OAuth2ProviderFactory.CreateProvider(SocialAuthenticatedUser.Provider);
        //        try
        //        {
        //            var utcNow = DateTime.UtcNow.AddMinutes(30);
        //            if (SocialAuthenticatedUser.ExpiresIn < utcNow)
        //            {
        //                var result = await oAuth2.RefreshTokenAsync(SocialAuthenticatedUser);
        //                if (result.IsRefresh)
        //                {
        //                    GlobalSettings.SetUserInfo(result.User);
        //                }
        //                else
        //                {
        //                    GlobalSettings.RemoveUserData();
        //                }

        //                refreshSucceded = result.IsRefresh;
        //            }
        //            else
        //            {
        //                refreshSucceded = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine($"Error with refresh attempt: {ex}");
        //        }

        //        return refreshSucceded;
        //    }
        //}

        public Task SocialLogoutAsync()
        {
            return Task.FromResult(true);
        }

        public async Task<WpLoginResponseModel> GoogleLogin(WPGoogleLoginRequest wpGoogleLoginRequest)
        {
            var builder = new UriBuilder(GlobalSettings.WpGoogleLoginEndpoint);
            var uri = builder.ToString();
            return await _requestService.PostAsync<WPGoogleLoginRequest, WpLoginResponseModel>(uri, wpGoogleLoginRequest);
        }

        public async Task<WpLoginResponseModel> FacebookLogin(WPFacebookLoginRequest wpFacebookLoginRequest)
        {
            var builder = new UriBuilder(GlobalSettings.WpFacebookLoginEndpoint);
            var uri = builder.ToString();
            return await _requestService.PostAsync<WPFacebookLoginRequest, WpLoginResponseModel>(uri, wpFacebookLoginRequest);
        }
    }
}
