using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public static class ResolutionExtensions
    {
        /// <summary>
        /// Set any properties on <paramref name="instance"/> that can be
        ///             resolved in the context.
        /// 
        /// </summary>
        /// <typeparam name="TService">Type of instance. Used only to provide method chaining.</typeparam><param name="locator">The context from which to resolve the service.</param><param name="instance">The instance to inject properties into.</param>
        /// <returns>
        /// <paramref name="instance"/>.
        /// </returns>
        public static TService InjectProperties<TService>(this IServiceLocator locator, TService instance)
        {
            AutowiringPropertyInjector.InjectProperties(locator, instance, true);
            return instance;
        }

        /// <summary>
        /// Set any null-valued properties on <paramref name="instance"/> that can be
        ///             resolved by the container.
        /// 
        /// </summary>
        /// <typeparam name="TService">Type of instance. Used only to provide method chaining.</typeparam><param name="context">The context from which to resolve the service.</param>
        /// <param name="locator"></param>
        /// <param name="instance">The instance to inject properties into.</param>
        /// <returns>
        /// <paramref name="instance"/>.
        /// </returns>
        public static TService InjectUnsetProperties<TService>(this IServiceLocator locator, TService instance)
        {
            AutowiringPropertyInjector.InjectProperties(locator, instance, false);
            return instance;
        }
    }
}
