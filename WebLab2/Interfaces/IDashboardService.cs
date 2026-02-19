using WebLab2.Models;

namespace WebLab2.Interfaces
{
    public interface IDashboardService
    {
        public Task<List<DashboardIncident>> GetDashboard();
        public Task<List<DashboardIncident>> GetDashboardSQL();
    }
}
