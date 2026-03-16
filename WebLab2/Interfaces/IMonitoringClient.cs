namespace WebLab2.Interfaces
{
    public interface IMonitoringClient
    {
        Task ReceiveStatusUpdate(int serviceId, string status, int ms);
    }
}
