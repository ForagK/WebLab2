using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebLab2.Hubs;
using WebLab2.Models;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHubContext<MonitoringHub> _hubContext;

        public TestController(IHubContext<MonitoringHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("update/{serviceId}")]
        public async Task<IActionResult> Test(int serviceId)
        {
            await _hubContext.Clients.Group($"Watch_{serviceId}").SendAsync("UpdateServiceStatus", serviceId, "Down", 0);
            return Ok();
        }
    }
}
