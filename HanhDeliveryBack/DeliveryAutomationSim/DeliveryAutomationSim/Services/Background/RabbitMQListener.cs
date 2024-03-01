using DeliveryAutomationSim.Controllers;
using DeliveryAutomationSim.Models;
using DeliveryAutomationSim.Services.Hubs;
using DeliveryAutomationSim.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace DeliveryAutomationSim.Services
{
    public class RabbitMQListener
    {
        public readonly IConfiguration Configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly IAutomationServices _automationServices = new AutomationServices { };
        private readonly IGraphService _graphService;
        private readonly IHubContext<NotificationHub> _hubContext;



        public RabbitMQListener(string hostName, string queueName, string password, string username, 
                                IGraphService graphService, IConfiguration configuration, IHubContext<NotificationHub> hubContext)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = username,
                Password = password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;
            _graphService = graphService;
            Configuration = configuration;
            _hubContext = hubContext;
        }

        public void StartConsuming()
        {
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);


            var consumer = new EventingBasicConsumer(_channel); 
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                ProcessMessage(message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessage(string message)
        {
            Order data = JsonConvert.DeserializeObject<Order>(message);
            var FindBestPath = _automationServices.AStarAlgorithm(_graphService.GetGraph(), data.OriginNodeId, data.targetNodeId);
            foreach (var (currentNode, currentCost) in  FindBestPath)
            {
                if (currentNode == null)
                {
                    continue;
                }
                _hubContext.Clients.All.SendAsync("NodesNotification",currentNode.Id, currentCost);
            }
            (List<Node>, double) pathList = _automationServices.getFullPathCost();

            if(pathList.Item2 < data.value)
            {
                var token = _graphService.GetToken();
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        string servicePath = Configuration.GetValue<string>("HahnSimServices:AcceptOrder");
                        UriBuilder uriBuilder = new UriBuilder(servicePath);
                        uriBuilder.Query = "orderId=" + data.Id;

                        HttpResponseMessage response = httpClient.PostAsync(uriBuilder.Uri, null).Result;
                        response.EnsureSuccessStatusCode();
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Error executing HTTP request: {e.Message}");
                    }
                }
                double earnings = data.value - pathList.Item2;
                _hubContext.Clients.All.SendAsync("OrderAcceptedNotification", data.Id, earnings, pathList.Item2);
            }
        }

    }
}
