using Newtonsoft.Json;
using Prism.Mvvm;

namespace WPSTORE.Models.WooCommerce.Requests
{
    public class NewOrderRequest : BaseRequest
    {
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("payment_method_title")]
        public string PaymentMethodTitle { get; set; }

        [JsonProperty("set_paid")]
        public bool SetPaid { get; set; }

        public Billing Billing { get; set; }
        public Shipping Shipping { get; set; }

        [JsonProperty("line_items")]
        public LineItem[] LineItems { get; set; }

        [JsonProperty("shipping_lines")]
        public ShippingLine[] ShippingLines { get; set; }

        [JsonProperty("customer_note")]
        public string CustomerNote { get; set; }
    }
    public class BaseShippingInfo
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("address_1")]
        public string Address1 { get; set; } = "";

        [JsonProperty("address_2")]
        public string Address2 { get; set; } = "";

        public string City { get; set; }
        public string State { get; set; } = "";
        public string Postcode { get; set; } = "";
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class Billing : BaseShippingInfo
    {
    }

    public class Shipping : BaseShippingInfo
    {
    }

    public class LineItem
    {
        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [JsonProperty("variation_id")]
        public int VariationId { get; set; }
    }

    public class ShippingLine
    {
        [JsonProperty("method_id")]
        public string MethodId { get; set; }

        [JsonProperty("method_title")]
        public string MethodTitle { get; set; }

        public int Total { get; set; }
    }
}
