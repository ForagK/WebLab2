namespace WebLab2.Models
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public string? Status { get; set; }
        public int? MinResponseTime { get; set; }
        public string? SortBy { get; set; }
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 50) ? 50 : value;
        }
    }
}
