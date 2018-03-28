using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace DContainer.Unity
{
    public class ServiceContextProvider : ServiceContextProviderImplBase<IUnityContainer>
    {
        protected virtual void ConfigureContainerCore(IUnityContainer container)
        {
        }

        public override IServiceRegister CreateServiceRegister(IServiceContext<IUnityContainer> context)
        {
            var register = new ServiceRegisterAdapter(context);
            return register;
        }

        public override void ReleaseServiceContext(IServiceContext<IUnityContainer> context)
        {
            context.Container.Dispose();
        }

        public override IServiceContext<IUnityContainer> CreateChildServiceContext(IServiceContext<IUnityContainer> context)
        {
            var childContainer = context.Container.CreateChildContainer();
            return new ServiceContextAdapter(childContainer);
        }

        public override IServiceLocator CreateServiceLocator(IServiceContext<IUnityContainer> context)
        {
            var locator = new ServiceLocatorAdapter(context);
            return locator;
        }

        public override IServiceContext<IUnityContainer> CreateServiceContext()
        {
            IUnityContainer container = new UnityContainer();
            ConfigureContainerCore(container);
            var serviceContext = new ServiceContextAdapter(container);
            return serviceContext;
        }
    }
}
