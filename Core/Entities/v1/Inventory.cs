namespace Core.Entities.v1
{
    public class Inventory
    {
        public int Invetory_Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int Product_Id { get; set; }
        public Product Product { get; set; }
    }
}
