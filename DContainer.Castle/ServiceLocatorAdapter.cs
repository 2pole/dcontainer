using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;

namespace DContainer.Castle
{
    /// <summary>
    /// Translates calls from IServiceLocator to Spring.NET's IListableObjectFactory-based containers
    /// </summary>
    public class ServiceLocatorAdapter : ServiceLocatorImplBase<IWindsorContainer>
    {
        public ServiceLocatorAdapter(IServiceContext<IWindsorContainer> context)
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
            return string.IsNullOrWhiteSpace(key) ? 
                    Container.Resolve(serviceType) : 
                    Container.Resolve(key, serviceType);
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
            return Container.ResolveAll(serviceType).Cast<object>();
        }
    }
}
