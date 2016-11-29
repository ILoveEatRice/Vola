﻿using System;
using System.Collections.Generic;
using AvalonAssets.Algorithm.Injection.Exception;
using AvalonAssets.DataStructure;

namespace AvalonAssets.Algorithm.Injection
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, NullableDictionary<string, IInjectionConstructor>> _iocMap;

        /// <summary>
        ///     Creates a new <see cref="IocContainer" />
        /// </summary>
        public Container()
        {
            _iocMap = new Dictionary<Type, NullableDictionary<string, IInjectionConstructor>>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IContainer RegisterType(Type request, Type @return, IInjectionConstructor constructor, string name)
        {
            if (!_iocMap.ContainsKey(request))
                _iocMap[request] = new NullableDictionary<string, IInjectionConstructor>();
            if (constructor == null)
                constructor = new TypeConstructor(@return);
            _iocMap[request][name] = constructor;
            return this;
        }

        public IContainer RegisterInstance(Type request, object instance, string name)
        {
            return RegisterType(request, instance.GetType(), new InstanceConstructor(instance), name);
        }

        public object Resolve(Type request, string name, IDictionary<string, object> arguments)
        {
            if(!_iocMap.ContainsKey(request))
                throw new TypeNotRegisteredException(request);
           return _iocMap[request][name].NewInstance(this, arguments);
        }

        public IEnumerable<object> ResolveAll(Type request, IDictionary<string, object> arguments)
        {
            if (!_iocMap.ContainsKey(request))
                throw new TypeNotRegisteredException(request);
            foreach (var constructor in _iocMap[request].Values)
            {
                yield return constructor.NewInstance(this, arguments);
            }
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