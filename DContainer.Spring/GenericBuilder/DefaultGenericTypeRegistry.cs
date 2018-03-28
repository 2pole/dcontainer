using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace DContainer.Spring.GenericBuilder
{
    public class DefaultGenericTypeRegistry : IGenericTypeRegistry, IObjectFactoryAware
    {
        private static readonly NamedTypesRegistry NamedTypesRegistry = new NamedTypesRegistry();

        public bool IsRegistered(Type fromType, string key)
        {
            AssertUtils.ArgumentNotNull(fromType, "fromType");
            if (!fromType.IsGenericType)
                return false;

            if (!fromType.IsGenericTypeDefinition)
                fromType = fromType.GetGenericTypeDefinition();

            return NamedTypesRegistry.Contains(fromType, key);
        }

        public void RegisterGenericTypeDefinition(Type fromType, Type toType, string key)
        {
            AssertUtils.ArgumentNotNull(fromType, "fromType");
            if (toType == null)
                toType = fromType;

            if (!fromType.IsGenericTypeDefinition)
                throw new ArgumentException(string.Format("The type [{0}] must be a generic.", fromType.FullName));

            if (!toType.IsGenericTypeDefinition)
                throw new ArgumentException(string.Format("The type [{0}] must be a generic.", toType.FullName));

            if (string.IsNullOrEmpty(key))
                key = null;

            NamedTypesRegistry.RegisterType(fromType, toType, key);
        }

        public object Resolve(Type serviceType, string key)
        {
            AssertUtils.ArgumentNotNull(serviceType, "serviceType");
            if(serviceType.IsGenericTypeDefinition)
                throw new ArgumentException(string.Format("The type [{0}] cannot a gereric definition type.", serviceType.FullName));

            if(string.IsNullOrEmpty(key))
                key = null;
            
            var originalType = serviceType.GetGenericTypeDefinition();
            var mappedType = NamedTypesRegistry.GetMappedType(originalType, key);
            if(mappedType == null)
                throw new InvalidOperationException(string.Format("The type [{0}] didn't register.", serviceType.FullName));
            mappedType = mappedType.MakeGenericType(serviceType.GetGenericArguments());

            var objectDefinition = GetObjectDefinitionOrDefault(mappedType, originalType.FullName);
            var genericObjectName = serviceType.FullName;
            var singletonRegistry = SingletonObjectRegistry;
            if (objectDefinition.IsSingleton && singletonRegistry.ContainsSingleton(genericObjectName))
                return singletonRegistry.GetSingleton(genericObjectName);

            var checkingMode = GetDependencyCheckingMode(objectDefinition);
            var instance = AutowireObjectFactory.Autowire(mappedType, objectDefinition.AutowireMode, checkingMode != DependencyCheckingMode.None);
            if (objectDefinition.IsSingleton && instance != null)
                singletonRegistry.RegisterSingleton(genericObjectName, instance);
            return instance;
        }

        private DependencyCheckingMode GetDependencyCheckingMode(IObjectDefinition objectDefinition)
        {
            var configurableDefinition = objectDefinition as IConfigurableObjectDefinition;
            if (configurableDefinition != null)
                return configurableDefinition.DependencyCheck;
            else
                return DependencyCheckingMode.None;
        }

        private IObjectDefinition GetObjectDefinitionOrDefault(Type objectType, string objectName)
        {
            var registry = ObjectDefinitionRegistry;
            IObjectDefinition objectDefinition = null;
            if (registry != null)
                objectDefinition = registry.GetObjectDefinition(objectName);
            if (objectDefinition == null)
                objectDefinition = new RootObjectDefinition(objectType, AutoWiringMode.Constructor);
            else
                objectDefinition = new RootObjectDefinition(objectDefinition) { ObjectType = objectType };
            return objectDefinition;
        }

        private ISingletonObjectRegistry SingletonObjectRegistry
        {
            get { return ObjectFactory as ISingletonObjectRegistry; }
        }

        private IAutowireCapableObjectFactory AutowireObjectFactory
        {
            get { return ObjectFactory as IAutowireCapableObjectFactory; }
        }

        private IObjectDefinitionRegistry ObjectDefinitionRegistry
        {
            get { return ObjectFactory as IObjectDefinitionRegistry; }
        }

        #region Implementation of IObjectFactoryAware

        /// <summary>
        /// Callback that supplies the owning factory to an object instance.
        /// </summary>
        /// <value>
        /// Owning <see cref="Spring.Objects.Factory.IObjectFactory"/>
        /// (may not be <see langword="null"/>). The object can immediately
        /// call methods on the factory.
        /// </value>
        /// <remarks>
        /// <p>
        /// Invoked after population of normal object properties but before an init
        /// callback like <see cref="Spring.Objects.Factory.IInitializingObject"/>'s
        /// <see cref="Spring.Objects.Factory.IInitializingObject.AfterPropertiesSet"/>
        /// method or a custom init-method.
        /// </p>
        /// </remarks>
        /// <exception cref="Spring.Objects.ObjectsException">
        /// In case of initialization errors.
        /// </exception>
        public IObjectFactory ObjectFactory { get; set; }

        #endregion
    }
}
