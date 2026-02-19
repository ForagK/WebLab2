using Microsoft.EntityFrameworkCore;
using WebLab2.DataBase;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Services
{
    public class DashboardService: IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public DashboardService(IHttpClientFactory factory, AppDbContext context)
        {
            _httpClient = factory.CreateClient("LocalApi");
            _context = context;
        }

        public async Task<List<DashboardIncident>> GetDashboard()
        {
            var incidents = await _httpClient.GetFromJsonAsync<List<Incident>>($"api/incidents");

            var result = new List<DashboardIncident>();

            if (incidents != null)
                foreach (var incident in incidents)
                {
                    var service = await _httpClient.GetFromJsonAsync<Service>($"api/services/{incident.ServiceId}");
                    if (service == null)
                        continue;

                    var admin = await _httpClient.GetFromJsonAsync<Admin>($"api/admins/{service.AdminId}");
                    if (admin == null)
                        continue;

                    result.Add(new DashboardIncident
                    {
                        Time = incident.Time,
                        ServiceName = service.Name,
                        AdminName = admin.Name
                    });
                }
            return result;
        }

        public async Task<List<DashboardIncident>> GetDashboardSQL()
        {
            var result = await _context.Incidents
                .OrderByDescending(i => i.Time)
                .Take(10)
                .Select(i => new DashboardIncident
                {
                    ServiceName = i.Service.Name,
                    Time = i.Time,
                    AdminName = i.Service.Admin.Name
                })
                .ToListAsync();

            return result;
        }
    }
}
