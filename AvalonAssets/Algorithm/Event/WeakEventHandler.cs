using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AvalonAssets.Algorithm.Event
{
    internal class WeakEventHandler : IEventHandler
    {
        private readonly Dictionary<Type, MethodInfo> _supportedHandlers;
        private readonly WeakReference _weakReference;

        public WeakEventHandler(object handler)
        {
            _weakReference = new WeakReference(handler);
            _supportedHandlers = new Dictionary<Type, MethodInfo>();
            // Gets all the ISubscriber<T> interface
            var interfaces = handler.GetType().GetInterfaces()
                .Where(x => typeof(ISubscriber).IsAssignableFrom(x) && x.IsGenericType);
            foreach (var @interface in interfaces)
            {
                var type = @interface.GetGenericArguments()[0];
                var method = @interface.GetMethod("Receive");
                _supportedHandlers[type] = method;
            }
        }

        public bool Alive
        {
            get { return _weakReference.Target != null; }
        }

        public bool Matches(object instance)
        {
            return _weakReference.Target == instance;
        }

        public bool Handle(Type messageType, object message)
        {
            if (!Alive)
                return false;
            foreach (var pair in _supportedHandlers)
            {
                // Continue if it is not the type or its sub-type
                if (!pair.Key.IsAssignableFrom(messageType)) continue;
                // Returns result ignored.
                pair.Value.Invoke(_weakReference.Target, new[] {message});
            }
            return true;
        }

        public bool CanHandle(Type messageType)
        {
            return _supportedHandlers.Any(pair => pair.Key.IsAssignableFrom(messageType));
        }
    }
}