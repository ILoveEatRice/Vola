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
    internal class InjectionConstructor : IConstructor
    {
        private readonly ConstructorInfo _constructor;

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using normal constructor.
        /// </summary>
        /// <param name="constructor">Desire constructor.</param>
        public InjectionConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");
            _constructor = constructor;
        }

        public object NewInstance(IContainer container, IDictionary<string, object> parameters)
        {
            var paramsInfoList = _constructor.GetParameters().ToList();
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
            return _constructor.Invoke(resolvedParams.ToArray());
        }
    }
}