using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Algorithm.Event
{
    /// <summary>
    ///     <para>
    ///         Implementation of <see cref="IEventAggregator" />.
    ///     </para>
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        // Registered event handlers
        private readonly List<IEventHandler> _eventHandlers;
        private readonly IEventHandlerFactory _handlerFactory;

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="EventAggregator" /> with <see cref="IEventHandlerFactory" />.
        ///     </para>
        /// </summary>
        /// <param name="handlerFactory">Initializes new <see cref="IEventHandler" /> for <see cref="EventAggregator" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="handlerFactory" />is null.</exception>
        /// <seealso cref="EventAggregators.Default" />
        /// <remarks>
        ///     <para>
        ///         It is not recommend to use this directly. You should use <see cref="EventAggregators.Default" /> instead.
        ///     </para>
        /// </remarks>
        public EventAggregator(IEventHandlerFactory handlerFactory)
        {
            if (handlerFactory == null)
                throw new ArgumentNullException("handlerFactory");
            _handlerFactory = handlerFactory;
            _eventHandlers = new List<IEventHandler>();
        }

        /// <summary>
        ///     <para>
        ///         <paramref name="subscriber" /> subscribes to <see cref="ISubscriber{T}" /> that it implemented.
        ///         For example, if it implemented <see cref="ISubscriber{T}" /> of <see cref="string" />.
        ///         It will receives any published messages that is <see cref="string" /> or its subclass.
        ///     </para>
        ///     <para>
        ///         If <paramref name="subscriber" /> does not implement <see cref="ISubscriber{T}" />  or
        ///         it has already subscribe will be ignored.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that implements <see cref="ISubscriber{T}" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" />is null.</exception>
        public void Subscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");
            lock (_eventHandlers)
            {
                // Does not allow double subscribe
                if (_eventHandlers.Any(h => h.Matches(subscriber)))
                    return;
                var handler = _handlerFactory.Create(subscriber);
                // Registers if there is atleast one type
                if (handler.Types.Any())
                    _eventHandlers.Add(handler);
            }
        }

        /// <summary>
        ///     <para>
        ///         <paramref name="subscriber" /> unsubscribes from all <see cref="ISubscriber{T}" />.
        ///         If <paramref name="subscriber" /> does not subscribe, it will be ignored.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that already subscribes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" />is null.</exception>
        public void Unsubscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");
            lock (_eventHandlers)
            {
                _eventHandlers.RemoveAll(h => h.Matches(subscriber));
            }
        }

        /// <summary>
        ///     <para>
        ///         Publishs a <paramref name="message" /> to all the registered <see cref="ISubscriber{T}" /> of
        ///         <typeparamref name="T" /> or its super class.
        ///     </para>
        ///     <para>
        ///         The receive order is not guarantee.
        ///     </para>
        /// </summary>
        /// <param name="message">Message to be published.</param>
        public void Publish<T>(T message)
        {
            var messageType = typeof(T);
            IEventHandler[] toNotify;
            lock (_eventHandlers)
            {
                toNotify = _eventHandlers.Where(h => h.CanHandle(messageType)).ToArray();
            }
            // Publishes message
            var dead = toNotify.Where(h => !h.Handle(messageType, message)).ToList();
            if (!dead.Any()) return;
            // Clean up
            lock (_eventHandlers)
            {
                foreach (var handler in dead)
                {
                    _eventHandlers.Remove(handler);
                }
            }
        }
    }
}