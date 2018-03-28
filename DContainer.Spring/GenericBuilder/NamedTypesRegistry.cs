using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Spring.GenericBuilder
{
    internal class NamedTypesRegistry
    {
        private readonly Dictionary<Type, List<string>> registeredKeys;
        private readonly Dictionary<NamedTypeBuildKey, Type> registeredTypes; 

        public NamedTypesRegistry()
            : this(null)
        {
        }

        public NamedTypesRegistry(NamedTypesRegistry parent)
        {
            registeredKeys = new Dictionary<Type, List<string>>();
            registeredTypes = new Dictionary<NamedTypeBuildKey, Type>();
        }

        public void RegisterType(Type fromType, Type toType, string name)
        {
            if (!registeredKeys.ContainsKey(fromType))
            {
                registeredKeys[fromType] = new List<string>();
            }

            RemoveMatchingKeys(fromType, name);
            registeredKeys[fromType].Add(name);
            var buildKey = new NamedTypeBuildKey(fromType, name);
            registeredTypes[buildKey] = toType;
        }

        public Type GetMappedType(Type type, string name)
        {
            var buildKey = new NamedTypeBuildKey(type, name);
            if (registeredTypes.ContainsKey(buildKey))
                return registeredTypes[buildKey];
                
            return null;
        }

        public bool Contains(Type t, string name)
        {
            if(registeredKeys.ContainsKey(t))
            {
                var keys = registeredKeys[t];
                return keys.Contains(name);
            }
            return false;
        }

        public IEnumerable<string> GetKeys(Type t)
        {
            var keys = Enumerable.Empty<string>();

            if (registeredKeys.ContainsKey(t))
            {
                keys = keys.Concat(registeredKeys[t]);
            }

            return keys;
        }

        public IEnumerable<Type> RegisteredTypes
        {
            get { return registeredKeys.Keys; }
        }

        public void Clear()
        {
            registeredKeys.Clear();
        }

        // We need to do this the long way - Silverlight doesn't support List<T>.RemoveAll(Predicate)
        private void RemoveMatchingKeys(Type t, string name)
        {
            var uniqueNames = from registeredName in registeredKeys[t]
                              where registeredName != name
                              select registeredName;

            registeredKeys[t] = uniqueNames.ToList();
        }
    }
}
