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
        private int _transporter;



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
            var token = _graphService.GetToken();

            //Buy transporter if _transporter is empty
            if(_transporter==null||_transporter==0)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        string servicePath = Configuration.GetValue<string>("HahnSimServices:BuyTransporter");
                        Uri urlWithParams = new Uri(servicePath + "?positionNodeId=" + data.OriginNodeId);
                        HttpResponseMessage response = httpClient.PostAsync(urlWithParams, null).Result;
                        response.EnsureSuccessStatusCode();
                        string json = response.Content.ReadAsStringAsync().Result;
                        _transporter = Convert.ToInt32(json);
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Error executing HTTP request: {e.Message}");
                    }
                }
            }

            (List<Node>, double) bestPath = _automationServices.AStarAlgorithm(_graphService.GetGraph(), data.OriginNodeId, data.targetNodeId);
            List<Node> bestNodes = bestPath.Item1;
            if (bestPath.Item2 < data.value)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        string servicePath = Configuration.GetValue<string>("HahnSimServices:AcceptOrder");
                        Uri urlWithParams = new Uri(servicePath + "?orderId=" + data.Id);
                        HttpResponseMessage response = httpClient.PostAsync(urlWithParams, null ).Result;
                        response.EnsureSuccessStatusCode();
                        string json = response.Content.ReadAsStringAsync().Result;
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Error executing HTTP request: {e.Message}");
                    }
                }

                //PENDING TO FIX: Add edge time delay to move between nodes
                //foreach(Node node in bestNodes)
                //{
                //    using (var httpClient = new HttpClient())
                //    {
                //        try
                //        {
                //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //            string servicePath = Configuration.GetValue<string>("HahnSimServices:MoveTransporter");
                //            Uri urlWithParams = new Uri(servicePath + "?transporterId=" + _transporter+"&targetNodeId="+node.Id);
                //            HttpResponseMessage response = httpClient.PutAsync(urlWithParams, null).Result;
                //            response.EnsureSuccessStatusCode();
                //            string json = response.Content.ReadAsStringAsync().Result;
                //        }
                //        catch (HttpRequestException e)
                //        {
                //            Console.WriteLine($"Error executing HTTP request: {e.Message}");
                //        }
                //    }
                //}
                _hubContext.Clients.All.SendAsync("NodesNotification", bestNodes, data.Id, bestPath.Item2, data.value-bestPath.Item2);

            }
        }

    }
}
