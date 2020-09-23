using Newtonsoft.Json;

namespace WPSTORE.Models.SocialLogin.Providers
{

    public class GoogleUser
    {
        public string Sub { get; set; }
        public string Name { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        public string Picture { get; set; }
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool Verified { get; set; }

        public string Locale { get; set; }
    }
}
