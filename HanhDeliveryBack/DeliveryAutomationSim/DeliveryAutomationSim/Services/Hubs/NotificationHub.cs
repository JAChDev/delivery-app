using Microsoft.AspNetCore.SignalR;

namespace DeliveryAutomationSim.Services.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task NodesNotification(int currentNodeId, double currentCost)
        {
            await Clients.All.SendAsync("NodesNotification", currentNodeId, currentCost);
        }

        public async Task OrderAcceptedNotification(int orderId, double earnings, double cost)
        {
            await Clients.All.SendAsync("OrderAcceptedNotification", orderId, earnings, cost);
        }
    }
}
