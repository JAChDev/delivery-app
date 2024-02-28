using DeliveryAutomationSim.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using DeliveryAutomationSim.Services.Interfaces;
using System.Net.Http.Headers;

namespace DeliveryAutomationSim.Services
{
    public class GraphService:IGraphService
    {
        public readonly IConfiguration Configuration;
        private static Graph? _graph;

        public GraphService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // Consume grid service from Hahn Sim Services and creates a graph
        public async Task LoadAndBuildGraph()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    //Using burned token for testing purposes
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkNocmlzdGlhbiIsIm5iZiI6MTcwODc5MjQxNywiZXhwIjoxNzA5Mzk3MjE3LCJpYXQiOjE3MDg3OTI0MTd9.nILUTyVSsqHkX1Zek1avZ5UcikVHWoqqfxQvYoMQG7Y");
                    string servicePath = Configuration.GetValue<string>("HahnSimServices:GetGridData");
                    HttpResponseMessage response = await httpClient.GetAsync(servicePath);
                    response.EnsureSuccessStatusCode();


                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Data>(json);
                    _graph = BuildGraph(data.Nodes, data.Edges, data.Connections);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error executing HTTP request: {e.Message}");
                }
            }
        }

        public Graph GetGraph()
        {
            if (_graph == null)
            {
                throw new InvalidOperationException("You need to execute LoadAndBuildGraph function to build the graph.");
            }
            return _graph;
        }

        private Graph BuildGraph(List<NodeData> nodesData, List<EdgeData> edgesData, List<ConnectionData> connectionsData)
        {
            Graph graph = new Graph();

            //Add all nodes into graph
            foreach (var nodeData in nodesData)
            {
                Node newNode = new Node(nodeData.Id, nodeData.Name);
                graph.Nodes.Add(newNode);
            }

            //Use connections list from grid to create edges in graph linked with nodes
            foreach (var connectionData in connectionsData)
            {
                Node startNode = graph.Nodes.Find(node => node.Id == connectionData.FirstNodeId);
                Node endNode = graph.Nodes.Find(node => node.Id == connectionData.SecondNodeId);

                EdgeData edgeId = edgesData.Find(edge => edge.Id == connectionData.EdgeId);

                Edge newEdge = new Edge(edgeId.Id, edgeId.Cost, edgeId.Time, startNode.Id, endNode.Id);
                startNode.Edges.Add(newEdge);

            }

            return graph;
        }
    }
}
