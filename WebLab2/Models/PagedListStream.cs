namespace WebLab2.Models
{
    public class PagedListStream<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Limit { get; set; }
        public int AfterId { get; set; }
    }
}
