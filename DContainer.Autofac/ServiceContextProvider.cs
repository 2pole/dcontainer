using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators.ProvidedInstance;

namespace DContainer.Autofac
{
    public class ServiceContextProvider : ServiceContextProviderImplBase<IComponentContext>
    {
        protected virtual void ConfigureContainerCore(IComponentContext container)
        {
        }

        public override IServiceRegister CreateServiceRegister(IServiceContext<IComponentContext> context)
        {
            var register = new ServiceRegisterAdapter(context);
            return register;
        }

        public override IServiceLocator CreateServiceLocator(IServiceContext<IComponentContext> context)
        {
            var locator = new ServiceLocatorAdapter(context);
            return locator;
        }

        public override IServiceContext<IComponentContext> CreateServiceContext()
        {
            ContainerBuilder builder = new ContainerBuilder();
            var container = builder.Build();
            this.ConfigureContainerCore(container);

            ServiceContextAdapter context = new ServiceContextAdapter(container);
            return context;
        }

        public override IServiceContext<IComponentContext> CreateChildServiceContext(IServiceContext<IComponentContext> context)
        {
            var scope = context.Container as ILifetimeScope;
            var lifetimeScope = scope.BeginLifetimeScope();
            var childContext = new ServiceContextAdapter(lifetimeScope);
            return childContext;
        }

        public override void ReleaseServiceContext(IServiceContext<IComponentContext> context)
        {
            var disposable = context.Container as IDisposable;
            if(disposable != null)
                disposable.Dispose();
        }
    }
}
