using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebLab2.DataBase;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Services
{
    public class ServiceRepository: IServiceRepository
    {
        private readonly AppDbContext _context;
        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Service> CreateService(ServiceDto serviceDto)
        {
            var service = new Service
            {
                Name = serviceDto.Name,
                Status = serviceDto.Status,
                AdminId = serviceDto.AdminId,
            };

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

        public async Task<List<Service>> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return services;
        }

        public async Task<Service?> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return null;
            return service;
        }

        public async Task<Service?> UpdateService(int id, ServiceDto serviceDto)
        {
            var service = await _context.Services.FindAsync(id);
               
            if (service == null) return null;

            service.Name = serviceDto.Name;
            service.Status = serviceDto.Status;
            service.AdminId = serviceDto.AdminId;

            await _context.SaveChangesAsync();

            return service;
        }

        public async Task<ServiceDetails> GetServiceDetails(int id)
        {
            var serviceDetails = await _context.Services
                .Where(s => s.Id == id)
                .Select(s => new ServiceDetails
                {
                    ServiceId = (int)s.Id,
                    ServiceName = s.Name,
                    ServiceStatus = s.Status,
                    AdminName = s.Admin.Name,
                    AdminEmail = s.Admin.Email,
                    TimeSinceIncident = _context.Incidents
                        .Where(i => i.ServiceId == s.Id)
                        .OrderByDescending(i => i.Time)
                        .Select(i => DateTime.Now - i.Time)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (serviceDetails == null)
            {
                return new ServiceDetails
                {
                    ServiceId = -1,
                    ServiceName = "null",
                    ServiceStatus = "null",
                    AdminName = "null",
                    AdminEmail = "null",
                };
            }

            return serviceDetails;
        }

        public async Task<List<int>> GetServiceIds()
        {
            return _context.Services.Select(s => s.Id).ToList();
        }
        public async void TrySendAlert(int serviceId) {}
    }
}
