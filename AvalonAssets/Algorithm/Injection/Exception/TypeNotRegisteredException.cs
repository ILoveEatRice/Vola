using System;

namespace AvalonAssets.Algorithm.Injection.Exception
{
    public class TypeNotRegisteredException : InvalidOperationException
    {
        public TypeNotRegisteredException(Type type) : base(string.Format("Can't resolve {0}.", type.FullName))
        {
        }
    }
}