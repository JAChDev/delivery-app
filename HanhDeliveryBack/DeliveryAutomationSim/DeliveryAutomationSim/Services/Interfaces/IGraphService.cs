using DeliveryAutomationSim.Models;

namespace DeliveryAutomationSim.Services.Interfaces
{
    public interface IGraphService
    {
        public Task GetTokenAndBuildGraph(string token);
        public Graph GetGraph();
        public string GetToken();

    }
}
