using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Spring.Context;
using DContainer.Spring;
using Xunit;

namespace DContainer.Spring.Test
{
    public class SpringServiceContextProviderTest
    {
        [Fact]
        public void GetServiceContext_Test()
        {
            var contextProvider = new ServiceContextProvider();
            var serviceContext = contextProvider.CreateServiceContext();
            var container = serviceContext.Container as IApplicationContext;

            Assert.NotNull(container);
        }
    }
}
