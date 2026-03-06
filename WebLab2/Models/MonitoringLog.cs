namespace WebLab2.Models
{
    public class MonitoringLog
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public int ResponseTimeMs { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
