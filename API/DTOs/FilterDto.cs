namespace API.DTOs
{
    public class FilterDto
    {
        public string BrandName{ get; set; }
        public string CategoryName { get; set; }
        public string Color { get; set; }
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set;}

    }
}
