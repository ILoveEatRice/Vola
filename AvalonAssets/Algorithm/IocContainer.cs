using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Algorithm
{
    /// <summary>
    ///     Container for Inversion of Control (IoC).
    /// </summary>
    public class IocContainer
    {
        private readonly Dictionary<Type, Type> _iocMap;

        /// <summary>
        ///     Creates a new <see cref="IocContainer" />
        /// </summary>
        public IocContainer()
        {
            _iocMap = new Dictionary<Type, Type>();
        }

        /// <summary>
        ///     Registers <typeparamref name="TResolve" /> to <typeparamref name="TRegister" />.
        /// </summary>
        /// <typeparam name="TRegister">Register type.</typeparam>
        /// <typeparam name="TResolve">Resolve type.</typeparam>
        public void Register<TRegister, TResolve>()
        {
            if (_iocMap.ContainsKey(typeof(TRegister)))
                throw new InvalidOperationException(string.Format("Type {0} already registered.",
                    typeof(TRegister).FullName));
            _iocMap.Add(typeof(TRegister), typeof(TResolve));
        }

        /// <summary>
        /// Creates a instance of <typeparamref name="TRegister" />.
        /// </summary>
        /// <typeparam name="TRegister">Register type.</typeparam>
        /// <returns>Resolved register type.</returns>
        public TRegister Resolve<TRegister>()
        {
            return (TRegister) Resolve(typeof(TRegister));
        }

        private object Resolve(Type typeToResolve)
        {
            if (!_iocMap.ContainsKey(typeToResolve))
                throw new InvalidOperationException(string.Format("Can't resolve {0}. Type is not registed.",
                    typeToResolve.FullName));
            var resolvedType = _iocMap[typeToResolve];
            // Gets first constructor 
            var constructor = resolvedType.GetConstructors().First();
            // Gets constructor parameter(s)
            var paramsInfo = constructor.GetParameters().ToList();
            // Resolves constructor parameter(s) if any
            var retObject = constructor.Invoke(paramsInfo.Select(param => param.ParameterType).Select(Resolve).ToArray());
            return retObject;
        }
    }
}