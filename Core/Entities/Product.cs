using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }


        // Navigator Props
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public int SupplierInfoId { get; set; }
        public SupplierInfo SupplierInfo { get; set; }

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public List<Inventory> Inventories { get; set; }
        



    }
}
