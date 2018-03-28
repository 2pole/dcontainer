using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Moq;
using Autofac;
using DContainer.Autofac;
using DContainer.Autofac.Test.Mock;
using Xunit;

namespace DContainer.Autofac.Test
{
    public class ContainerUseFixture
    {
        public IContainer Container { get; private set; }
        public ContainerUseFixture()
        {
            this.Container = new ContainerBuilder().Build();
        }
    }

    public class IContainerExtensionsTest : TestFixtureBase<ContainerUseFixture>
    {
        public IContainer Container { get { return base.FixtureData.Container; } }

        [Fact]
        public void RegisterInstance()
        {
            Mock<IServiceContext> m1 = new Mock<IServiceContext>();
            Container.RegisterInstance<IServiceContext>(m1.Object);

            var service = Container.Resolve<IServiceContext>();
            Assert.NotNull(service);
            Assert.Equal(m1.Object, service);
        }

        [Fact]
        public void RegisterTypeWithoutName()
        {
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), null, LifetimeScope.Transient);
            var service = Container.Resolve<IServiceContext>();
            Assert.Equal(service.GetType(), typeof(MockServiceContext));
        }

        [Fact]
        public void RegisterTypeWithName()
        {
            var name = "Test";
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), name, LifetimeScope.Transient);
            var service = Container.ResolveNamed<IServiceContext>(name);
            Assert.Equal(service.GetType(), typeof(MockServiceContext));
        }

        [Fact]
        public void RegisterTypeWithoutNameSingleton()
        {
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), null, LifetimeScope.Singleton);
            var service = Container.Resolve<IServiceContext>();
            var service2 = Container.Resolve<IServiceContext>();
            Assert.Equal(service, service2);
        }

        [Fact]
        public void RegisterTypeWithNameSingleton()
        {
            var name = "Test";
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), name, LifetimeScope.Singleton);
            var service = Container.ResolveNamed<IServiceContext>(name);
            var service2 = Container.ResolveNamed<IServiceContext>(name);
            Assert.Equal(service, service2);
        }

        [Fact]
        public void ResolveAllByType()
        {
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), Guid.NewGuid().ToString(), LifetimeScope.Transient);
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), Guid.NewGuid().ToString(), LifetimeScope.Transient);

            var items = Container.ResolveAll(typeof(IServiceContext)).ToList();
            Assert.NotEmpty(items);
        }

        [Fact]
        public void ResolveAllByGeneric()
        {
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), Guid.NewGuid().ToString(), LifetimeScope.Transient);
            Container.RegisterType(typeof(IServiceContext), typeof(MockServiceContext), Guid.NewGuid().ToString(), LifetimeScope.Transient);

            var items = Container.ResolveAll<IServiceContext>().ToList();
            Assert.NotEmpty(items);
        }

        [Fact]
        public void RegisterGeneric()
        {
            var fromType = typeof(IUser<>);
            var toType = typeof(User<>);
            Container.RegisterType(fromType, toType, "R1", LifetimeScope.Transient);
        }

        [Fact]
        public void ResolveGeneric()
        {
            var fromType = typeof(IUser<>);
            var toType = typeof(User<>);
            Container.RegisterType(fromType, toType, "R2", LifetimeScope.Transient);
            Container.ResolveNamed<IUser<int>>("R2");
        }

        public interface IUser<T>
        {
        }

        public class User<T> : IUser<T>
        {
        }
    }
}
