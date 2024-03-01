using DeliveryAutomationSim.Models;
using DeliveryAutomationSim.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DeliveryAutomationSim.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DeliveryAutomationController:ControllerBase
    {
        private readonly IGraphService _graphService;
        public DeliveryAutomationController(IGraphService graphService)
        {
            _graphService = graphService;
        }

        [HttpPost]
        public IActionResult TokenAndBuild([FromBody] Token token)
        {
            try
            {
                var tokenString = token.token;
                _graphService.GetTokenAndBuildGraph(tokenString);
                return Ok(_graphService.GetGrid());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        

    }
}
