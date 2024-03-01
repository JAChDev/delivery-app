using DeliveryAutomationSim.Controllers;
using DeliveryAutomationSim.Services.Hubs;
using DeliveryAutomationSim.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryAutomationSim.Services.Background
{
    public class BackgroundExecutionService:BackgroundService
    {
        private readonly ILogger<BackgroundExecutionService> _logger;
        private readonly IGraphService _graphService;
        private readonly IAutomationServices _automationServices;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;
        private readonly string _orderQueue;

        public BackgroundExecutionService(ILogger<BackgroundExecutionService> logger, IConfiguration configuration, 
                                          IGraphService graphService, IAutomationServices automationServices,
                                          IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _hostName = configuration.GetValue<string>("RabbitMQSettings:Host");
            _username = configuration.GetValue<string>("RabbitMQSettings:Username");
            _password = configuration.GetValue<string>("RabbitMQSettings:Password");
            _orderQueue = configuration.GetValue<string>("RabbitMQSettings:QueueOrders");
            _graphService = graphService;
            _automationServices = automationServices;
            _configuration = configuration;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                if (_graphService.GetToken() == null || _graphService.GetToken().Length <= 0)
                {
                    await Task.Delay(2000, stoppingToken);
                    continue;
                }

                //Define queue order setting
                var consumerOrder = new RabbitMQListener(_hostName, _orderQueue, _password, _username, _graphService, _configuration, _hubContext);
                consumerOrder.StartConsuming();

                consumerOrder.Close();

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
