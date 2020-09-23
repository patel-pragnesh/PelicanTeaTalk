using System;
namespace WPSTORE.Models.SocialLogin.Providers
{
    public class OAuth2ProviderFactory
    {
        public static OAuth2Base CreateProvider(SocialProvider provider)
        {
            OAuth2Base oAuth2 = null;

            switch (provider)
            {
                case SocialProvider.Google:
                    oAuth2 = GoogleOAuth2.Instance;
                    break;
                case SocialProvider.Line:
                    oAuth2 = LineOAuth2.Instance;
                    break;
                case SocialProvider.Facebook:
                    oAuth2 = FacebookOAuth2.Instance;
                    break;
            }

            return oAuth2;
        }
    }
}
