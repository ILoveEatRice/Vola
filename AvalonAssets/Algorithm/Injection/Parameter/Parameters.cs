using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Algorithm.Injection.Parameter
{
    public static class Parameters
    {
        /// <summary>
        ///     Creates a <see cref="IParameter" /> by using <see cref="IContainer.Resolve" />.
        /// </summary>
        /// <param name="type">Type of the parameter.</param>
        /// <param name="name">Field name.</param>
        /// <param name="resolveName">Resolve name.</param>
        /// <returns><see cref="IParameter" /> for injection.</returns>
        /// <seealso cref="Resolve{T}" />
        public static IParameter Resolve(Type type, string name, string resolveName = null)
        {
            return new ResolveParameter(type, name, resolveName);
        }

        /// <summary>
        ///     Creates a <see cref="IParameter" /> by using <see cref="IContainer.Resolve" />.
        /// </summary>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        /// <param name="name">Field name.</param>
        /// <param name="resolveName">Resolve name.</param>
        /// <returns><see cref="IParameter" /> for injection.</returns>
        /// <seealso cref="Resolve{T}" />
        public static IParameter Resolve<T>(string name, string resolveName = null)
        {
            return Resolve(typeof(T), name, resolveName);
        }

        /// <summary>
        ///     Creates a <see cref="IParameter" /> by given <paramref name="value" />.
        /// </summary>
        /// <param name="value">Value of the parameter.</param>
        /// <param name="name">Field name.</param>
        /// <returns><see cref="IParameter" /> for injection.</returns>
        public static IParameter Value(object value, string name)
        {
            return new ValueParameter(name, value);
        }

        /// <summary>
        ///     Converts for <see cref="IContainer.Resolve" /> use.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <param name="container">Container.</param>
        /// <returns>Parameters.</returns>
        public static IDictionary<string, object> ToDictionary(this IParameter[] parameters, IContainer container)
        {
            return parameters.ToDictionary(parameter => parameter.Name, parameter => parameter.Value(container));
        }

        /// <summary>
        ///     Creates a <see cref="IParameter" /> that use default value.
        /// </summary>
        /// <param name="name">Field name.</param>
        /// <returns><see cref="IParameter" /> for injection.</returns>
        public static IParameter Default(string name)
        {
            return new ValueParameter(name, Type.Missing);
        }
    }
}