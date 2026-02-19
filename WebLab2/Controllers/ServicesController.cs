using Microsoft.AspNetCore.Mvc;
using WebLab2.Models;
using WebLab2.Interfaces;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _service;

        public ServicesController(IServicesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(Service service)
        {
            var _service = await this._service.CreateService(service);
            return base.CreatedAtAction("GetById", new { id = _service.Id }, _service);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (!await _service.DeleteService(id))
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _service.GetService(id);
            if (service == null)
                return NotFound();

            ServiceWithActions serviceWithActions = new ServiceWithActions
            {
                Id = service.Id,
                Name = service.Name,
                Status = service.Status,
                AdminId = service.AdminId,
                Links = new List<ActionLink>
                {
                    new ActionLink { Rel = "self", Href = Url.Action("GetById", new { service.Id })!, Method = "GET" },
                    new ActionLink { Rel = "delete", Href = Url.Action("DeleteService", new { service.Id })!, Method = "DELETE" },
                    new ActionLink { Rel = "log", Href = Url.Action("GetLogs", new { service.Id })!, Method = "GET" },
                }
            };

            return Ok(serviceWithActions);
        }

        [HttpGet("{id}/logs")]
        public IActionResult GetLogs(int id)
        {
            return Ok();
        }
    }
}
