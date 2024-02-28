using DeliveryAutomationSim.Models;
using DeliveryAutomationSim.Services;
using DeliveryAutomationSim.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeliveryAutomationSim.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutomationController:Controller
    {
        private readonly IGraphService _graphService;

        public AutomationController(IGraphService graphService)
        {
            _graphService = graphService;
        }

        [HttpPost]
        public async Task<IActionResult> findRoute()
        {
            try
            {
                var grid = _graphService.GetGraph();
                //Using burned data from testing purposes
                (List<Node>,double) pathResult = AutomationServices.AStarAlgorithm(grid, 268, 11);
                return Ok(pathResult.Item1);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> graph()
        {
            try
            {
                var response = _graphService.GetGraph();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
