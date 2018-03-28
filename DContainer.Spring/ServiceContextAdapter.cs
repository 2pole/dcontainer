using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;

namespace DContainer.Spring
{
    public class ServiceContextAdapter : ServiceContextImplBase<IApplicationContext>, IDisposable
    {
        private readonly IApplicationContext _rootContext;

        public ServiceContextAdapter(IApplicationContext context)
        {
            _rootContext = context;
        }

        protected override IApplicationContext GetContainer()
        {
            return ContextRegistry.GetContext();
        }

        public void Dispose()
        {
            _rootContext.Dispose();
            ContextRegistry.Clear();
        }
    }
}
