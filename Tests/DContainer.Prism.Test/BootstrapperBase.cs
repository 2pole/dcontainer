using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
//using NUnit.Framework;
//using Assert = NUnit.Framework.Assert;
//using StringAssert = NUnit.Framework.StringAssert;

namespace DContainer.Prism.Test
{
    [TestClass]
    public abstract class BootstrapperBase
    {
        [TestMethod]
        public void LocatorIsNotNull()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();
            var locator = bootstrapper.ServiceLocator;

            Assert.IsNotNull(locator);
        }

        [TestMethod]
        public void RegisterIsNotNull()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();
            var register = bootstrapper.ServiceRegister;

            Assert.IsNotNull(register);
        }

        [TestMethod]
        public void ConfigureContainerAddsModuleCatalogToContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            var returnedCatalog = bootstrapper.ServiceLocator.Resolve<IModuleCatalog>();
            Assert.IsNotNull(returnedCatalog);
            Assert.IsTrue(returnedCatalog is ModuleCatalog);
        }

        [TestMethod]
        public void ConfigureContainerAddsLoggerFacadeToContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            var returnedCatalog = bootstrapper.ServiceLocator.Resolve<ILoggerFacade>();
            Assert.IsNotNull(returnedCatalog);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationJournalEntryToContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationJournalEntry>();
            var actual2 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationJournalEntry>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationJournalToContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationJournal>();
            var actual2 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationJournal>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationServiceToContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationService>();
            var actual2 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationService>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsNavigationTargetHandlerToContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationContentLoader>();
            var actual2 = bootstrapper.ServiceLocator.Resolve<IRegionNavigationContentLoader>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreSame(actual1, actual2);
        }

        [TestMethod]
        public void RegisterFrameworkExceptionTypesShouldRegisterActivationException()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(typeof(ActivationException)));
        }

        //[TestMethod]
        //public void RegisterFrameworkExceptionTypesShouldRegisterResolutionFailedException()
        //{
        //    var bootstrapper = new DefaultBootstrapper();

        //    bootstrapper.CallRegisterFrameworkExceptionTypes();

        //    Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(typeof(Microsoft.Practices.Unity.ResolutionFailedException)));
        //}

        // TODO: Move to shared DLL
        protected static void AssertExceptionThrownOnRun(DContainerBootstrapper bootstrapper, Type expectedExceptionType, string expectedExceptionMessageSubstring)
        {
            bool exceptionThrown = false;
            try
            {
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                Assert.Equals(expectedExceptionType, ex.GetType());
                StringAssert.Contains(ex.Message, expectedExceptionMessageSubstring);
                exceptionThrown = true;
            }

            if (!exceptionThrown)
            {
                Assert.Fail("Exception not thrown.");
            }
        }
    }
}
