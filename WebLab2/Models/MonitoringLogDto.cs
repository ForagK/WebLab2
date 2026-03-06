namespace WebLab2.Models
{
    public class MonitoringLogDto
    {
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public int ResponseTimeMs { get; set; }
    }
}
