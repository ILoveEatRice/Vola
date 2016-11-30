using System;
using System.Reflection;

namespace AvalonAssets.Algorithm.Injection.Constructor
{
    /// <summary>
    ///     Provides static methods for commonly use <see cref="IConstructor" />.
    /// </summary>
    public static class Constructors
    {
        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using object instance.
        /// </summary>
        /// <param name="value">Object instance.</param>
        public static IConstructor Instance(object value)
        {
            return new InstanceConstructor(value);
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using normal constructor.
        /// </summary>
        /// <param name="constructor">Desire constructor.</param>
        public static IConstructor Injection(ConstructorInfo constructor)
        {
            return new InjectionConstructor(constructor);
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using given type.
        /// </summary>
        /// <param name="type">Type.</param>
        public static IConstructor Type(Type type)
        {
            return new TypeConstructor(type);
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using given type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        public static IConstructor Type<T>()
        {
            return Type(typeof(T));
        }
    }
}