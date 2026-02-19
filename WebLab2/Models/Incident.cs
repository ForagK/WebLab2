namespace WebLab2.Models
{
    public class Incident
    {
        public int? Id { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
        public DateTime Time { get; set; }
    }
}
