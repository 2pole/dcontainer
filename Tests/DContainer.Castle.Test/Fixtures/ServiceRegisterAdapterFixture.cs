using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using DContainer;
using DContainer.Castle;

namespace DContainer.Castle.Test.Fixtures
{
    public class ServiceRegisterAdapterFixture : IDisposable
    {
        public ServiceRegisterAdapter ServiceRegister { get; private set; }
        public ServiceLocatorAdapter ServiceLocator { get; private set; }
        public IServiceContext<IWindsorContainer> RootServiceContext { get; private set; }
        public ServiceContextProvider ServiceContextProvider { get; private set; }

        public ServiceRegisterAdapterFixture()
        {
            var provider = new ServiceContextProvider();
            var context = provider.CreateServiceContext();

            this.ServiceContextProvider = provider;
            this.RootServiceContext = context;
            this.ServiceRegister = provider.CreateServiceRegister(context) as ServiceRegisterAdapter;
            this.ServiceLocator = provider.CreateServiceLocator(context) as ServiceLocatorAdapter;
        }

        public IServiceContext<IWindsorContainer> CreateChildServiceContext()
        {
            return ServiceContextProvider.CreateChildServiceContext(RootServiceContext);
        }

        public ServiceRegisterAdapter CreateChildServiceRegister()
        {
            var childServiceContext = this.CreateChildServiceContext();
            return ServiceContextProvider.CreateServiceRegister(childServiceContext) as ServiceRegisterAdapter;
        }

        public ServiceLocatorAdapter CreateChildServiceLocator()
        {
            var childServiceContext = this.CreateChildServiceContext();
            return ServiceContextProvider.CreateServiceLocator(childServiceContext) as ServiceLocatorAdapter;
        }

        public void Dispose()
        {
            ServiceContextProvider.ReleaseServiceContext(RootServiceContext);
        }
    }
}
