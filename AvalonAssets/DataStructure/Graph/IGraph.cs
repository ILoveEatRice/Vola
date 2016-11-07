using System.Collections.Generic;

namespace AvalonAssets.DataStructure.Graph
{
    /// <summary>
    ///     Simple graph
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGraph<T>
    {
        /// <summary>
        ///     Get the neighbor of current node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Neighbor of <paramref name="node" />.</returns>
        IEnumerable<T> Neighbors(T node);
    }
}