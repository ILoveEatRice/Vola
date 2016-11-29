using System.Collections.Generic;

namespace AvalonAssets.Algorithm.Injection
{
    /// <summary>
    ///     <see cref="IInjectionConstructor" /> using object instance.
    /// </summary>
    public class InstanceConstructor : IInjectionConstructor
    {
        private readonly object _value;

        /// <summary>
        ///     Creates a <see cref="IInjectionConstructor" /> using object instance.
        /// </summary>
        /// <param name="value">Object instance.</param>
        public InstanceConstructor(object value)
        {
            _value = value;
        }

        public object NewInstance(IContainer container, IDictionary<string, object> arguments)
        {
            return _value;
        }
    }
}