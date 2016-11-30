using System;

namespace AvalonAssets.Algorithm.Injection.Exception
{
    internal class NameNotResolveException : InvalidOperationException
    {
        public NameNotResolveException(string name)
            : base(string.Format("Type registered but can't resolve {0}.", name ?? "default"))
        {
        }
    }
}