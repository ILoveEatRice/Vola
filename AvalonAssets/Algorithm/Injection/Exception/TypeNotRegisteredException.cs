using System;

namespace AvalonAssets.Algorithm.Injection.Exception
{
    /// <summary>
    ///     Throws when the request type is not registered in the <see cref="IContainer" />.
    /// </summary>
    public class TypeNotRegisteredException : InvalidOperationException
    {
        public TypeNotRegisteredException(Type type) : base(string.Format("Can't resolve {0}.", type.FullName))
        {
        }
    }
}