using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Conversion;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace DContainer.Castle
{
    public class ServiceContextProvider : ServiceContextProviderImplBase<IWindsorContainer>
    {
        protected virtual void ConfigureContainerCore(IWindsorContainer container)
        {   
        }

        public override IServiceRegister CreateServiceRegister(IServiceContext<IWindsorContainer> context)
        {
            var register = new ServiceRegisterAdapter(context);
            return register;
        }

        public override IServiceLocator CreateServiceLocator(IServiceContext<IWindsorContainer> context)
        {
            var locator = new ServiceLocatorAdapter(context);
            return locator;
        }

        public override IServiceContext<IWindsorContainer> CreateServiceContext()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            this.ConfigureContainerCore(container);

            ServiceContextAdapter context = new ServiceContextAdapter(container);
            return context;
        }

        public override IServiceContext<IWindsorContainer> CreateChildServiceContext(
            IServiceContext<IWindsorContainer> context)
        {
            var childWindsor = new WindsorContainer();
            context.Container.AddChildContainer(childWindsor);
            
            var childContext = new ServiceContextAdapter(childWindsor);
            return childContext;
        }

        public override void ReleaseServiceContext(IServiceContext<IWindsorContainer> context)
        {
            context.Container.Dispose();
        }
    }
}
