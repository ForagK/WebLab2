using StackExchange.Redis;
using System.Text.Json;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Services
{
    public class CachedLogService: ILogService
    {
        private readonly ILogService _service;
        private readonly IConnectionMultiplexer _redis;
        private readonly string key = "global:recent_logs";
        public CachedLogService(ILogService service, IConnectionMultiplexer redis)
        {
            _service = service;
            _redis = redis;
        }
        public async Task<MonitoringLog> CreateLog(MonitoringLogDto dto)
        {
            var log = await _service.CreateLog(dto);

            var db = _redis.GetDatabase();
            var logJson = JsonSerializer.Serialize(log);

            await db.ListLeftPushAsync(key, logJson);
            await db.ListTrimAsync(key, 0, 9);

            return log;
        }

        public async Task<MonitoringLog?> GetLog(int id)
        {
            return await _service.GetLog(id);
        }

        public async Task<PagedList<MonitoringLog>> GetLogs(PaginationParams pagParams)
        {
            return await _service.GetLogs(pagParams);
        }

        public async Task<PagedListStream<MonitoringLog>> GetLogsStream(StreamParams pagParams)
        {
            return await _service.GetLogsStream(pagParams);
        }

        public async Task<List<MonitoringLog>> GetLogDashboard()
        {
            var db = _redis.GetDatabase();
            var list = await db.ListRangeAsync(key, 0, -1);

            var result = new List<MonitoringLog>();

            foreach (var item in list)
            {
                var log = JsonSerializer.Deserialize<MonitoringLog>((string)item);
                if (log != null)
                {
                    result.Add(log);
                }
            }

            return result;
        }
    }
}
