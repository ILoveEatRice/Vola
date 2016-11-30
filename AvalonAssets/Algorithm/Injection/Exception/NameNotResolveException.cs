using System;

namespace AvalonAssets.Algorithm.Injection.Exception
{
    /// <summary>
    ///     Throws when the request type is registered but the required name cannot be found in the <see cref="IContainer" />.
    /// </summary>
    public class NameNotResolveException : InvalidOperationException
    {
        public NameNotResolveException(string name)
            : base(string.Format("Type registered but can't resolve {0}.", name ?? "default"))
        {
        }
    }
}