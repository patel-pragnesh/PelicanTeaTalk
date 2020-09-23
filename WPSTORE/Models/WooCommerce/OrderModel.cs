using Newtonsoft.Json;
using System;
using WPSTORE.Models.WooCommerce.Requests;

namespace WPSTORE.Models.WooCommerce
{
    public class OrderModel
    {
        public int Id { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        public string Number { get; set; }

        [JsonProperty("order_key")]
        public string OrderKey { get; set; }

        [JsonProperty("created_via")]
        public string CreatedVia { get; set; }

        public string Version { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }

        [JsonProperty("date_created")]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public string CreatedOnStr => CreatedDate.ToLocalTime().ToString("dd MMM yyyy hh:mm tt");

        [JsonProperty("date_created_gmt")]
        public DateTime CreatedDateGmt { get; set; }

        [JsonProperty("date_modified")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("date_modified_gmt")]
        public DateTime UpdatedDateGmt { get; set; }

        [JsonProperty("discount_total")]
        public string DiscountTotal { get; set; }

        [JsonProperty("discount_tax")]
        public string DiscountTax { get; set; }

        [JsonProperty("shipping_total")]
        public string ShippingTotal { get; set; }

        [JsonProperty("shipping_tax")]
        public string ShippingTax { get; set; }

        [JsonProperty("cart_tax")]
        public string CartTax { get; set; }

        public string Total { get; set; }

        [JsonProperty("total_tax")]
        public string TotalTax { get; set; }

        [JsonProperty("prices_include_tax")]
        public bool PricesIncludeTax { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("customer_note")]
        public string CustomerNote { get; set; }
        public Billing Billing { get; set; }
        public Shipping Shipping { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("payment_method_title")]
        public string PaymentMethodTitle { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("line_items")]
        public LineItemDetail[] LineItems { get; set; }

        [JsonProperty("tax_lines")]
        public object[] TaxLines { get; set; }

        [JsonProperty("shipping_lines")]
        public ShippingLineDetail[] ShippingLines { get; set; }

        [JsonProperty("fee_lines")]
        public object[] FeeLines { get; set; }

        [JsonProperty("coupon_lines")]
        public CouponLine[] CouponLines { get; set; }
        public object[] Refunds { get; set; }

        [JsonProperty("currency_symbol")]
        public string CurrencySymbol { get; set; }

        public class LineItemDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }

            [JsonProperty("product_id")]
            public int pProductId { get; set; }

            [JsonProperty("variation_id")]
            public int VariationId { get; set; }

            public int Quantity { get; set; }

            [JsonProperty("tax_class")]
            public string TaxClass { get; set; }

            [JsonProperty("subtotal")]
            public string SubTotal { get; set; }

            [JsonProperty("subtotal_tax")]
            public string SubtoTalTax { get; set; }

            public string Total { get; set; }

            [JsonProperty("total_tax")]
            public string TotalTax { get; set; }
            public object[] Taxes { get; set; }

            [JsonProperty("meta_data")]
            public object[] MetadData { get; set; }
            public string Sku { get; set; }
            public decimal Price { get; set; }
        }

        public class ShippingLineDetail
        {
            public int Id { get; set; }

            [JsonProperty("method_title")]
            public string MethodTitle { get; set; }

            [JsonProperty("method_id")]
            public string MethodId { get; set; }

            [JsonProperty("instance_id")]
            public string InstanceId { get; set; }

            public string Total { get; set; }

            [JsonProperty("total_tax")]
            public string TotalTax { get; set; }

            public object[] Taxes { get; set; }

            [JsonProperty("meta_data")]
            public ShippingLineMetaData[] MetaData { get; set; }
        }


        public class ShippingLineMetaData : BaseMetaData
        {
        }

        public class CouponLine
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Discount { get; set; }

            [JsonProperty("discount_tax")]
            public string DiscountTax { get; set; }

            [JsonProperty("meta_data")]
            public CouponLineMetaData[] MetaData { get; set; }
        }

        public class CouponLineMetaData
        {
            public int Id { get; set; }
            public string Key { get; set; }
            public CouponLineMetaValue Value { get; set; }
        }

        public class CouponLineMetaValue
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Amount { get; set; }

            [JsonProperty("discount_type")]
            public string DiscountType { get; set; }

            public string Description { get; set; }

            [JsonProperty("usage_count")]
            public int UsageCount { get; set; }

            [JsonProperty("individual_use")]
            public bool IndividualUse { get; set; }

            [JsonProperty("product_ids")]
            public object[] ProductIds { get; set; }

            [JsonProperty("excluded_product_ids")]
            public object[] ExcludedProductIds { get; set; }

            [JsonProperty("usage_limit")]
            public int UsageLimit { get; set; }

            [JsonProperty("usage_limit_per_user")]
            public int UsageLimitPerUser { get; set; }

            [JsonProperty("limit_usage_to_x_items")]
            public object LimitUsageToItems { get; set; }

            [JsonProperty("free_shipping")]
            public bool FreeShipping { get; set; }

            [JsonProperty("product_categories")]
            public object[] ProductCategories { get; set; }

            [JsonProperty("excluded_product_categories")]
            public object[] ExcludedProductCategories { get; set; }

            [JsonProperty("exclude_sale_items")]
            public bool ExcludeSaleItems { get; set; }

            [JsonProperty("minimum_amount")]
            public string MinimumAmount { get; set; }

            [JsonProperty("maximum_amount")]
            public string MaximumAmount { get; set; }

            [JsonProperty("email_restrictions")]
            public object[] EmailRestrictions { get; set; }

            [JsonProperty("meta_data")]
            public BaseMetaData[] MetaData { get; set; }
        }
    }
    public class BaseMetaData
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
