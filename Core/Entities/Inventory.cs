using System.Collections.Generic;

namespace Core.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<Media> Medias { get; set; }
    }
}
