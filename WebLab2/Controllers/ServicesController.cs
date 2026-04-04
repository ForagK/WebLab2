using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using System.Net.NetworkInformation;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _service;

        public ServicesController(IServiceRepository service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(ServiceDto serviceDto)
        {
            var _service = await this._service.CreateService(serviceDto);
            return base.CreatedAtAction("GetById", new { id = _service.Id }, _service);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (!await _service.DeleteService(id))
                return NotFound();

            return NoContent();
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var service = await _service.GetService(id);
        //    if (service == null)
        //        return NotFound();

        //    ServiceWithActions serviceWithActions = new ServiceWithActions
        //    {
        //        Id = service.Id,
        //        Name = service.Name,
        //        Status = service.Status,
        //        AdminId = service.AdminId,
        //        Links = new List<ActionLink>
        //        {
        //            new ActionLink { Rel = "self", Href = Url.Action("GetById", new { service.Id })!, Method = "GET" },
        //            new ActionLink { Rel = "delete", Href = Url.Action("DeleteService", new { service.Id })!, Method = "DELETE" },
        //            new ActionLink { Rel = "log", Href = Url.Action("GetLogs", new { service.Id })!, Method = "GET" },
        //        }
        //    };

        //    return Ok(serviceWithActions);
        //}

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [Obsolete]
        public async Task<IActionResult> GetStatusV1(int id)
        {
            var service = await _service.GetService(id);
            if (service == null)
                return NotFound();

            return Ok(service.Status);
        }

        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetStatusV2(int id)
        {
            var service = await _service.GetService(id);
            if (service == null)
                return NotFound();

            return Ok(new { status = service.Status, ping = Random.Shared.NextInt64(1000), lastCheck = DateTime.Now.AddDays(-1) });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, ServiceDto serviceDto)
        {
            var service = await _service.UpdateService(id, serviceDto);
            if (service == null)
                return NotFound();
            return Ok(service);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetServiceDetails(int id)
        {
            var serviceDetails = await _service.GetServiceDetails(id);
            if (serviceDetails == null)
                return NotFound();
            return Ok(serviceDetails);
        }

        [HttpGet("{id}/alert")]
        public async Task<IActionResult> SendAlert(int id)
        {
            for(int i = 0; i < 5; i++)
            {
                _service.TrySendAlert(id);
            }
            return Ok();
        }
    }
}
