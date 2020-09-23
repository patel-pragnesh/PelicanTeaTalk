using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace WPSTORE.Models.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "LoginPage.UsernameRequired")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "LoginPage.PasswordRequired")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class WpLoginResponseModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("user_email")]
        public string UserEmail { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_nicename")]
        public string UserNiceName { get; set; }

        [JsonProperty("user_display_name")]
        public string DisplayName { get; set; }

        public string Avatar { get; set; }

        [JsonProperty("token_expires")]
        public float ExpireTime { get; set; }

        [JsonIgnore]
        public bool Success => UserId > 0 && !string.IsNullOrEmpty(Token);
    }  
}