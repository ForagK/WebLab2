using Microsoft.AspNetCore.SignalR;
using System.Net.NetworkInformation;
using WebLab2.Interfaces;
using WebLab2.Models;

namespace WebLab2.Hubs
{
    public class MonitoringHub : Hub<IMonitoringClient>
    {
        public async Task UpdateServiceStatus() {}

        public async Task JoinServiceGroup(int serviceId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Watch_{serviceId}");
        }

        public async Task LeaveServiceGroup(int serviceId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Watch_{serviceId}");
        }
    }
}
