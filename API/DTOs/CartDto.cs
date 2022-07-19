using System.Collections.Generic;

namespace API.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; }
    }
}
