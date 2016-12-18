using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Algorithm.Injection.Exception;

namespace AvalonAssets.Algorithm.Injection.Constructor
{
    /// <summary>
    ///     <see cref="IConstructor" /> using given type.
    /// </summary>
    internal class TypeConstructor : IConstructor
    {
        private readonly Type _type;

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using given type.
        ///     Uses <see cref="Constructors.Type{T}" /> instead.
        /// </summary>
        /// <param name="type">Type.</param>
        public TypeConstructor(Type type)
        {
            _type = type;
        }

        public object NewInstance(IContainer container, IDictionary<string, object> parameters)
        {
            foreach (var constructor in _type.GetConstructors())
            {
                var selfLoop = constructor.GetParameters()
                    .Any(p => p.ParameterType == _type && parameters != null && !parameters.ContainsKey(p.Name));
                if (selfLoop)
                    continue; // Prevents self loop.
                try
                {
                    var injectionConstructor = Constructors.Injection(constructor);
                    return injectionConstructor.NewInstance(container, parameters);
                }
                catch (TypeNotRegisteredException)
                {
                    // Falls back to next constructor if any.
                }
            }
            // No constructor works
            throw new TypeNotRegisteredException(_type);
        }
    }
}