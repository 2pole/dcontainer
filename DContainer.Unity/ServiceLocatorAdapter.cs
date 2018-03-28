using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace DContainer.Unity
{
    /// <summary>
    /// Translates calls from IServiceLocator to Spring.NET's IListableObjectFactory-based containers
    /// </summary>
    public class ServiceLocatorAdapter : ServiceLocatorImplBase<IUnityContainer>
    {
        public ServiceLocatorAdapter(IServiceContext<IUnityContainer> context)
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
            if (string.IsNullOrEmpty(key))
            {
                return Container.Resolve(serviceType);
            }
            return Container.Resolve(serviceType, key);
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
