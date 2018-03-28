using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Dependencies;

namespace DContainer.WebApi
{
    public class WebApiDependencyScope : IDependencyScope
    {
        private IServiceLocator _serviceLocator;

        public IServiceLocator ServiceLocator
        {
            get { return _serviceLocator; }
        }

        public WebApiDependencyScope(IServiceLocator locator)
        {
            _serviceLocator = locator;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _serviceLocator.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceLocator.ResolveAll(serviceType);
        }

        public void Dispose()
        {
            if (_serviceLocator != null)
            {
                _serviceLocator.Dispose();
                _serviceLocator = null;
            }
        }
    }
}
