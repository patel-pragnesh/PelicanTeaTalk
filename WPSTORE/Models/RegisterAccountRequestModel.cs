using Newtonsoft.Json;

namespace WPSTORE.Models
{
    public class RegisterAccountRequestModel
    {
        [JsonProperty("user_login")]
        public string UserName { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("user_email")]
        public string Email { get; set; }

        [JsonProperty("user_pass")]
        public string Password { get; set; }
    }
}
