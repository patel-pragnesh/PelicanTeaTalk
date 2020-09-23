using Newtonsoft.Json;

namespace WPSTORE.Models.WooCommerce.Requests
{
    public class BaseRequest
    {
        public string consumer_key { get; set; } = GlobalSettings.WooCommerceConsumerKey;

        public string consumer_secret { get; set; } = GlobalSettings.WooCommerceConsumerSecret;
    }
}
