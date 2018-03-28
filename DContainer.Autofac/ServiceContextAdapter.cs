using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DContainer.Autofac
{
    public class ServiceContextAdapter : ServiceContextImplBase<IComponentContext>
    {
        private IComponentContext _rootContainer;

        public ServiceContextAdapter(IComponentContext container)
        {
            _rootContainer = container;
        }

        protected override IComponentContext GetContainer()
        {
            return _rootContainer;
        }

        public override void Dispose()
        {
            base.Dispose();
            _rootContainer = null;
        }
    }
}
