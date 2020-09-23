using Newtonsoft.Json;

namespace WPSTORE.Models
{
    public class RegisterAccountResponseModel: BaseResponseModel
    {
        [JsonProperty("user_name")]
        public string UserName { get; set; }
    }
}
