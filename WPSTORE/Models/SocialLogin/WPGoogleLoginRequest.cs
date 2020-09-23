using Newtonsoft.Json;

namespace WPSTORE.Models.SocialLogin
{
    public class BaseWPSocialLoginRequest
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
    public class WPGoogleLoginRequest: BaseWPSocialLoginRequest
    {
    }
    public class WPFacebookLoginRequest : BaseWPSocialLoginRequest
    {
    }

}
