using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Models.WooCommerce
{
    public class CouponModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Amount { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_created_gmt { get; set; }
        public DateTime date_modified { get; set; }
        public DateTime date_modified_gmt { get; set; }
        public string discount_type { get; set; }
        public string Description { get; set; }
        public DateTime date_expires { get; set; }
        public DateTime date_expires_gmt { get; set; }
        public int usage_count { get; set; }
        public bool individual_use { get; set; }
        public object[] product_ids { get; set; }
        public object[] excluded_product_ids { get; set; }
        public int usage_limit { get; set; }
        public int usage_limit_per_user { get; set; }
        public object limit_usage_to_x_items { get; set; }
        public bool free_shipping { get; set; }
        public object[] product_categories { get; set; }
        public object[] excluded_product_categories { get; set; }
        public bool exclude_sale_items { get; set; }
        public string minimum_amount { get; set; }
        public string maximum_amount { get; set; }
        public object[] email_restrictions { get; set; }
        public string[] used_by { get; set; }

    }
}
