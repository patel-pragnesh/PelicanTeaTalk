using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Models.WooCommerce
{
    public class CountryModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public State[] States { get; set; }
    }
    public class State
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
