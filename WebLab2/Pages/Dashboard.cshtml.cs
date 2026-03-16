using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Pages
{
    public class DashboardModel : PageModel
    {
        public List<Service> Services {  get; set; }
        private readonly IServiceRepository _serviceRepository;
        public DashboardModel(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        public async Task OnGetAsync()
        {
            Services = await _serviceRepository.GetServices();
        }
    }
}
