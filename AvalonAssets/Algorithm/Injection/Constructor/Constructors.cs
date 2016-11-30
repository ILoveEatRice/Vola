using System;
using System.Reflection;

namespace AvalonAssets.Algorithm.Injection.Constructor
{
    public static class Constructors
    {
        public static IConstructor Instance(object value)
        {
            return new InstanceConstructor(value);
        }

        public static IConstructor Injection(ConstructorInfo constructor)
        {
            return new InjectionConstructor(constructor);
        }

        public static IConstructor Type(Type type)
        {
            return new TypeConstructor(type);
        }

        public static IConstructor Type<T>()
        {
            return Type(typeof(T));
        }
    }
}