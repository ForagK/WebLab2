namespace WebLab2.Models
{
    public class Service
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Admin Admin { get; set; }
        public int AdminId { get; set; }
    }
}
