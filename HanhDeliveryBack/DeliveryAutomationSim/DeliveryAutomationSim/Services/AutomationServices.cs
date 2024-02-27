using DeliveryAutomationSim.Models;
using DeliveryAutomationSim.Services.Interfaces;

namespace DeliveryAutomationSim.Services
{
    public class AutomationServices : IAutomationService
    {
        private List<Node> nodes;
        private List<Edge> edges;
        private List<Connection> connections;
        public AutomationServices(List<Node> nodes, List<Edge> edges, List<Connection> connections)
        {
            this.nodes = nodes;
            this.edges = edges;
            this.connections = connections;

            InitializeGraph();
        }

        private void InitializeGraph()
        {
            foreach (var connection in connections)
            {
                Node firstNode = nodes.Find(node => node.Id == connection.FirstNodeId);
                Node secondNode = nodes.Find(node => node.Id == connection.SecondNodeId);

                if (firstNode != null && secondNode != null)
                {
                    firstNode.Neighbors.Add(secondNode);
                    secondNode.Neighbors.Add(firstNode);
                }
            }
        }
        public List<Node> AStar(int startNodeId, int targetNodeId, double realCost, double realTime)
        {
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();

            Node startNode = nodes.Find(node => node.Id == startNodeId);
            Node targetNode = nodes.Find(node => node.Id == targetNodeId);

            startNode.CostFromStart = 0;

            openSet.Add(startNode);


            while (openSet.Count > 0)
            {
                Node currentNode = FindNodeWithLowestCost(openSet);

                if (currentNode == targetNode)
                {
                    return ReconstructPath(startNode, targetNode);
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (var neighbor in currentNode.Neighbors)
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    double tentativeCost = currentNode.CostFromStart + CalculateCost(currentNode, neighbor);

                    if (!openSet.Contains(neighbor) || tentativeCost < neighbor.CostFromStart)
                    {
                        neighbor.CostFromStart = tentativeCost;
                        neighbor.Parent = currentNode;
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            return null;
        }
        private Node FindNodeWithLowestCost(List<Node> nodes)
        {
            Node lowestCostNode = nodes[0];
            foreach (var node in nodes)
            {
                if (node.CostFromStart < lowestCostNode.CostFromStart)
                {
                    lowestCostNode = node;
                }
            }
            return lowestCostNode;
        }
        private List<Node> ReconstructPath(Node startNode, Node targetNode)
        {
            var path = new List<Node>();
            Node currentNode = targetNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Add(startNode);
            path.Reverse();
            return path;
        }
        private double CalculateCost(Node currentNode, Node neighbor)
        {
            var connection = connections.FirstOrDefault(c =>
                (c.FirstNodeId == currentNode.Id && c.SecondNodeId == neighbor.Id) ||
                (c.FirstNodeId == neighbor.Id && c.SecondNodeId == currentNode.Id));

            if (connection != null)
            {
                var edge = edges.FirstOrDefault(e => e.Id == connection.EdgeId);

                if (edge != null)
                {
                    return edge.Cost;
                }
            }
            return double.PositiveInfinity; 
        }

    }
}
