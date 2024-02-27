namespace DeliveryAutomationSim.Models
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CostFromStart { get; set; }
        public List<Node> Neighbors { get; } = new List<Node>();
        public Node Parent { get; set; }
        public Node(int id)
        {
            Id = id;
            CostFromStart = double.PositiveInfinity;
        }
    }

    public class Edge
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public TimeSpan Time { get; set; }
    }

    public class Connection
    {
        public int Id { get; set; }
        public int EdgeId { get; set; }
        public int FirstNodeId { get; set; }
        public int SecondNodeId { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int OriginNodeId { get; set; }
        public int TargetNodeId { get; set; }
        public int Load { get; set; }
        public int Value { get; set; }
        public DateTime DeliveryDateUtc { get; set; }
        public DateTime ExpirationDateUtc { get; set; }
    }

    public class PathNode
    {
        public Node Node { get; set; }
        public int CostFromStart { get; set; }
        public TimeSpan TimeFromStart { get; set; }
        public int EstimatedCostToTarget { get; set; }
        public PathNode PreviousNode { get; set; }
    }
}
