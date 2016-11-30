using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Algorithm.Event
{
    /// <summary>
    ///     Implementation of <see cref="IEventAggregator" />.
    /// </summary>
    /// <seealso cref="EventAggregators.Default" />
    internal class EventAggregator : IEventAggregator
    {
        private readonly List<IEventHandler> _eventHandlers;
        private readonly IEventHandlerFactory _handlerFactory;

        public EventAggregator(IEventHandlerFactory handlerFactory)
        {
            if (handlerFactory == null)
                throw new ArgumentNullException("handlerFactory");
            _handlerFactory = handlerFactory;
            _eventHandlers = new List<IEventHandler>();
        }

        public void Subscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");
            lock (_eventHandlers)
            {
                if (!_eventHandlers.Any(h => h.Matches(subscriber)))
                    _eventHandlers.Add(_handlerFactory.Create(subscriber));
            }
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");
            lock (_eventHandlers)
            {
                _eventHandlers.RemoveAll(h => h.Matches(subscriber));
            }
        }

        public void Publish(object message)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            var messageType = message.GetType();
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