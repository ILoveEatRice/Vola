﻿using System;
using System.Linq;

namespace AvalonAssets.Algorithm.Event
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        ///     Handles <paramref name="message" /> of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Message type.</typeparam>
        /// <param name="handler">Handler.</param>
        /// <param name="message">Message</param>
        /// <returns>True if the object is alive.</returns>
        public static bool Handle<T>(this IEventHandler handler, T message)
        {
            return handler.Handle(typeof(T), message);
        }

        /// <summary>
        ///     Returns true if it can handle <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Message type.</typeparam>
        /// <param name="handler">Handler.</param>
        /// <returns>True if it can handle <typeparamref name="T" />.</returns>
        public static bool CanHandle<T>(this IEventHandler handler)
        {
            return handler.CanHandle(typeof(T));
        }

        /// <summary>
        ///     Returns true if it can handle <paramref name="messageType" />.
        /// </summary>
        /// <param name="handler">Handler.</param>
        /// <param name="messageType">Message type.</param>
        /// <returns>True if it can handle <paramref name="messageType" />.</returns>
        public static bool CanHandle(this IEventHandler handler, Type messageType)
        {
            return handler.Types.Contains(messageType);
        }
    }
}