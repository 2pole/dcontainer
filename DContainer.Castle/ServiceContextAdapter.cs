using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;

namespace DContainer.Castle
{
    public class ServiceContextAdapter : ServiceContextImplBase<IWindsorContainer>
    {
        private IWindsorContainer _rootContainer;

        public ServiceContextAdapter(IWindsorContainer container)
        {
            _rootContainer = container;
        }

        protected override IWindsorContainer GetContainer()
        {
            return _rootContainer;
        }
    }
}
