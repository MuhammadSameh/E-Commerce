using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        // Navigator Props
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public List<Inventory> Inventories { get; set; }
        public List<Media> Medias { get; set; }



    }
}
