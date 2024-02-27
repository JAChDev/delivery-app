using DeliveryAutomationSim.Models;

namespace DeliveryAutomationSim.Services.Interfaces
{
    public interface IAutomationService
    {
        public List<Node> AStar(int startNodeId, int targetNodeId, double realCost, double realTime);
    }
}
