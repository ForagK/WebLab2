using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _service;

        public LogController(ILogService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLog(MonitoringLogDto dto)
        {
            var _log = await _service.CreateLog(dto);
            return base.CreatedAtAction("GetLogs", new { id = _log.Id }, _log);
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] PaginationParams pagParams)
        {
            var logs = await _service.GetLogs(pagParams);

            if (!logs.Items.Any()) return NoContent();

            var metadata = new
            {
                logs.TotalCount,
                logs.PageSize,
                logs.CurrentPage,
                logs.TotalPages
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(logs.Items);
        }
        [HttpGet("stream")]
        public async Task<IActionResult> GetLogsStream([FromQuery] StreamParams streamParams)
        {
            var logs = await _service.GetLogsStream(streamParams);

            if (!logs.Items.Any()) return NoContent();

            var metadata = new
            {
                logs.TotalCount,
                logs.Limit,
                logs.AfterId
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(logs.Items);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetLogsDashboard()
        {
            var logs = await _service.GetLogDashboard();

            return Ok(logs);
        }
    }
}
