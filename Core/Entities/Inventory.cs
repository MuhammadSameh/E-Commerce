using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    [Index(nameof(Inventory.Price))]
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<Media> Medias { get; set; }
    }
}
