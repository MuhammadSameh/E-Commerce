using System.Collections.Generic;

namespace Core.Entities.v1
{
    public class Products_Cart
    {
        public int Cart_Id { get; set; }
        public Cart Cart { get; set; }
        public int Product_Id { get; set; }
        public Product Product { get; set; }
    }
}
