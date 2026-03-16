using Microsoft.AspNetCore.SignalR;
using WebLab2.Hubs;
using WebLab2.Interfaces;

namespace WebLab2.Services
{
    public class UptimeMonitorWorker : BackgroundService
    {
        private readonly IHubContext<MonitoringHub, IMonitoringClient> _hubContext;
        public UptimeMonitorWorker(IHubContext<MonitoringHub, IMonitoringClient> hubContext)
        {
            _hubContext = hubContext;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rand = new Random();
            var ids = new List<int> { 2, 3, 5, 6 };

            while (!stoppingToken.IsCancellationRequested)
            {
                var randomId = ids[rand.Next(ids.Count)];

                string randomStatus = rand.Next(2) == 1 ? randomStatus = "Up" : randomStatus = "Down";

                var randomTime = rand.Next(2000);

                await _hubContext.Clients.All.ReceiveStatusUpdate(randomId, randomStatus, randomTime);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
