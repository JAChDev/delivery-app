using Microsoft.AspNetCore.Mvc;

namespace DeliveryAutomationSim.Controllers
{
    [ApiController]
    [Route("api/automation")]
    public class AutomationController:Controller
    {
        [HttpPost]
        public IActionResult findRoute()
        {

            return Ok();
        }

    }
}
