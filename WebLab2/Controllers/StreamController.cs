using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebLab2.Interfaces;
using WebLab2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private readonly IStreamService _service;

        public StreamController(IStreamService service)
        {
            _service = service;
        }

        [HttpGet("time")]
        public async Task GetLog(CancellationToken ct)
        {
            try
            {
                Response.Headers.Append("Content-Type", "text/event-stream");
                Response.Headers.Append("Cache-Control", "no-cache");
                Response.Headers.Append("Connection", "keep-alive");

                string lastId = Request.Headers["Last-Event-ID"];
                int startFrom = int.TryParse(lastId, out int id) ? id + 1 : 0;

                if (startFrom > 0)
                {
                    var oldLogs = _service.GetOldLogs(startFrom);

                    foreach (var log in oldLogs)
                    {
                        await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(log), ct);
                        await Response.Body.FlushAsync(ct);
                    }
                }

                while (!ct.IsCancellationRequested)
                {
                    var data = await _service.GetLog();

                    await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(data), ct);
                    await Response.Body.FlushAsync(ct);
                    await Task.Delay(1000, ct);

                    if (_service.CurrentId % 10 == 0 && _service.CurrentId != 0)
                    {
                        Console.WriteLine("testing disconnect");
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Client disconnected");
            }
        }
    }
}
