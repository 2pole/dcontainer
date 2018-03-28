using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Dependencies;

namespace DContainer.WebApi
{
    public class WebApiDependencyResolver : WebApiDependencyScope, IDependencyResolver
    {
        private IServiceLocator _rootLocator;

        public IServiceLocator ServiceLocator
        {
            get { return _rootLocator; }
        }

        public WebApiDependencyResolver()
            : this(Locator.Root)
        {
        }

        public WebApiDependencyResolver(IServiceLocator rootLocator)
            : base(rootLocator)
        {
        }

        public IDependencyScope BeginScope()
        {
            var childLocator = Locator.CreateChildLocator(base.ServiceLocator);
            return new WebApiDependencyScope(childLocator);
        }
    }
}
