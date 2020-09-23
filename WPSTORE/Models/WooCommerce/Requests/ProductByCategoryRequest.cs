namespace WPSTORE.Models.WooCommerce.Requests
{
    public class ProductByCategoryRequest : BasePagingModel
    {
        public string Category { get; set; }
    }
}
