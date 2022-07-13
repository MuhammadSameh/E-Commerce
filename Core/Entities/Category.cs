using System.Collections.Generic;

namespace Core.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Category ParentCategory { get; set; }
        public  List<Product> Products { get; set; }


    }
}
