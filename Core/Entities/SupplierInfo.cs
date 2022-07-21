using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SupplierInfo
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        List<Product> Products { get; set; } = new List<Product>();
    }
}
