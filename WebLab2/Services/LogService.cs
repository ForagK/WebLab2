using Microsoft.EntityFrameworkCore;
using WebLab2.DataBase;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Services
{
    public class LogService: ILogService
    {
        private readonly AppDbContext _context;
        public LogService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MonitoringLog> CreateLog(MonitoringLogDto dto)
        {
            var log = new MonitoringLog
            {
                ServiceName = dto.ServiceName,
                Status = dto.Status,
                ResponseTimeMs = dto.ResponseTimeMs,
                CreatedAt = DateTime.Now
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return log;
        }

        public async Task<MonitoringLog?> GetLog(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null) return null;
            return log;
        }

        public async Task<PagedList<MonitoringLog>> GetLogs(PaginationParams pagParams)
        {
            if(pagParams.PageNumber <= 0 || pagParams.PageSize <= 0)
            {
                return new PagedList<MonitoringLog>();
            }

            var query = _context.Logs.AsQueryable();

            if (!string.IsNullOrEmpty(pagParams.Status))
            {
                query = query.Where(l => l.Status == pagParams.Status);
            }

            if (pagParams.MinResponseTime != null)
            {
                query = query.Where(l => l.ResponseTimeMs >= pagParams.MinResponseTime);
            }

            query = pagParams.SortBy switch
            {
                "time" => query.OrderByDescending(l => l.ResponseTimeMs),
                "time_asc" => query.OrderBy(l => l.ResponseTimeMs),
                "createdAt_asc" => query.OrderBy(l => l.CreatedAt),
                _ => query.OrderByDescending(l => l.CreatedAt)
            };

            var totalCount = await query.CountAsync();

            var logs = await query.Skip((pagParams.PageNumber - 1) * pagParams.PageSize).Take(pagParams.PageSize).ToListAsync();

            var result = new PagedList<MonitoringLog>()
            {
                Items = logs,
                TotalCount = totalCount,
                PageSize = pagParams.PageSize,
                CurrentPage = pagParams.PageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagParams.PageSize)
            };

            return result;
        }

        public async Task<PagedListStream<MonitoringLog>> GetLogsStream(StreamParams streamParams)
        {
            if (streamParams.Limit <= 0)
            {
                return new PagedListStream<MonitoringLog>();
            }

            var query = _context.Logs.AsQueryable();
            
            var totalCount = await query.CountAsync();    

            var logs = await query.Where(l => l.Id > streamParams.AfterId).Take(streamParams.Limit).ToListAsync();

            var result = new PagedListStream<MonitoringLog>()
            {
                Items = logs,
                TotalCount = totalCount,
                Limit = streamParams.Limit,
                AfterId = streamParams.AfterId
            };

            return result;
        }
        public async Task<List<MonitoringLog>> GetLogDashboard() {
            return new List<MonitoringLog>();
        }
    }
}
