namespace AvalonAssets.Algorithm.Event
{
    /// <summary>
    ///     Provides static methods for commonly use <see cref="IEventAggregator" />.
    /// </summary>
    public static class EventAggregators
    {
        /// <summary>
        ///     Creates a default implementaion of <see cref="IEventAggregator" />.
        /// </summary>
        /// <returns><see cref="IEventAggregator" />.</returns>
        public static IEventAggregator Default()
        {
            return new EventAggregator(new WeakEventHandlerFactory());
        }
    }
}