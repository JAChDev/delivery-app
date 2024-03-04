using DeliveryAutomationSim.Models;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryAutomationSim.Services.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task NodesNotification(List<Node> bestNodes, int orderId, int cost, int earnings)
        {
            await Clients.All.SendAsync("NodesNotification", bestNodes, orderId, cost, earnings);
        }
    }
}
