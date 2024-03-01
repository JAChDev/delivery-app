using DeliveryAutomationSim.Models;
using DeliveryAutomationSim.Services.Interfaces;

namespace DeliveryAutomationSim.Services
{
    public class AutomationServices:IAutomationServices
    {
        private (List<Node>,double) _fullPath;
        public IEnumerable<(Node,double)> AStarAlgorithm(Graph graph, int startNodeId, int targetNodeId)
        {
            Node? startNode = graph.Nodes.Find(node => node.Id == startNodeId);
            Node? targetNode = graph.Nodes.Find(node => node.Id == targetNodeId);

            //Define open and closed list for A* algorithm
            var openList = new List<PathNode>();
            var closedList = new HashSet<Node>();

            //Define initial path
            var initialPathNode = new PathNode(startNode, 0, CalculateHeuristic(startNode, targetNode), null);
            openList.Add(initialPathNode);

            //Execute loop searching the best path
            while (openList.Count > 0)
            {
                var currentNode = GetNodeWithLowestTotalCost(openList);

                //Evaluates if actual node is target node and return path, cost and Time (in process)
                if (currentNode.Node == targetNode)
                {
                    _fullPath = (ReconstructPath(currentNode), currentNode.CostFromStart);
                    yield return (currentNode.Node, currentNode.CostFromStart);
                    yield break;
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode.Node);

                foreach (var neighborEdge in currentNode.Node.Edges)
                {
                    var neighborNode = graph.Nodes.Find(node => node.Id == neighborEdge.EndNode);
                    
                    //Ignore checked nodes
                    if (closedList.Contains(neighborNode))
                    {
                        continue;
                    }

                    var newCostFromStart = currentNode.CostFromStart + neighborEdge.Cost;

                    var existingPathNode = openList.Find(node => node.Node == neighborNode);
                    if (existingPathNode == null || newCostFromStart < existingPathNode.CostFromStart)
                    {
                        var heuristicToEnd = CalculateHeuristic(neighborNode, targetNode);

                        var newPathNode = new PathNode(neighborNode, newCostFromStart, heuristicToEnd, currentNode);
                        if (existingPathNode != null)
                        {
                            openList.Remove(existingPathNode);
                        }
                        openList.Add(newPathNode);
                    }
                }
                yield return (currentNode.Node, currentNode.CostFromStart);

            }

            yield return (null, 0);

        }

        public (List<Node>, double) getFullPathCost()
        {
            return _fullPath;
        }

        private class PathNode
        {
            public Node Node { get; }
            public double CostFromStart { get; }
            public double HeuristicToEnd { get; }
            public PathNode Parent { get; }

            public PathNode(Node node, double costFromStart, double heuristicToEnd, PathNode parent)
            {
                Node = node;
                CostFromStart = costFromStart;
                HeuristicToEnd = heuristicToEnd;
                Parent = parent;
            }

            public double TotalCost => CostFromStart + HeuristicToEnd;
        }

        private static double CalculateHeuristic(Node node, Node targetNode)
        {
            //Absolute value between first and second node
            return Math.Abs(node.Id - targetNode.Id);
        }

        private static PathNode GetNodeWithLowestTotalCost(List<PathNode> nodeList)
        {
            var lowestCost = double.MaxValue;
            PathNode selectedNode = null;
            //Loop for evaluate cost and takes lower cost node
            foreach (var node in nodeList)
            {
                if (node.TotalCost < lowestCost)
                {
                    lowestCost = node.TotalCost;
                    selectedNode = node;
                }
            }
            return selectedNode;
        }

        private static List<Node> ReconstructPath(PathNode endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;
            while (currentNode != null)
            {
                path.Add(currentNode.Node);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

    }
}
