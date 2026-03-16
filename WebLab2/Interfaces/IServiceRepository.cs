using WebLab2.Models;

namespace WebLab2.Interfaces
{
    public interface IServiceRepository
    {
        public Task<Service> CreateService(ServiceDto serviceDto);
        public Task<bool> DeleteService(int id);
        public Task<List<Service>> GetServices();
        public Task<Service?> GetService(int id);
        public Task<ServiceDetails> GetServiceDetails(int id);
        public Task<List<int>> GetServiceIds();
        public Task<Service?> UpdateService(int id, ServiceDto serviceDto);
        public void TrySendAlert(int serviceId);
    }
}
