using System.Collections.Generic;

/// <summary>
/// My implementation of greedy best first search algorithm. It isn't used anywhere in the project, I just wanted to try it out.
/// </summary>
class GreedyBestFirstSearch : IPathfinding
{
    public override List<T> FindPath<T>(Dictionary<T, Dictionary<T, int>> edges, T originNode, T destinationNode)
    {
        IPriorityQueue<T> frontier = new HeapPriorityQueue<T>();
        frontier.Enqueue(originNode,0);

        Dictionary<T, T> cameFrom = new Dictionary<T, T>();
        cameFrom.Add(originNode, default(T));
        
        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue();
            if (current.Equals(destinationNode))
                break;

            var neighbours = GetNeigbours(edges, current);
            foreach (var neighbour in neighbours)
            { 
                if (!cameFrom.ContainsKey(neighbour))
                {
                    cameFrom.Add(neighbour, current);
                    frontier.Enqueue(neighbour, Heuristic(destinationNode,neighbour));
                }
            }   
        }
        List<T> path = new List<T>();
        if (!cameFrom.ContainsKey(destinationNode))
            return path;

        path.Add(destinationNode);
        var temp = destinationNode;

        while (!cameFrom[temp].Equals(originNode))
        {
            var currentPathElement = cameFrom[temp];
            path.Add(currentPathElement);

            temp = currentPathElement;
        }
        return path;
    } 
    private int Heuristic<T>(T a, T b) where T : IGraphNode
    {
        return a.GetDistance(b);
    }
}

