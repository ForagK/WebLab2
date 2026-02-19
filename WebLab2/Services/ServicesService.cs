using WebLab2.DataBase;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Services
{
    public class ServicesService: IServicesService
    {
        private readonly AppDbContext _context;
        public ServicesService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Service> CreateService(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return service;
        }

        public async Task<bool> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Service?> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return null;
            return service;
        }
    }
}
