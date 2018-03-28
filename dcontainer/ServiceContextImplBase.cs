using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public abstract class ServiceContextImplBase : IServiceContext
    {
        public virtual object Container { get { return this.GetContainer(); } }

        protected abstract object GetContainer();

        public virtual void Dispose()
        {
            var disposable = Container as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }

    public abstract class ServiceContextImplBase<TContainer> : IServiceContext<TContainer>
        where TContainer : class
    {
        protected abstract TContainer GetContainer();

        public virtual TContainer Container
        {
            get { return this.GetContainer(); }
        }

        object IServiceContext.Container
        {
            get { return this.Container; }
        }

        public virtual void Dispose()
        {
            var disposable = Container as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
