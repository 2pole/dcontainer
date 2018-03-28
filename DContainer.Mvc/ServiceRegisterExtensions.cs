using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DContainer.Mvc.Mock;

namespace DContainer.Mvc
{
    public static class ServiceRegisterExtensions
    {
        public static IServiceRegister RegisterCurrentLocatorProvider(this IServiceRegister register)
        {
            Locator.SetCurrentLocatorProvider(() => RequestLifetimeHttpModule.ChildServiceLocator);
            return register;
        }

        public static IServiceRegister RegisterControllers(
                this IServiceRegister register,
                params Assembly[] controllerAssemblies)
        {
            var controllers = ScanAssemblies(controllerAssemblies).Where(t => typeof(IController).IsAssignableFrom(t) &&
                    t.Name.EndsWith("Controller"));
            foreach (var controller in controllers)
            {
                register.RegisterType(controller);
            }
            return register;
        }

        public static IServiceRegister RegisterWebComponents(this IServiceRegister register)
        {
            if (register == null) throw new ArgumentNullException("register");
            register
                .RegisterType<HttpContextBase, DContainerHttpContext>(LifetimeScope.PerContainer)
                .RegisterType<HttpRequestBase, DContainerHttpRequest>(LifetimeScope.PerContainer)
                .RegisterType<HttpResponseBase, DContainerHttpResponse>(LifetimeScope.PerContainer)
                .RegisterType<HttpSessionStateBase, DContainerHttpSessionState>(LifetimeScope.PerContainer)
                .RegisterType<HttpServerUtilityBase, DContainerHttpServerUtility>(LifetimeScope.PerContainer)
                .RegisterType<HttpApplicationStateBase, DContainerHttpApplicationState>(LifetimeScope.PerContainer)
                .RegisterType<HttpBrowserCapabilitiesBase, DContainerHttpBrowserCapabilities>(LifetimeScope.PerContainer)
                .RegisterType<HttpCachePolicyBase, DContainerHttpCachePolicy>(LifetimeScope.PerContainer)
                ;
            return register;
        }

        /// <summary>
        /// Registers the <see cref="DContainerFilterAttributeFilterProvider"/>.
        /// </summary>
        /// <param name="register">The container builder.</param>
        public static IServiceRegister RegisterFilterProvider(this IServiceRegister register)
        {
            if (register == null) throw new ArgumentNullException("register");

            foreach (var provider in FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().ToArray())
                FilterProviders.Providers.Remove(provider);

            register.RegisterType<DContainerFilterAttributeFilterProvider>(LifetimeScope.Singleton);
            return register;
        }

        private static IEnumerable<Type> ScanAssemblies(IEnumerable<Assembly> assemblies)
        {
            var query = assemblies.SelectMany(a => a.GetTypes()).Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericTypeDefinition &&
                    !t.IsDelegate());
            return query;
        }
    }
}
