using System;

namespace AvalonAssets.Algorithm.Injection.Parameter
{
    internal class ResolveParameter : IParameter
    {
        private readonly string _resolveName;
        private readonly Type _type;

        public ResolveParameter(Type type, string name, string resolveName = null)
        {
            Name = name;
            _type = type;
            _resolveName = resolveName;
        }

        public string Name { get; private set; }

        public object Value(IContainer container)
        {
            return container.Resolve(_type, _resolveName);
        }
    }
}