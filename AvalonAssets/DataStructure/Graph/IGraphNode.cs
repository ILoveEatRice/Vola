using System.Collections.Generic;

namespace AvalonAssets.DataStructure.Graph
{
    /// <summary>
    ///     Simple graph node.
    /// </summary>
    public interface IGraphNode
    {
        /// <summary>
        ///     Get the neighbor of current node.
        /// </summary>
        /// <returns>Neighbor of this node.</returns>
        IEnumerable<IGraphNode> GetNeighbors();
    }
}