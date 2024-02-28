using DeliveryAutomationSim.Models;

namespace DeliveryAutomationSim.Services.Interfaces
{
    public interface IGraphService
    {
        public Task LoadAndBuildGraph();
        public Graph GetGraph();

    }
}
