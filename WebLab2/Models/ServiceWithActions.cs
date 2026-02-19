namespace WebLab2.Models
{
    public class ServiceWithActions
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int AdminId { get; set; }
        public List<ActionLink> Links { get; set; }
    }
}
