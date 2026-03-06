using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using System.Text.Json;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Services
{
    public class CachedServiceRepository: IServiceRepository
    {
        private readonly IServiceRepository _service;
        private readonly IConnectionMultiplexer _redis;
        public CachedServiceRepository(IServiceRepository service, IConnectionMultiplexer redis)
        {
            _service = service;
            _redis = redis;
        }

        public async Task<ServiceDetails> GetServiceDetails(int id)
        {
            string cacheKey = $"service:{id}:details";

            var db = _redis.GetDatabase();

            string cachedValue = await db.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedValue))
            {
                return JsonSerializer.Deserialize<ServiceDetails>(cachedValue);
            }

            var serviceDetails = await _service.GetServiceDetails(id);
            
            if (serviceDetails != null && serviceDetails.ServiceId != -1)
            {
                string json = JsonSerializer.Serialize(serviceDetails);

                await db.StringSetAsync(cacheKey, json, TimeSpan.FromMinutes(5));
            }
            return serviceDetails;
        }



        public async Task<Service> CreateService(ServiceDto serviceDto)
        {
            return await _service.CreateService(serviceDto);
        }

        public async Task<bool> DeleteService(int id)
        {
            return await _service.DeleteService(id);
        }

        public async Task<Service?> GetService(int id)
        {
            return await _service.GetService(id);
        }

        public async Task<Service?> UpdateService(int id, ServiceDto serviceDto)
        {
            var service = await _service.UpdateService(id, serviceDto);
            if (service != null) {
                await _redis.GetDatabase().KeyDeleteAsync($"service:{id}:details");
            }
            return service;
        }
        public async void TrySendAlert(int serviceId)
        {
            string key = $"alert:lock:{serviceId}";

            var db = _redis.GetDatabase();

            bool wasSet = await db.StringSetAsync(key, "sent", TimeSpan.FromHours(1), When.NotExists);
            if (wasSet)
            {
                Console.WriteLine("Email sent!");
            }
            else
            {
                Console.WriteLine("Alert suppressed (cooldown).");
            }

        }
    }
}
