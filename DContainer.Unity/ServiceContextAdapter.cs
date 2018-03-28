using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace DContainer.Unity
{
    public class ServiceContextAdapter : ServiceContextImplBase<IUnityContainer>
    {
        private readonly IUnityContainer _rootContainer;

        public ServiceContextAdapter(IUnityContainer container)
        {
            _rootContainer = container;
        }

        protected override IUnityContainer GetContainer()
        {
            return _rootContainer;
        }
    }
}
