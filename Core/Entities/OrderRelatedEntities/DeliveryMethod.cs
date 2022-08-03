using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderRelatedEntities
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string MethodDesc { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Price { get; set; }
    }
}
