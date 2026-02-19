using Microsoft.AspNetCore.Mvc;
using WebLab2.Interfaces;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentService _service;
        public IncidentsController(IIncidentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get10Incidents()
        {
            var incidents = await _service.Get10Incidents();
            return Ok(incidents);
        }
    }
}
