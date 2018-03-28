using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DContainer.WebApi
{
    public static class RegistrationExtensions
    {
        //public static IServiceRegister RegisterCurrentLocatorProvider(this IServiceRegister register)
        //{
        //    MicrosoftLocation.ServiceLocator.SetLocatorProvider(() => (MicrosoftLocation.IServiceLocator)RequestLifetimeHttpModule.ChildServiceLocator);
        //    Locator.SetCurrentLocatorProvider(() => RequestLifetimeHttpModule.ChildServiceLocator);
        //    return register;
        //}

        public static IServiceRegister RegisterApiControllers(
                this IServiceRegister register,
                params Assembly[] controllerAssemblies)
        {
            var controllers = ScanAssemblies(controllerAssemblies).Where(t => typeof(IHttpController).IsAssignableFrom(t) && t.Name.EndsWith("Controller"));
            foreach (var controller in controllers)
            {
                register.RegisterType(controller);
            }
            return register;
        }

        public static IServiceRegister RegisterWebApiFilterProvider(this IServiceRegister register, HttpConfiguration configuration)
        {
            var filterProvider = new WebApiActionDescriptorFilterProvider();
            configuration.Services.RemoveAll(typeof(IFilterProvider), provider => provider is ActionDescriptorFilterProvider);
            register.RegisterInstance<IFilterProvider>(filterProvider);
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
