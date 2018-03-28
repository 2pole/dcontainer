using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DContainer.Spring.GenericBuilder;
using DContainer.Spring.Test.Mock;
using Spring.Context;
using Spring.Context.Support;
using Xunit;

namespace DContainer.Spring.Test
{
    public class IApplicationContextExtensionsTest : TestFixtureBase<ConfigurableContextFixture>
    {
        private IConfigurableApplicationContext Context { get { return base.FixtureData.Context; } }

        [Fact]
        public void ResolveGenericTypeRegistry()
        {
            var typeRegistry1 = Context.Resolve<IGenericTypeRegistry>();
            var typeRegistry2 = Context.Resolve<IGenericTypeRegistry>();

            Assert.NotNull(typeRegistry1);
            Assert.NotNull(typeRegistry2);
            Assert.Same(typeRegistry1, typeRegistry2);
            Assert.NotNull(((DefaultGenericTypeRegistry)typeRegistry1).ObjectFactory);
        }

        [Fact]
        public void RegisterGenericType()
        {
            Context.RegisterType(typeof(IList<>), typeof(List<>), null, LifetimeScope.Transient);
            Assert.True(true);
        }

        [Fact]
        public void ResolveGenericType()
        {
            Context.RegisterType(typeof(IDictionary<,>), typeof(Dictionary<,>), null, LifetimeScope.Transient);
            var map = Context.Resolve<IDictionary<string, string>>();
            Assert.NotNull(map);
        }
    }
}
