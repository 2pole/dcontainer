using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;

namespace DContainer.Castle
{
    public static class ComponentRegistrationExtensions
    {
        public static ComponentRegistration<TService> SetLifeTime<TService>(
            this ComponentRegistration<TService> registration, LifetimeScope scope)
            where TService : class
        {
            switch (scope)
            {
                case LifetimeScope.Transient:
                    registration.LifestyleTransient();
                    break;
                case LifetimeScope.Singleton:
                    registration.LifestyleSingleton();
                    break;
                case LifetimeScope.PerContainer:
                    registration.LifestyleCustom<PerContainerLifestyleManager>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("scope");
            }
            return registration;
        }
    }
}
