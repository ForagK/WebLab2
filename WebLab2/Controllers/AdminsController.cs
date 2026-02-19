using Microsoft.AspNetCore.Mvc;
using WebLab2.Interfaces;

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminsController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var admin = await _service.GetAdmin(id);
            if (admin == null)
                return NotFound();
            return Ok(admin);
        }
     }
}
