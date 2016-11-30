using System;

namespace AvalonAssets.Algorithm.Injection.Parameter
{
    internal class ValueParameter : IParameter
    {
        private readonly object _value;

        public ValueParameter(string name, object value, Type type = null)
        {
            Name = name;
            Type = type ?? value.GetType();
            _value = value;
        }

        public Type Type { get; private set; }
        public string Name { get; private set; }

        public object Value(IContainer container)
        {
            return _value;
        }
    }
}