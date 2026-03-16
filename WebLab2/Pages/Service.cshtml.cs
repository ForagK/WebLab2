using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Pages
{
    public class ServiceModel : PageModel
    {
        public ServiceDetails ServiceDetails { get; set; }

        private readonly IServiceRepository _serviceRepository;

        public ServiceModel(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task OnGetAsync(int id)
        {
            ServiceDetails = await _serviceRepository.GetServiceDetails(id);
        }
    }
}
