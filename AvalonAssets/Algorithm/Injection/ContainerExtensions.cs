using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AvalonAssets.Algorithm.Injection.Constructor;
using AvalonAssets.Algorithm.Injection.Parameter;

namespace AvalonAssets.Algorithm.Injection
{
    public static class ContainerExtensions
    {
        #region Non-Generics

        /// <summary>
        ///     Returns a instance of <paramref name="request" />.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="request">Request type.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <param name="parameters">Parameters pass to constructor.</param>
        /// <returns>Resolved request type.</returns>
        public static object Resolve(this IContainer container, Type request, string name = null,
            IDictionary<string, object> parameters = null)
        {
            return container.Resolve(request, name, parameters);
        }

        /// <summary>
        ///     Returns a instance of <paramref name="request" />.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="request">Request type.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <param name="parameters">Parameters pass to constructor.</param>
        /// <returns>Resolved request type.</returns>
        public static object Resolve(this IContainer container, Type request, string name = null,
            params IParameter[] parameters)
        {
            return container.Resolve(request, name, parameters.ToDictionary(container));
        }

        /// <summary>
        ///     Registers <paramref name="return" /> to <paramref name="request" />.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="request">Request type.</param>
        /// <param name="return">Return type.</param>
        /// <param name="constructor">Object constructor.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <returns>Itself.</returns>
        public static IContainer RegisterType(this IContainer container, Type request, Type @return,
            IConstructor constructor = null, string name = null)
        {
            return container.RegisterType(request, @return, constructor, name);
        }

        /// <summary>
        ///     Registers <paramref name="instance" /> to <paramref name="request" />.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="request">Request type.</param>
        /// <param name="instance">Object instance.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <returns>Itself.</returns>
        public static IContainer RegisterInstance(this IContainer container, Type request, object instance = null,
            string name = null)
        {
            return container.RegisterInstance(request, instance, name);
        }

        #endregion

        #region Generics

        /// <summary>
        ///     Registers <paramref name="constructor" /> to <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="constructor">Object constructor.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <returns>Itself.</returns>
        public static IContainer RegisterType<TRequest>(this IContainer container, ConstructorInfo constructor,
            string name = null) where TRequest : class
        {
            return container.RegisterType(typeof(TRequest), null, Constructors.Injection(constructor), name);
        }

        /// <summary>
        ///     Registers <typeparamref name="TReturn" /> to <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <typeparam name="TReturn">Return type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <returns>Itself.</returns>
        public static IContainer RegisterType<TRequest, TReturn>(this IContainer container, string name = null)
            where TReturn : TRequest where TRequest : class
        {
            return container.RegisterType(typeof(TRequest), typeof(TReturn), null, name);
        }

        /// <summary>
        ///     Registers <paramref name="instance" /> to <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="instance">Object instance.</param>
        /// <param name="name">Identifier. Null for default.</param>
        /// <returns>Itself.</returns>
        public static IContainer RegisterInstance<TRequest>(this IContainer container, TRequest instance = null,
            string name = null) where TRequest : class
        {
            return container.RegisterInstance(typeof(TRequest), instance, name);
        }

        /// <summary>
        ///     Returns a instance of <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="name">Identifier.</param>
        /// <param name="parameters">Parameters pass to constructor.</param>
        /// <returns>Resolved request type.</returns>
        public static TRequest Resolve<TRequest>(this IContainer container, string name,
            IDictionary<string, object> parameters = null) where TRequest : class
        {
            return container.Resolve(typeof(TRequest), name, parameters) as TRequest;
        }

        /// <summary>
        ///     Returns a instance of <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="parameters">Parameters pass to constructor.</param>
        /// <returns>Resolved request type.</returns>
        public static TRequest Resolve<TRequest>(this IContainer container,
            IDictionary<string, object> parameters = null) where TRequest : class
        {
            return container.Resolve<TRequest>(null, parameters);
        }

        /// <summary>
        ///     Returns a instance of <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="name">Identifier.</param>
        /// <param name="parameters">Parameters pass to constructor.</param>
        /// <returns>Resolved request type.</returns>
        public static TRequest Resolve<TRequest>(this IContainer container, string name, params IParameter[] parameters)
            where TRequest : class
        {
            if (parameters == null || parameters.Length == 0)
                return container.Resolve<TRequest>(name);
            return container.Resolve<TRequest>(name, parameters.ToDictionary(container));
        }

        /// <summary>
        ///     Returns a instance of <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="parameters">Parameters pass to constructor.</param>
        /// <returns>Resolved request type.</returns>
        public static TRequest Resolve<TRequest>(this IContainer container, params IParameter[] parameters)
            where TRequest : class
        {
            return container.Resolve<TRequest>(null, parameters);
        }

        /// <summary>
        ///     Returns all instance that registered to <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="parameters">Parameters pass to constructor.</param>
        /// <returns>All resolved request type.</returns>
        public static IEnumerable<TRequest> ResolveAll<TRequest>(this IContainer container,
            IDictionary<string, object> parameters = null) where TRequest : class
        {
            return container.ResolveAll(typeof(TRequest), parameters).Cast<TRequest>();
        }

        /// <summary>
        ///     Returns if <typeparamref name="TRequest" /> is registered.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <returns>True if <typeparamref name="TRequest" /> is registered.</returns>
        public static bool IsRegistered<TRequest>(this IContainer container) where TRequest : class
        {
            return container.IsRegistered(typeof(TRequest));
        }

        #endregion
    }
}