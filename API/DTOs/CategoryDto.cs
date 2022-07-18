namespace API.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}
