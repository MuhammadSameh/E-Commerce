using System.Collections.Generic;

namespace Core.Entities.v1
{
    public class Cart
    {
        public int Cart_Id { get; set; }
        public List<Products_Cart> Products_Carts { get; set; }
    }
}
