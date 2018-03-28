using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public abstract class ServiceContextProviderImplBase<TContainer> : IServiceContextProvider<TContainer>
        where TContainer : class
    {
        #region IServiceContextProvider
        IServiceContext IServiceContextProvider.CreateServiceContext()
        {
            return this.CreateServiceContext();
        }

        public IServiceContext CreateChildServiceContext(IServiceContext context)
        {
            return CreateChildServiceContext((IServiceContext<TContainer>)context);
        }

        public IServiceRegister CreateServiceRegister(IServiceContext context)
        {
            return CreateServiceRegister((IServiceContext<TContainer>)context);
        }

        public IServiceLocator CreateServiceLocator(IServiceContext context)
        {
            return CreateServiceLocator((IServiceContext<TContainer>)context);
        }

        public void ReleaseServiceContext(IServiceContext context)
        {
            ReleaseServiceContext((IServiceContext<TContainer>)context);
        }
        #endregion

        #region IServiceContextProvider<TContainer>

        public abstract IServiceRegister CreateServiceRegister(IServiceContext<TContainer> context);

        public abstract IServiceLocator CreateServiceLocator(IServiceContext<TContainer> context);

        public abstract IServiceContext<TContainer> CreateServiceContext();

        public abstract IServiceContext<TContainer> CreateChildServiceContext(IServiceContext<TContainer> context);
     
        public abstract void ReleaseServiceContext(IServiceContext<TContainer> context);

        #endregion
    }
}
