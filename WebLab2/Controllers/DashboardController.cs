using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _service.GetDashboard();
            return Ok(result);
        }
        [HttpGet("report")]
        public async Task<IActionResult> GetDashboardSQL()
        {
            var result = await _service.GetDashboardSQL();
            return Ok(result);
        }
    }
}
