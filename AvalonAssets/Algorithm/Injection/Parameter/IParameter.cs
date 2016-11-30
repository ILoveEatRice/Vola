using System;

namespace AvalonAssets.Algorithm.Injection.Parameter
{
    public interface IParameter
    {
        Type Type { get; }
        string Name { get; }
        object Value(IContainer container);
    }
}