using Newtonsoft.Json;

namespace WPSTORE.Models.WooCommerce.Requests
{
    public class OrderHistoryRequest: BasePagingModel
    {
        [JsonProperty("customer")]
        public int? CustomerId { get; set; }

        [JsonProperty("include")]
        public int[] OrderIds { get; set; }
    }
}
