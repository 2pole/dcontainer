using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DContainer.Configuration;
using Moq;
using Xunit;

namespace DContainer.Unity.Test
{
    public class LocatorTest
    {
        private DContainerSection _locationSection;

        public LocatorTest()
        {
            _locationSection = (DContainerSection)ConfigurationManager.GetSection(DContainerSection.DContainerSectionName);
        }

        [Fact]
        public void LoadSection()
        {
            Assert.NotNull(_locationSection);
        }

        [Fact]
        public void CreateServiceContextProvider()
        {
            var provider = _locationSection.ContextProvider.CreateInstance<IServiceContextProvider>();
            Assert.NotNull(provider);
        }

        [Fact]
        public void GetServiceContext()
        {
            var provider = _locationSection.ContextProvider.CreateInstance<IServiceContextProvider>();
            var context = provider.CreateServiceContext();
            Assert.NotNull(context);
        }

        [Fact]
        public void ServiceContext_Configuration_Test()
        {
            var context = Locator.Context;
            Assert.NotNull(context);
        }

        [Fact]
        public void ServiceRegister_Configuration_Test()
        {
            var register = Locator.Register;
            Assert.NotNull(register);
        }

        [Fact]
        public void ServiceLocator_Configuration_Test()
        {
            var locator = Locator.Root;
            Assert.NotNull(locator);
        }
    }
}
