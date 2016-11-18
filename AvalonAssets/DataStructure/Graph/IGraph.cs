using System.Collections.Generic;

namespace AvalonAssets.DataStructure.Graph
{
    /// <summary>
    ///     Simple graph
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public interface IGraph<TNode>
    {
        bool AllowSelfLoops { get; }
        IEnumerable<EndPointPair<TNode>> Edges { get; }
        bool IsDirected { get; }
        IEnumerable<TNode> Nodes { get; }

        /// <summary>
        ///     Get the neighbors of current node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Neighbor of <paramref name="node" />.</returns>
        IEnumerable<TNode> Neighbors(TNode node);
        int Degree(TNode node);
        int InDegree(TNode node);
        int OutDegree(TNode node);
        IEnumerable<TNode> Predecessors(TNode node);
        IEnumerable<TNode> Successors(TNode node);
    }
}