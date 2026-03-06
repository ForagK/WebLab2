using WebLab2.Models;

namespace WebLab2.Interfaces
{
    public interface ILogService
    {
        public Task<MonitoringLog> CreateLog(MonitoringLogDto dto);
        public Task<MonitoringLog?> GetLog(int id);
        public Task<PagedList<MonitoringLog>> GetLogs(PaginationParams pagParams);
        public Task<PagedListStream<MonitoringLog>> GetLogsStream(StreamParams pagParams);
        public Task<List<MonitoringLog>> GetLogDashboard();
    }
}
