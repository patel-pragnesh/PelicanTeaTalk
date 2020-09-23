using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

namespace WPSTORE.Models.SocialLogin.Providers
{
    public class GoogleOAuth2 : OAuth2Base
    {
        private static readonly Lazy<GoogleOAuth2> lazy = new Lazy<GoogleOAuth2>(() => new GoogleOAuth2());

        public static GoogleOAuth2 Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private GoogleOAuth2()
        {
            Initialize();
        }
        public static string GetClientId()
        {
            switch(Device.RuntimePlatform)
            {
                case Device.Android:
                    return GlobalSettings.AndroidClientId;
                case Device.iOS:
                    return GlobalSettings.iOSClientId;
                default:
                    return "";
            }
        }
        public static string GetRedirectUri()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    return GlobalSettings.AndroidUrlSchema;
                case Device.iOS:
                    return "com.tls.xstore";// GlobalSettings.iOSUrlSchema;
                default:
                    return "";
            }
        }

        //Doc: https://developers.google.com/identity/protocols/oauth2/native-app
        // https://console.developers.google.com/apis/dashboard?project=storeapp-275107
        
        void Initialize()
        {
            IsUsingNativeUI = true;
            ProviderName = "Google";
            Description = "Google Login Provider";
            Provider = SocialProvider.Google;
            ClientId = GetClientId();// "877130854950-fjai37crgsccjmbp8nbhlh49e4tuhsc5.apps.googleusercontent.com";
            ClientSecret = null;
            Scope = "email profile";
            AuthorizationUri = new Uri("https://accounts.google.com/o/oauth2/v2/auth");
            RequestTokenUri = new Uri("https://oauth2.googleapis.com/token");

            var redirectUri = GetRedirectUri();
            RedirectUri = new Uri($"{redirectUri}:/oauth2redirect");
            UserInfoUri = new Uri("https://www.googleapis.com/oauth2/v3/userinfo"); //access_token=...&client_secret=...&redirect_uri=...&client_id=...
        }

        #region Implement Abstract Method
        public override async Task<SocialUser> GetUserInfoAsync(Account account)
        {
            SocialUser user = null;
            string token = account.Properties["access_token"];
            string refreshToke = account.Properties["refresh_token"];
            int expriesIn;
            int.TryParse(account.Properties["expires_in"], out expriesIn);


            var request = new OAuth2Request("GET", UserInfoUri, null, account);
            var response = await request.GetResponseAsync();
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string userJson = await response.GetResponseTextAsync();
                var googleUser = JsonConvert.DeserializeObject<GoogleUser>(userJson);
                user = new SocialUser
                {
                    SocialId = googleUser.Sub,
                    Token = token,
                    RefreshToken = null,
                    Name = googleUser.Name,
                    Email = googleUser.Email,
                    ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(expriesIn)),
                    PictureUrl = googleUser.Picture,
                    Provider = SocialProvider.Google,
                    LoggedInWithSocialAccount = true,
                };
            }
            return user;
        }

        public override async Task<(bool IsRefresh, SocialUser User)> RefreshTokenAsync(SocialUser user)
        {
            bool refreshSuccess = false;
            if (user == null)
            {
                return (refreshSuccess, user);
            }

            //https://graph.facebook.com/oauth/client_code?access_token=...&client_secret=...&redirect_uri=...&client_id=...
            //-> return {code : ""}
            string code = null;
            Uri codeUri = new Uri($"https://graph.facebook.com/oauth/client_code?access_token={user.Token}&client_secret={ClientSecret}&redirect_uri={RedirectUri.AbsoluteUri}&client_id={ClientId}");

            var request = new Request("POST", codeUri, null, null);
            var response = await request.GetResponseAsync();
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string tokenString = await response.GetResponseTextAsync();
                JObject jwtDynamic = JsonConvert.DeserializeObject<JObject>(tokenString);
                code = jwtDynamic.Value<string>("code");
            }

            if (!string.IsNullOrEmpty(code))
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string> { { "code", code }, { "client_id", ClientId }, { "redirect_uri", RedirectUri.AbsoluteUri } };
                var refreshRequest = new Request("POST", RequestTokenUri, dictionary, null);
                var refreshResponse = await refreshRequest.GetResponseAsync();
                if (refreshResponse != null && refreshResponse.StatusCode == HttpStatusCode.OK)
                {
                    string tokenString = await refreshResponse.GetResponseTextAsync();
                    JObject jwtDynamic = JsonConvert.DeserializeObject<JObject>(tokenString);
                    var accessToken = jwtDynamic.Value<string>("access_token");
                    var expiresIn = jwtDynamic.Value<int>("expires_in");


                    user.Token = accessToken;
                    user.ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(0, 0, expiresIn));

                    refreshSuccess = true;
                }
            }
            //machine_id <- option
            //https://graph.facebook.com/oauth/access_token?code=...&client_id=...&redirect_uri=...&machine_id= ...
            //-> return {"access_token":"...", "expires_in":..., "machine_id":"..."}
            return (refreshSuccess, user);
        }
        #endregion
    }
}
