using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Xunit;

namespace DContainer.Spring.Test
{
    public class SpringServiceLocatorAdapterTest : TestFixtureBase<ServiceLocatorFixture>
    {
        [Fact]
        public void GetInstance_Test()
        {
            var service = base.FixtureData.ServiceLocator.Resolve<IAliasGenerator>();
            Assert.NotNull(service);
        }

        [Fact]
        public void GetAllInstances_Test()
        {
            var services = base.FixtureData.ServiceLocator.ResolveAll<IAliasGenerator>().ToList();
            Assert.NotEmpty(services);
        }
    }

    public class ServiceLocatorFixture
    {
        public IServiceContext<IApplicationContext> ServiceContext { get; private set; }
        public IServiceLocator ServiceLocator { get; private set; }

        public ServiceLocatorFixture()
        {
            var accessor = new ServiceContextProvider();
            ServiceContext = accessor.CreateServiceContext();
            ServiceLocator = new ServiceLocatorAdapter(ServiceContext);
            var container = ServiceContext.Container as IApplicationContext;
            container.RegisterType(typeof(IAliasGenerator), typeof(DefaultAliasGenerator), null, LifetimeScope.Singleton);
        }
    }
}
