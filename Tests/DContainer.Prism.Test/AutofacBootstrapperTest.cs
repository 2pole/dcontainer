using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DContainer.Prism.Test
{
    [TestClass]
    public class AutofacBootstrapperTest : BootstrapperBase
    {
        [TestInitialize]
        public void Setup()
        {
            var provider = new Autofac.ServiceContextProvider();
            var context = provider.CreateServiceContext();
            var locator = provider.CreateServiceLocator(context);
            var register = provider.CreateServiceRegister(context);

            Locator.SetContextProvider(provider);

            register.RegisterInstanceIfMissing<IServiceContext>(context);
            register.RegisterInstanceIfMissing<IServiceRegister>(register);
            register.RegisterInstanceIfMissing<IServiceLocator>(locator);
        }
    }
}
