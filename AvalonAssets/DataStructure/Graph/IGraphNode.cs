using System.Collections.Generic;

namespace AvalonAssets.DataStructure.Graph
{
    /// <summary>
    ///     Simple graph node.
    /// </summary>
    public interface IGraphNode<out T> where T : IGraphNode<T>
    {
        /// <summary>
        ///     Get the neighbor of current node.
        /// </summary>
        /// <returns>Neighbor of this node.</returns>
        IEnumerable<T> GetNeighbors();
    }
}