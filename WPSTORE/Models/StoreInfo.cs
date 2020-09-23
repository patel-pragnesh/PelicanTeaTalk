using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Models
{
    public class StoreInfo
    {
        public int id { get; set; }
        public string Store_Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string email { get; set; }
        public string Telephone { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        [JsonIgnore]
        public double Distance { get; set; }

        [JsonIgnore]
        public string DistanceStr => string.Format("{0:0.##} km", Distance / 1000);
    }
}
