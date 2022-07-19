namespace API.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int CartId { get; set; }
        public int InventoryId { get; set; }

        public string Inventory { get; set; }
    }
}