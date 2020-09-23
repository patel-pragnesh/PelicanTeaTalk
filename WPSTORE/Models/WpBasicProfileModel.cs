using Newtonsoft.Json;
using Prism.Mvvm;

namespace WPSTORE.Models
{
    public class WpBasicProfileModel : BindableBase
    {
        public int Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        private string _firstName;
        [JsonProperty("first_name")]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        [JsonProperty("last_name")]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Slug { get; set; }

        [JsonProperty("avatar_urls")]
        public AvatarUrls AvatarUrls { get; set; }
    }
    public class AvatarUrls
    {
        [JsonProperty("24")]
        public string ImageUrl24 { get; set; }

        [JsonProperty("48")]
        public string ImageUrl48 { get; set; }

        [JsonProperty("96")]
        public string ImageUrl96 { get; set; }
    }
}
