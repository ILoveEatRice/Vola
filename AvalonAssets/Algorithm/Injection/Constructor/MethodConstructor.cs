using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AvalonAssets.Algorithm.Injection.Exception;

namespace AvalonAssets.Algorithm.Injection.Constructor
{
    /// <summary>
    ///     <see cref="IConstructor" /> using normal constructor.
    /// </summary>
    internal class MethodConstructor : IConstructor
    {
        private readonly MethodBase _method;
        private readonly object _object;

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using normal constructor.
        ///     Uses <see cref="Constructors.Injection" /> instead.
        /// </summary>
        /// <param name="constructor">Desire constructor.</param>
        public MethodConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");
            _method = constructor;
            _object = null;
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using factory method.
        ///     Uses <see cref="Constructors.Factory" /> or <see cref="Constructors.StaticFactory" /> instead.
        /// </summary>
        /// <param name="method">Factory method.</param>
        /// <param name="object">Object. Null if static method.</param>
        public MethodConstructor(MethodInfo method, object @object = null)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            _method = method;
            _object = @object;
        }

        public object NewInstance(IContainer container, IDictionary<string, object> parameters)
        {
            var paramsInfoList = _method.GetParameters().ToList();
            var resolvedParams = new List<object>();
            foreach (var paramsInfo in paramsInfoList)
            {
                object value;
                // Uses given parameters if possible.
                if (parameters != null && parameters.ContainsKey(paramsInfo.Name))
                    value = parameters[paramsInfo.Name];
                else
                {
                    try
                    {
                        // Asks container to resolve type.
                        value = container.Resolve(paramsInfo.ParameterType);
                    }
                    catch (TypeNotRegisteredException)
                    {
                        // Falls back to defualt value if any.
                        if (paramsInfo.HasDefaultValue)
                            value = paramsInfo.RawDefaultValue;
                        else
                            throw;
                    }
                }
                resolvedParams.Add(value);
            }
            var resolvedArray = resolvedParams.Count > 0 ? resolvedParams.ToArray() : null;
            var constructor = _method as ConstructorInfo;
            return constructor != null ? constructor.Invoke(resolvedArray) : _method.Invoke(_object, resolvedArray);
        }
    }
}