namespace API.DTOs
{
    public class orderItemDto
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
