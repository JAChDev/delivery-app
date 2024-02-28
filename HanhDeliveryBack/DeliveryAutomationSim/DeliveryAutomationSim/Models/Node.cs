namespace DeliveryAutomationSim.Models
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Edge> Edges { get; set; }

        public Node(int id, string name)
        {
            Id = id;
            Name = name;
            Edges = new List<Edge>();
        }
    }
}
