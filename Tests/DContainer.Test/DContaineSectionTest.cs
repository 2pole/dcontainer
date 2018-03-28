using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DContainer.Configuration;
using Xunit;

namespace DContainer.Test
{
    public class DContainerSectionTest
    {
        [Fact]
        public void LoadConfiguration_Test()
        {
            var section = (DContainerSection)ConfigurationManager.GetSection(DContainerSection.DContainerSectionName);
            Assert.NotNull(section);
        }
    }
}
