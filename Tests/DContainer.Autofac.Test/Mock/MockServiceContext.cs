using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Autofac.Test.Mock
{
    public class MockServiceContext : IServiceContext
    {
        public object Container { get; set; }

        public object CurrentContainer { get; set; }

        public void Dispose()
        {
        }
    }
}
