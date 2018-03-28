using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DContainer.Autofac
{
    /// <summary>
    /// Translates calls from IServiceLocator to Spring.NET's IListableObjectFactory-based containers
    /// </summary>
    public class ServiceLocatorAdapter : ServiceLocatorImplBase<IComponentContext>
    {
        public ServiceLocatorAdapter(IServiceContext<IComponentContext> context)
            : base(context)
        {
        }

        /// <summary>
        /// Resolves a requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// The requested service instance or null, if <paramref name="key"/> is not found.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            object instance = Container.ResolveOptional(serviceType, key);
            if (instance == null && serviceType.CanResolve())
            {  
                Container.RegisterType(serviceType, key, LifetimeScope.Transient);
                instance = Container.ResolveRequired(serviceType, key);
            }

            return instance;
        }

        /// <summary>
        /// Resolves service instances by type.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects matching the <paramref name="serviceType"/>.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return Container.ResolveAll(serviceType);
        }
    }
}
