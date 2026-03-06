namespace WebLab2.Models
{
    public class ServiceDetails
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceStatus { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public TimeSpan TimeSinceIncident { get; set; }
    }
}
