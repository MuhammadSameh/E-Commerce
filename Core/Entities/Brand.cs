using System.Collections.Generic;

namespace Core.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public List<Product> Products { get; set; }
    }
}
