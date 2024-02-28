namespace DeliveryAutomationSim.Models
{
    public class NodeData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EdgeData
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public TimeSpan Time { get; set; }
    }

    public class ConnectionData
    {
        public int Id { get; set; }
        public int EdgeId { get; set; }
        public int FirstNodeId { get; set; }
        public int SecondNodeId { get; set; }
    }
}
