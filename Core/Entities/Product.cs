using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public int BrandId { get; set; }
        public Category Category { get; set; }

        public Brand Brand { get; set; }
        public List<Inventory> Inventories { get; set; }
        
    }
}
