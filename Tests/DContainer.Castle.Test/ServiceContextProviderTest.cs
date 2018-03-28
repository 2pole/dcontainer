using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DContainer;
using DContainer.Castle;
using DContainer.Test;
using Xunit;

namespace DContainer.Castle.Test
{
    public class ServiceContextProviderTest : TestFixtureBase<ServiceContextProvider>
    {
        protected ServiceContextProvider Provider { get { return base.FixtureContext; } }

        public ServiceContextProviderTest()
        {
            var l = Locator.Context;
        }

        [Fact]
        public void CreateServiceContext_ByConfig()
        {
            Locator.SetContextProvider(null);
            var provider = Locator.ContextProvider;
            Assert.NotNull(provider);
        }

        [Fact]
        public void CreateServiceContext_ByNew()
        {
            var provider = new ServiceContextProvider();
            Assert.NotNull(provider);

            Locator.SetContextProvider(provider);
            Assert.Equal(provider, Locator.ContextProvider);
        }

        [Fact]
        public void CreateServiceContext()
        {
            var serviceContext = Provider.CreateServiceContext();

            Assert.NotNull(serviceContext);
            Assert.NotNull(serviceContext.Container);
        }

        [Fact]
        public void CreateChildServiceContext_NotNull()
        {
            var childContext = Provider.CreateChildServiceContext(Locator.Context);
            Assert.NotNull(childContext);
        }

        [Fact]
        public void CreateChildServiceContext_Child_Equal_Root()
        {
            var rootContext = Locator.Context;
            var childContext = Provider.CreateChildServiceContext(rootContext);
            Assert.NotEqual(rootContext, childContext);
            Assert.NotEqual(rootContext.Container, childContext.Container);
        }

        [Fact]
        public void CreateChildServiceContext_ChildContainer_In_RootContainer()
        {
            var rootContext = Locator.Context as IServiceContext<IWindsorContainer>;
            var childContext = Provider.CreateChildServiceContext(rootContext);
            Assert.Equal(childContext.Container.Parent, rootContext.Container);
        }

        [Fact]
        public void CreateServiceLocator_NotNull()
        {
            var locator = Provider.CreateServiceLocator(Locator.Context);
            Assert.NotNull(locator);
        }

        [Fact]
        public void CreateServiceRegister_NotNull()
        {
            var register = Provider.CreateServiceRegister(Locator.Context);
            Assert.NotNull(register);
        }

        [Fact]
        public void ReleaseServiceContext()
        {
            var rootContext = Locator.Context as IServiceContext<IWindsorContainer>;
            var childContext = Provider.CreateChildServiceContext(rootContext);

            Provider.ReleaseServiceContext(childContext);
            Assert.True(true);
        }

        [Fact]
        public void ReleaseServiceContext_Child_NotIn_Root()
        {
            var rootContext = Locator.Context as IServiceContext<IWindsorContainer>;
            var childContext = Provider.CreateChildServiceContext(rootContext);
            Provider.ReleaseServiceContext(childContext);
            Assert.Null(childContext.Container.Parent);
            var childInRoot = rootContext.Container.GetChildContainer(childContext.Container.Name);
            Assert.Null(childInRoot);
        }
    }
}
