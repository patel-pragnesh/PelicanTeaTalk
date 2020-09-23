using System;
using System.Collections.Generic;
using System.Text;
using WPSTORE.Models;

namespace WPSTORE.Services
{
    public class OrderServices
    {
        public List<Food> GetPopulars()
        {
            var food = new List<Food>
            {

                new Food {
                   Image_Food="https://i.pinimg.com/originals/ae/a2/71/aea271c5f75be7aea8ccfacc6f95b36e.jpg",
                   Title_Food="Lomo saltado ipsum dolo sit amet",
                   Price_Food="$ 69.50",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/46/b8/79/46b8793aac8e9162fad20cd34195dbbe.jpg",
                   Title_Food="Ceviche",
                   Price_Food="$ 780.0",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/cc/64/5f/cc645f2c094f29588fd6466b602bf30c.jpg",
                   Title_Food="Arroz chaufa rice paddys",
                   Price_Food="$ 34.30",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/9b/aa/0f/9baa0f7df306d8035596f4f29e0730f6.jpg",
                   Title_Food="Torta tres leches",
                   Price_Food="$ 150.00",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/8f/53/d3/8f53d3429ae7baa4bb69da6602d35b86.jpg",
                   Title_Food="Chancho al palo",
                   Price_Food="$ 7500",
                },
                new Food {
                   Image_Food="https://i.pinimg.com/564x/31/76/d0/3176d076d01b4eeb7f8bd2aaa5a13d37.jpg",
                   Title_Food="Pollo a la braza",
                   Price_Food="S/ 75",
                }
                };

            return food;
        }
    }
}
