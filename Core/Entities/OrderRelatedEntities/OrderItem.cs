using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderRelatedEntities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Inventory Inventory { get; set; }
        public Order Order { get; set; }

    }
}
