namespace AvalonAssets.DataStructure.Graph
{
    /// <summary>
    ///     Simple weight graph
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWeightGraph<T> : IGraph<T>
    {
        /// <summary>
        ///     Get the weight from <paramref cref="from" /> to <paramref cref="to" />.
        /// </summary>
        /// <param name="from">Node.</param>
        /// <param name="to">Node.</param>
        /// <returns>Required weight.</returns>
        int GetWeight(T from, T to);
    }
}