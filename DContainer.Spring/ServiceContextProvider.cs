using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DContainer.Spring.GenericBuilder;
using Spring.Context;
using Spring.Context.Support;

namespace DContainer.Spring
{
    public class ServiceContextProvider : ServiceContextProviderImplBase<IApplicationContext>
    {
        protected virtual void ConfigureContainerCore(IApplicationContext container)
        {
        }

        public override IServiceRegister CreateServiceRegister(IServiceContext<IApplicationContext> context)
        {
            var register = new ServiceRegisterAdapter(context);
            return register;
        }

        public override IServiceLocator CreateServiceLocator(IServiceContext<IApplicationContext> context)
        {
            var locator = new ServiceLocatorAdapter(context);
            return locator;
        }

        public override IServiceContext<IApplicationContext> CreateServiceContext()
        {
            var context = ContextRegistry.GetContext();
            context.RegisterType(typeof(IGenericTypeRegistry), typeof(DefaultGenericTypeRegistry), null, LifetimeScope.Singleton);
            ConfigureContainerCore(context);

            var serviceContext = new ServiceContextAdapter(context);
            return serviceContext;
        }

        public override IServiceContext<IApplicationContext> CreateChildServiceContext(IServiceContext<IApplicationContext> context)
        {
            var contextName = Guid.NewGuid().ToString();
            var childContext = ContextRegistry.GetContext(contextName);

            var serviceContext = new ServiceContextAdapter(childContext);
            return serviceContext;
        }

        public override void ReleaseServiceContext(IServiceContext<IApplicationContext> context)
        {
            context.Container.Dispose();
        }
    }
}
