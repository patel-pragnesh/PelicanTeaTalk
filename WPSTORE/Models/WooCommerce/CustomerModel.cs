using Newtonsoft.Json;
using System;
using WPSTORE.Models.WooCommerce.Requests;

namespace WPSTORE.Models.WooCommerce
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Billing = new Billing();
            Shipping = new Shipping();
        }

        public int Id { get; set; }

        [JsonProperty("date_created")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("date_created_gmt")]
        public DateTime CreatedDateGmt { get; set; }

        [JsonProperty("date_modified")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("date_modified_gmt")]
        public DateTime UpdatedDateGmt { get; set; }

        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        public string Role { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        public Billing Billing { get; set; }
        public Shipping Shipping { get; set; }

        [JsonProperty("is_paying_customer")]
        public bool IsPayingCustomer { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
    }
}
