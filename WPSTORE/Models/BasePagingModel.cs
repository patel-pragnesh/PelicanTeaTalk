using Newtonsoft.Json;
using WPSTORE.Models.WooCommerce.Requests;

namespace WPSTORE.Models
{
    public class BasePagingModel : BaseRequest
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PageSize { get; set; }
    }
}
