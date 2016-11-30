using System.Collections.Generic;

namespace AvalonAssets.Algorithm.Injection.Constructor
{
    /// <summary>
    ///     <see cref="IConstructor" /> using object instance.
    /// </summary>
    internal class InstanceConstructor : IConstructor
    {
        private readonly object _value;

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using object instance.
        /// </summary>
        /// <param name="value">Object instance.</param>
        public InstanceConstructor(object value)
        {
            _value = value;
        }

        public object NewInstance(IContainer container, IDictionary<string, object> parameters)
        {
            return _value;
        }
    }
}