using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WPSTORE.Models.WooCommerce
{

    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Parent { get; set; }
        public string Description { get; set; }
        public string Display { get; set; }
        public CategoryImage Image { get; set; }
        public int Count { get; set; }
    }

    public class CategoryImage
    {
        public int Id { get; set; }

        [JsonProperty("name")]
        public string ImageName { get; set; }

        [JsonProperty("alt")]
        public string ImageAlt { get; set; }

        [JsonProperty("date_created")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("date_modified")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("date_created_gmt")]
        public DateTime CreatedDateGmt { get; set; }

        [JsonProperty("date_modified_gmt")]
        public DateTime UpdatedDateGmt { get; set; }

        [JsonProperty("src")]
        public string ImageUrl { get; set; }
    }
}
