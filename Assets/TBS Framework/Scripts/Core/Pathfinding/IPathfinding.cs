using System.Collections.Generic;
using System.Linq;

public abstract class IPathfinding
{
    /// <summary>
    /// Method finds path between two nodes in a graph.
    /// </summary>
    /// <param name="edges">
    /// Graph edges represented as nested dictionaries. Outer dictionary contains all nodes in the graph, inner dictionary contains 
    /// its neighbouring nodes with edge weight.
    /// </param>
    /// <returns>
    /// If the path exist, method returns list of nodes that the path consists of. Otherwise, empty list is returned.
    /// </returns>
    public abstract List<T> FindPath<T>(Dictionary<T, Dictionary<T, int>> edges, T originNode, T destinationNode) where T : IGraphNode;

    protected List<T> GetNeigbours<T>(Dictionary<T, Dictionary<T, int>> edges, T node) where T : IGraphNode
    {
        if (!edges.ContainsKey(node))
        {
            return new List<T>();
        }
        return edges[node].Keys.ToList();
    }
}