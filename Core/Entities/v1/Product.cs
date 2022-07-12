using System;
using System.Collections.Generic;

namespace Core.Entities.v1
{
    public class Product
    {
        public int Product_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Category_Id { get; set; }
        public Category Category { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<Products_Cart> Products_Carts { get; set; }
    }
}
