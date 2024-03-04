using DeliveryAutomationSim.Models;

namespace DeliveryAutomationSim.Services.Interfaces
{
    public interface IAutomationServices
    {
        public (List<Node>, double) AStarAlgorithm(Graph graph, int startNodeId, int targetNodeId);
        public (List<Node>, double) getFullPathCost();
    }
}
