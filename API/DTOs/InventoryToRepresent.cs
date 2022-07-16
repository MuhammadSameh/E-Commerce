using Core.Entities;
using System.Collections.Generic;

namespace API.DTOs
{
    public class InventoryToRepresent
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public string Brand { get; set; }
        public string Category { get; set; }
        public List<string> Medias { get; set; }
    }
}
