using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DContainer.Test.Fixtures;
using DContainer.TestBase.Mock;
using Moq;
using Xunit;

namespace DContainer.Test
{
    public class AutowiringPropertyInjectorTest : TestFixtureBase<ContainerFixture>
    {
        [Fact]
        public void InjectProperties()
        {
            var user = new User();
            AutowiringPropertyInjector.InjectProperties(
                base.FixtureContext.ServiceLocator,
                user,
                false);
            Assert.NotNull(user.Person);
        }

        [Fact]
        public void InjectProperties_Override_False()
        {
            var user = new User();
            var person = new Person();
            user.Person = person;

            AutowiringPropertyInjector.InjectProperties(
                base.FixtureContext.ServiceLocator,
                user,
                false);

            Assert.NotNull(user.Person);
            Assert.Equal(user.Person, person);
        }

        [Fact]
        public void InjectProperties_Override_True()
        {
            var user = new User();
            var person = new Person();
            user.Person = person;

            AutowiringPropertyInjector.InjectProperties(
                base.FixtureContext.ServiceLocator,
                user,
                true);

            Assert.NotEqual(user.Person, person);
        }
    }
}
