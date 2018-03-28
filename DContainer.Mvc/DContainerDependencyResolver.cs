using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DContainer.Mvc
{
    public class DContainerDependencyResolver : IDependencyResolver
    {
        public IServiceLocator ChildServiceLocator
        {
            get { return RequestLifetimeHttpModule.ChildServiceLocator; }
        }

        #region IDependencyResolver
        public virtual object GetService(Type serviceType)
        {
            try
            {
                return ChildServiceLocator.Resolve(serviceType);
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            return ChildServiceLocator.ResolveAll(serviceType);
        }
        #endregion
    }
}
