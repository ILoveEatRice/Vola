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
        /// <returns>Constructor for injection.</returns>
        public static IConstructor Instance(object value)
        {
            return new InstanceConstructor(value);
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using normal constructor.
        /// </summary>
        /// <param name="constructor">Desire constructor.</param>
        /// <returns>Constructor for injection.</returns>
        public static IConstructor Injection(ConstructorInfo constructor)
        {
            return new MethodConstructor(constructor);
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using given type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Constructor for injection.</returns>
        public static IConstructor Type(Type type)
        {
            return new TypeConstructor(type);
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using given type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Constructor for injection.</returns>
        public static IConstructor Type<T>()
        {
            return Type(typeof(T));
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using factory method.
        ///     Note that <paramref name="object" /> strong reference by the <see cref="IConstructor" />.
        /// </summary>
        /// <param name="method">Factory method.</param>
        /// <param name="object">Factory.</param>
        /// <returns>Constructor for injection.</returns>
        public static IConstructor Factory(MethodInfo method, object @object)
        {
            return new MethodConstructor(method, @object);
        }

        /// <summary>
        ///     Creates a <see cref="IConstructor" /> using static factory method.
        /// </summary>
        /// <param name="method">Static factory method.</param>
        /// <returns>Constructor for injection.</returns>
        public static IConstructor StaticFactory(MethodInfo method)
        {
            return new MethodConstructor(method);
        }
    }
}