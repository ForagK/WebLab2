using Microsoft.AspNetCore.Mvc;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStats(int id)
        {
            Thread.Sleep(2000);
            Response.Headers.Append("Cache-Control", "public, max-age=60");
            return Ok();
        }
    }
}
