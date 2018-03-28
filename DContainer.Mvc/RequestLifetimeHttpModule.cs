using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DContainer.Mvc
{
    public class RequestLifetimeHttpModule : IHttpModule
    {
        //public const string HttpContextKey = "PerRequestContainer";
        public const string PerRequestLocatorKey = "PerRequestDContainer";
        
        /// <summary>
        /// Gets the IoC Service Location that should be notified when a HTTP request ends.
        /// </summary>
        public static IServiceLocator ChildServiceLocator
        {
            get
            {
                var locator = (IServiceLocator)HttpContext.Current.Items[PerRequestLocatorKey];
                if (locator == null)
                    HttpContext.Current.Items[PerRequestLocatorKey] = locator = CreateChildLocator();
                return locator;
            }
        }

        public virtual void Init(HttpApplication context)
        {
            context.EndRequest += OnEndRequest;
        }

        public virtual void Dispose()
        {
        }

        private static void OnEndRequest(object sender, EventArgs e)
        {
            var childLocator = ChildServiceLocator;
            if (childLocator != null)
            {
                Locator.ContextProvider.ReleaseServiceContext(childLocator.ServiceContext);
                HttpContext.Current.Items.Remove(PerRequestLocatorKey);
            }
        }

        private static IServiceLocator CreateChildLocator()
        {
            var serviceContextProvider = Locator.ContextProvider;
            var childContext = serviceContextProvider.CreateChildServiceContext(Locator.Context);
            var locator = serviceContextProvider.CreateServiceLocator(childContext);
            return locator;
        }
    }
}
