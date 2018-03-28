using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DContainer.Castle.Test.Fixtures;
using DContainer.Castle.Test.Mock;
using DContainer;
using DContainer.Castle;
using DContainer.Test;
using Xunit;

namespace DContainer.Castle.Test
{
    public class ServiceRegisterAdapterTest : TestFixtureBase<ServiceRegisterAdapterFixture>, IDisposable
    {
        public ServiceRegisterAdapter ServiceRegister { get { return base.FixtureContext.ServiceRegister; } }
        public ServiceLocatorAdapter ServiceLocator { get { return base.FixtureContext.ServiceLocator; } }
        public IWindsorContainer RootContainer { get { return base.FixtureContext.RootServiceContext.Container; } }

        [Fact]
        public void Register_PerContainer()
        {
            //Assert.DoesNotThrow(() => ServiceRegister.RegisterType<IUser, User>(LifetimeScope.PerContainer));
            Assert.NotNull(ServiceRegister.RegisterType<IUser, User>(LifetimeScope.PerContainer));
        }

        [Fact]
        public void Resolve_PerContainer()
        {
            ServiceRegister.RegisterTypeIfMissing<IUser, User>(LifetimeScope.PerContainer);

            var locator = base.FixtureContext.CreateChildServiceLocator();
            //Assert.DoesNotThrow(() => locator.Resolve<IUser>());
            Assert.NotNull(locator.Resolve<IUser>());
        }

        [Fact]
        public void Resolve_PerContainer_NotEqual()
        {
            ServiceRegister.RegisterTypeIfMissing<IUser, User>(LifetimeScope.PerContainer);

            var locator1 = base.FixtureContext.CreateChildServiceLocator();
            var locator2 = base.FixtureContext.CreateChildServiceLocator();
            var u = ServiceLocator.Resolve<IUser>();
            var u1 = locator1.Resolve<IUser>();
            var u2 = locator2.Resolve<IUser>();

            Assert.NotEqual(u, u1);
            Assert.NotEqual(u, u2);
            Assert.NotEqual(u1, u2);
        }
    }
}
