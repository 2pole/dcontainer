using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DContainer.TestBase.Mock;
using Moq;

namespace DContainer.Test.Fixtures
{
    public class ContainerFixture
    {
        public IServiceLocator ServiceLocator { get; private set; }
        public IServiceRegister ServiceRegister { get; private set; }

        public ContainerFixture()
        {
            var u1 = new User();
            var p1 = new Person();
            var r1 = new Role();

            var locatorMock = new Mock<IServiceLocator>();
            var registerMock = new Mock<IServiceRegister>();

            registerMock.Setup(m => m.IsRegistered<IRole>()).Returns(true);
            registerMock.Setup(m => m.IsRegistered<IUser>()).Returns(true);
            registerMock.Setup(m => m.IsRegistered<IPerson>()).Returns(true);
            registerMock.Setup(m => m.IsRegistered(typeof(IRole))).Returns(true);
            registerMock.Setup(m => m.IsRegistered(typeof(IUser))).Returns(true);
            registerMock.Setup(m => m.IsRegistered(typeof(IPerson))).Returns(true);

            locatorMock.Setup(m => m.Resolve<IUser>()).Returns(u1);
            locatorMock.Setup(m => m.Resolve<IPerson>()).Returns(p1);
            locatorMock.Setup(m => m.Resolve<IRole>()).Returns(r1);
            locatorMock.Setup(m => m.Resolve<IServiceRegister>()).Returns(registerMock.Object);

            ServiceLocator = locatorMock.Object;
            ServiceRegister = registerMock.Object;
            //Locator.SetServiceLocator(ServiceLocator);
            //Locator.SetServiceRegister(ServiceRegister);
        }
    }
}
