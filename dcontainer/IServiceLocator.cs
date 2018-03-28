using System;
using System.Collections;
using System.Collections.Generic;

namespace DContainer
{
    /// <summary>
    /// The generic Service Locator interface. This interface is used
    /// to retrieve services (instances identified by type and optional
    /// name) from a container.
    /// </summary>
    public interface IServiceLocator : IServiceProvider, IDisposable
    {
        IServiceContext ServiceContext { get; }

        TService Resolve<TService>();

        TService Resolve<TService>(string key);

        object Resolve(Type serviceType);

        object Resolve(Type serviceType, string key);

        IEnumerable<object> ResolveAll(Type serviceType);

        IEnumerable<TService> ResolveAll<TService>(); 
    }
}
