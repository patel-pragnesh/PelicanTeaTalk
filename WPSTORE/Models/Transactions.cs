using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Models
{
    public class Transactions
    {
        public string Image { get; set; }
        public string OrderID { get; set; }
        public DateTime Date { get; set; }
        public string PointEarned { get; set; }
    }
}
