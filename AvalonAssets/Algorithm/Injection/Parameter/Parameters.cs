using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Algorithm.Injection.Parameter
{
    public static class Parameters
    {
        public static IParameter Resolve(Type type, string name = null, string resolveName = null)
        {
            return new ResolveParameter(type, name, resolveName);
        }

        public static IParameter Resolve<T>(string name = null, string resolveName = null)
        {
            return Resolve(typeof(T), name, resolveName);
        }

        public static IParameter Value(object value, string name = null)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return new ValueParameter(name, value);
        }

        public static IParameter Value(Type type, object value, string name = null)
        {
            return new ValueParameter(name, value, type);
        }

        public static IParameter Value<T>(object value, string name = null)
        {
            return Value(typeof(T), value, name);
        }

        public static IDictionary<string, object> ToDictionary(this IParameter[] parameters, IContainer container)
        {
            return parameters.ToDictionary(parameter => parameter.Name, parameter => parameter.Value(container));
        }
    }
}