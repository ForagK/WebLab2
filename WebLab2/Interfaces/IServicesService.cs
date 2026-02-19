using WebLab2.Models;

namespace WebLab2.Interfaces
{
    public interface IServicesService
    {
        public Task<Service> CreateService(Service service);
        public Task<bool> DeleteService(int id);
        public Task<Service?> GetService(int id);
    }
}
