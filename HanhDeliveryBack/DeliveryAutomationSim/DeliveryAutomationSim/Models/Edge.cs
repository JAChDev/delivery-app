using System.Text.Json.Serialization;

namespace DeliveryAutomationSim.Models
{
    public class Edge
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public TimeSpan Time { get; set; }
        public int StartNode { get; set; }
        public int EndNode { get; set; }

        public Edge(int id, int cost, TimeSpan time, int startNode, int endNode)
        {
            Id = id;
            Cost = cost;
            Time = time;
            StartNode = startNode;
            EndNode = endNode;
        }
    }
}
