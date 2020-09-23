using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace WPSTORE.Models.WooCommerce
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Permalink { get; set; }

        [JsonProperty("date_created")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("date_created_gmt")]
        public DateTime CreatedDateGmt { get; set; }

        [JsonProperty("date_modified")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("date_modified_gmt")]
        public DateTime UpdatedDateGmt { get; set; }

        public string Type { get; set; }
        public string Status { get; set; }
        public bool Featured { get; set; }

        [JsonProperty("catalog_visibility")]
        public string CatalogVisibility { get; set; }

        public string Description { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        public string Sku { get; set; }
        public string Price { get; set; }

        [JsonProperty("regular_price")]
        public string RegularPrice { get; set; }

        [JsonProperty("sale_price")]
        public string SalePrice { get; set; }

        [JsonProperty("date_on_sale_from")]
        public DateTime OnSaleFrom { get; set; }

        [JsonProperty("date_on_sale_from_gmt")]
        public DateTime OnSaleFromGmt { get; set; }

        [JsonProperty("date_on_sale_to")]
        public DateTime OnSaleTo { get; set; }

        [JsonProperty("date_on_sale_to_gmt")]
        public DateTime OnSaleToGmt { get; set; }

        [JsonProperty("price_html")]
        public string PriceHtml { get; set; }

        [JsonProperty("on_sale")]
        public bool OnSale { get; set; }

        public bool Purchaseble { get; set; }

        [JsonProperty("total_sales")]
        public int TotalSales { get; set; }

        public bool Virtual { get; set; }
        public bool Downloadable { get; set; }

        public DownloadProduct[] Downloads { get; set; }

        [JsonProperty("download_limit")]
        public int DownloadLimit { get; set; }

        [JsonProperty("download_expery")]
        public int DownloadExpery { get; set; }

        [JsonProperty("external_url")]
        public string ExternalUrl { get; set; }

        [JsonProperty("button_text")]
        public string ButtonText { get; set; }

        [JsonProperty("tax_status")]
        public string TaxStatus { get; set; }

        [JsonProperty("tax_class")]
        public string TaxClass { get; set; }

        [JsonProperty("manage_stock")]
        public bool ManageStock { get; set; }

        [JsonProperty("stock_quantity")]
        public int StockQuantity { get; set; }

        [JsonProperty("stock_status")]
        public string StockStatus { get; set; }

        public string Backorders { get; set; }

        [JsonProperty("backorders_allowed")]
        public bool BackOrderAllowed { get; set; }

        public bool Backordered { get; set; }

        [JsonProperty("sold_individually")]
        public bool SoldeIndividually { get; set; }

        public string Weight { get; set; }
        public Dimensions Dimensions { get; set; }

        [JsonProperty("shipping_required")]
        public bool ShippingRequired { get; set; }

        [JsonProperty("shipping_taxable")]
        public bool ShippingTaxable { get; set; }

        [JsonProperty("shipping_class")]
        public string ShippingClass { get; set; }

        [JsonProperty("shipping_class_id")]
        public int ShippingClassId { get; set; }

        [JsonProperty("reviews_allowed")]
        public bool ReviewAllowed { get; set; }

        [JsonProperty("average_rating")]
        public string AverageRating { get; set; }

        [JsonProperty("rating_count")]
        public int RatingCount { get; set; }

        [JsonProperty("related_ids")]
        public object[] RelatedIds { get; set; }

        [JsonProperty("upsell_ids")]
        public object[] UpsellIds { get; set; }

        [JsonProperty("cross_sell_ids")]
        public object[] CrossSellIds { get; set; }

        public ProductCategories[] Categories { get; set; }

        public object[] Tags { get; set; }

        [JsonIgnore]
        public string ProductImage => Images != null ? Images.Select(x => x.ImageUrl).FirstOrDefault() : "";

        public ProductImage[] Images { get; set; }
        public ProductAttributes[] Attributes { get; set; }

        [JsonProperty("default_attributes")]
        public ProductDefaultAttributes[] DefaultAttributes { get; set; }

        public object[] Variations { get; set; }

        [JsonProperty("grouped_products")]
        public object[] GroupedProducts { get; set; }

        [JsonProperty("menu_order")]
        public int MenuOrder { get; set; }

        //[JsonProperty("meta_data")]
        //public MetaData[] MetaData { get; set; }
    }
    public class ProductImage
    {
        public int Id { get; set; }

        [JsonProperty("date_created")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("date_modified")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("src")]
        public string ImageUrl { get; set; }
    }

    public class DownloadProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
    }

    public class ProductCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
    public class Dimensions
    {
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class ProductAttributes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool Visible { get; set; }
        public bool Variation { get; set; }
        public object[] Options { get; set; }
    }

    public class ProductDefaultAttributes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
    }

    public class MetaData
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
