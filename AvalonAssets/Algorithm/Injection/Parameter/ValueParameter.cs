namespace AvalonAssets.Algorithm.Injection.Parameter
{
    internal class ValueParameter : IParameter
    {
        private readonly object _value;

        public ValueParameter(string name, object value)
        {
            Name = name;
            _value = value;
        }

        public string Name { get; private set; }

        public object Value(IContainer container)
        {
            return _value;
        }
    }
}