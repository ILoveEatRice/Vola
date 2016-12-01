namespace AvalonAssets.Algorithm.Event
{
    /// <summary>
    ///     <para>
    ///         <see cref="EventAggregators" /> provides static methods for commonly used <see cref="IEventAggregator" />.
    ///     </para>
    /// </summary>
    public static class EventAggregators
    {
        /// <summary>
        ///     <para>
        ///         Initializes a new instance of a default implementaion of <see cref="IEventAggregator" />.
        ///     </para>
        /// </summary>
        /// <param name="handlerFactory">Initializes new <see cref="IEventHandler" /> for <see cref="IEventAggregator" />.</param>
        /// <returns>New instance of <see cref="IEventAggregator" />.</returns>
        public static IEventAggregator Default(IEventHandlerFactory handlerFactory = null)
        {
            if (handlerFactory == null)
                handlerFactory = new WeakEventHandlerFactory();
            return new EventAggregator(handlerFactory);
        }
    }
}