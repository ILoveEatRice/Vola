using System;

namespace AvalonAssets.Algorithm.Injection.Parameter
{
    internal class ResolveParameter : IParameter
    {
        private readonly string _resolveName;

        public ResolveParameter(Type type, string name,  string resolveName = null)
        {
            Name = name;
            Type = type;
            _resolveName = resolveName;
        }

        public Type Type { get; private set; }
        public string Name { get; private set; }

        public object Value(IContainer container)
        {
            return container.Resolve(Type, _resolveName);
        }
    }
}