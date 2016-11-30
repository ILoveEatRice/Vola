using System;

namespace AvalonAssets.Algorithm.Event
{
    /// <summary>
    ///     Event handle with publish–subscribe pattern.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        ///     Subscribes to this aggregator.
        /// </summary>
        /// <param name="subscriber">Subscriber</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" /> is null.</exception>
        void Subscribe(ISubscriber subscriber);

        /// <summary>
        ///     Unsubscribes to this aggregator.
        /// </summary>
        /// <param name="subscriber">Subscriber</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" /> is null.</exception>
        void Unsubscribe(ISubscriber subscriber);

        /// <summary>
        ///     Publishs <paramref name="message" /> to all <see cref="ISubscriber{T}" /> of that type.
        /// </summary>
        /// <param name="message">Message</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is null.</exception>
        void Publish(object message);
    }
}