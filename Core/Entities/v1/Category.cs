using System.Collections.Generic;

namespace Core.Entities.v1
{
    public class Category
    {
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
