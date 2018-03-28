using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DContainer.Spring.GenericBuilder;
using Spring.Context;
using Spring.Context.Support;

namespace DContainer.Spring.Test.Mock
{
    public class ConfigurableContextFixture
    {
        public IConfigurableApplicationContext Context { get; private set; }

        public ConfigurableContextFixture()
        {
            this.Context = ContextRegistry.GetContext() as IConfigurableApplicationContext;
            this.Context.RegisterType(typeof(IGenericTypeRegistry), typeof(DefaultGenericTypeRegistry), null, LifetimeScope.Singleton);
        }
    }
}
