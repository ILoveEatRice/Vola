namespace AvalonAssets.Algorithm.Event
{
    /// <summary>
    ///     <para>
    ///         Wraps <see cref="ISubscriber{T}" /> to <see cref="WeakEventHandler" />.
    ///     </para>
    /// </summary>
    public class WeakEventHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="WeakEventHandler" /> with <paramref name="subscriber" />.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        /// <returns>New instance of <see cref="WeakEventHandler" />.</returns>
        public IEventHandler Create(ISubscriber subscriber)
        {
            return new WeakEventHandler(subscriber);
        }
    }
}