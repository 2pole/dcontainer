using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Unity;
using Xunit;

namespace DContainer.Unity.Test
{
    public class ServiceRegisterAdapterTest
    {
        private IServiceContext<IUnityContainer> ServiceContext;
        private IServiceRegister ServiceRegister;
        private IUnityContainer Container;

        [Fact]
        public void SetUp()
        {
            var accessor = new ServiceContextProvider();
            ServiceContext = accessor.CreateServiceContext();
            ServiceRegister = new ServiceRegisterAdapter(ServiceContext);
            Container = ServiceContext.Container as IUnityContainer;
        }
    }
}
