using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public interface IServiceContextProvider
    {
        IServiceContext CreateServiceContext();

        IServiceContext CreateChildServiceContext(IServiceContext context);

        IServiceLocator CreateServiceLocator(IServiceContext context);

        IServiceRegister CreateServiceRegister(IServiceContext context);

        void ReleaseServiceContext(IServiceContext serviceContext);
    }

    public interface IServiceContextProvider<TContainer> : IServiceContextProvider
        where TContainer : class
    {
        IServiceContext<TContainer> CreateServiceContext();

        IServiceContext<TContainer> CreateChildServiceContext(IServiceContext<TContainer> context);

        IServiceLocator CreateServiceLocator(IServiceContext<TContainer> context);

        IServiceRegister CreateServiceRegister(IServiceContext<TContainer> context);

        void ReleaseServiceContext(IServiceContext<TContainer> context);
    }
}
