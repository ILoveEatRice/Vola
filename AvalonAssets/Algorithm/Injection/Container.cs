using System;
using System.Collections.Generic;
using AvalonAssets.Algorithm.Injection.Constructor;
using AvalonAssets.Algorithm.Injection.Exception;
using AvalonAssets.DataStructure;

namespace AvalonAssets.Algorithm.Injection
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, NullableDictionary<string, IConstructor>> _iocMap;

        /// <summary>
        ///     Creates a new <see cref="Container" />.
        /// </summary>
        public Container()
        {
            _iocMap = new Dictionary<Type, NullableDictionary<string, IConstructor>>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IContainer RegisterType(Type request, Type @return, IConstructor constructor, string name)
        {
            if (!IsRegistered(request))
                _iocMap[request] = new NullableDictionary<string, IConstructor>();
            if (constructor == null)
                constructor = Constructors.Type(@return);
            _iocMap[request][name] = constructor;
            return this;
        }

        public IContainer RegisterInstance(Type request, object instance, string name)
        {
            return RegisterType(request, instance.GetType(), Constructors.Instance(instance), name);
        }

        public object Resolve(Type request, string name, IDictionary<string, object> arguments)
        {
            if (!IsRegistered(request))
                throw new TypeNotRegisteredException(request);
            if (!_iocMap[request].ContainsKey(name))
                throw new NameNotResolveException(name);
            return _iocMap[request][name].NewInstance(this, arguments);
        }

        public IEnumerable<object> ResolveAll(Type request, IDictionary<string, object> arguments)
        {
            if (!IsRegistered(request))
                throw new TypeNotRegisteredException(request);
            foreach (var constructor in _iocMap[request].Values)
                yield return constructor.NewInstance(this, arguments);
        }

        public bool IsRegistered(Type request)
        {
            return _iocMap.ContainsKey(request);
        }

        protected virtual void Dispose(bool disposing)
        {
            // free native resources if there are any.
            if (!disposing) return;
            foreach (var dictonary in _iocMap.Values)
            {
                foreach (var constructor in dictonary.Values)
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    var disposable = constructor as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
            }
        }
    }
}