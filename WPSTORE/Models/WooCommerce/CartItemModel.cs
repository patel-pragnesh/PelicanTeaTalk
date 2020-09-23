using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Models.WooCommerce
{
    public class CartItemModel : BindableBase
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                SetProperty(ref _totalPrice, value * Price, "TotalPrice");
            }
        }

        public decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                SetProperty(ref _totalPrice, value * Quantity, "TotalPrice");
            }
        }


        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }
    }
}
