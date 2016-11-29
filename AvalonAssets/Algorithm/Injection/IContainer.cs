using System;
using System.Collections.Generic;

namespace AvalonAssets.Algorithm.Injection
{
    /// <summary>
    ///     Light weight container for Inversion of Control (IoC).
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        ///     Registers <paramref name="return" /> to <paramref name="request" />.
        /// </summary>
        /// <param name="request">Request type.</param>
        /// <param name="return">Return type.</param>
        /// <param name="constructor">Object constructor.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <returns>Itself.</returns>
        IContainer RegisterType(Type request, Type @return, IInjectionConstructor constructor, string name);

        /// <summary>
        ///     Registers <paramref name="instance" /> to <paramref name="request" />.
        /// </summary>
        /// <param name="request">Request type.</param>
        /// <param name="instance">Object instance.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <returns>Itself.</returns>
        IContainer RegisterInstance(Type request, object instance, string name);

        /// <summary>
        ///     Returns a instance of <paramref name="request" />.
        /// </summary>
        /// <param name="request">Request type.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <param name="arguments">Arguments pass to constructor.</param>
        /// <returns>Resolved request type.</returns>
        object Resolve(Type request, string name, IDictionary<string, object> arguments);

        /// <summary>
        ///     Returns all instance that registered to <paramref name="request" />.
        /// </summary>
        /// <param name="request">Request type.</param>
        /// <param name="arguments">Arguments pass to constructor.</param>
        /// <returns>All resolved request type.</returns>
        IEnumerable<object> ResolveAll(Type request, IDictionary<string, object> arguments);

        /// <summary>
        ///     Returns if <paramref name="request" /> is registered.
        /// </summary>
        /// <param name="request">Request type.</param>
        /// <returns>True if <paramref name="request" /> is registered.</returns>
        bool IsRegistered(Type request);
    }
}