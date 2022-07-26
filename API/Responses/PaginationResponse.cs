namespace API.Responses
{
    public class PaginationResponse<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public T Data { get; set; }

    }
}
