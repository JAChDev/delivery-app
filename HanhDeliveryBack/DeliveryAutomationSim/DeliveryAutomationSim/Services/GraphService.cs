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
        private static string _token;
        private static object _grid;

        public GraphService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // Consume grid service from Hahn Sim Services and creates a graph
        public async Task GetTokenAndBuildGraph(string token)
        {
            _token = token;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    string servicePath = Configuration.GetValue<string>("HahnSimServices:GetGridData");
                    HttpResponseMessage response = await httpClient.GetAsync(servicePath);
                    response.EnsureSuccessStatusCode();


                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Data>(json);
                    _grid = data;
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
            return _graph;
        }

        public string GetToken()
        {
            return _token;
        }

        public object GetGrid()
        {
            return _grid;
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
