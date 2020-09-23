using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Models.WooCommerce
{
    public class TaxRatesModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Rate { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool Compound { get; set; }
        public bool Shipping { get; set; }
        public int Order { get; set; }
        public string Class { get; set; }
    }
}
