using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Algorithm
{
    /// <summary>
    ///     Light weight container for Inversion of Control (IoC).
    /// </summary>
    public class IocContainer
    {
        private readonly Dictionary<Type, Func<object>> _iocFuncs;
        private readonly Dictionary<Type, Type> _iocMap;

        /// <summary>
        ///     Creates a new <see cref="IocContainer" />
        /// </summary>
        public IocContainer()
        {
            _iocMap = new Dictionary<Type, Type>();
            _iocFuncs = new Dictionary<Type, Func<object>>();
        }

        /// <summary>
        ///     Registers <typeparamref name="TResolve" /> to <typeparamref name="TRegister" />.
        /// </summary>
        /// <typeparam name="TRegister">Register type.</typeparam>
        /// <typeparam name="TResolve">Resolve type.</typeparam>
        /// <param name="overwrite">Allow overwrite registered type. Default false.</param>
        /// <returns>Itself.</returns>
        public IocContainer Register<TRegister, TResolve>(bool overwrite = false)
        {
            var type = typeof(TRegister);
            if (IsRegistered(type) && !overwrite)
                throw new InvalidOperationException(string.Format("Type {0} already registered.",
                    type.FullName));
            _iocMap[type] = typeof(TResolve);
            return this;
        }

        /// <summary>
        ///     Registers <typeparamref name="TRegister" /> with a delegate.
        /// </summary>
        /// <typeparam name="TRegister">Register type.</typeparam>
        /// <param name="register">Function returns an instance of <typeparamref name="TRegister" />.</param>
        /// <param name="overwrite">Allow overwrite registered type. Default false.</param>
        /// <returns>Itself.</returns>
        public IocContainer Register<TRegister>(Func<object> register, bool overwrite = false)
        {
            var type = typeof(TRegister);
            if (IsRegistered(type) && !overwrite)
                throw new InvalidOperationException(string.Format("Type {0} already registered.",
                    type.FullName));
            _iocFuncs[type] = register;
            return this;
        }

        /// <summary>
        ///     Returns if <typeparamref name="TRegister" /> is registered.
        /// </summary>
        /// <typeparam name="TRegister">Register type.</typeparam>
        /// <returns>True if <typeparamref name="TRegister" /> is registered.</returns>
        public bool IsRegistered<TRegister>()
        {
            var type = typeof(TRegister);
            return IsRegistered(type);
        }

        private bool IsRegistered(Type type)
        {
            return _iocMap.ContainsKey(type) || _iocFuncs.ContainsKey(type);
        }

        /// <summary>
        ///     Creates a instance of <typeparamref name="TRegister" />.
        /// </summary>
        /// <typeparam name="TRegister">Register type.</typeparam>
        /// <returns>Resolved register type.</returns>
        public TRegister Resolve<TRegister>()
        {
            return (TRegister) Resolve(typeof(TRegister));
        }

        private object Resolve(Type typeToResolve)
        {
            // Uses register delegate if any.
            // Not needs to resolve further dependencies.
            if (_iocFuncs.ContainsKey(typeToResolve))
                return _iocFuncs[typeToResolve]();
            if (!_iocMap.ContainsKey(typeToResolve))
                throw new InvalidOperationException(string.Format("Can't resolve {0}. Type is not registed.",
                    typeToResolve.FullName));
            var resolvedType = _iocMap[typeToResolve];
            // Gets first constructor. 
            foreach (var constructor in resolvedType.GetConstructors())
            {
                // Gets constructor parameter(s)
                var paramsInfo = constructor.GetParameters().ToList();
                // Resolves constructor parameter(s) if any.
                try
                {
                    var resolvedParams = new List<object>();
                    foreach (var param in paramsInfo)
                    {
                        var t = param.ParameterType;
                        object value;
                        try
                        {
                            value = Resolve(t);
                        }
                        catch (InvalidOperationException)
                        {
                            // fall back to defualt value if any.
                            if (param.HasDefaultValue)
                                value = param.RawDefaultValue;
                            else
                                throw;
                        }
                        resolvedParams.Add(value);
                    }

                    // Using reflection invoke constructor to create the object.
                    var retObject = constructor.Invoke(resolvedParams.ToArray());
                    return retObject;
                }
                catch (InvalidOperationException)
                {
                    // fall back to next constructor if any.
                }
            }
            throw new InvalidOperationException(string.Format("Can't resolve {0}. Type is not registed.",
                typeToResolve.FullName));
        }
    }
}