using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public static class IServiceRegisterExtensions
    {
        public static IServiceRegister RegisterInstanceIfMissing<TService>(this IServiceRegister register, TService service)
        {
            bool isRegistered = register.IsRegistered<TService>();
            if (!isRegistered)
                register.RegisterInstance<TService>(service);
            return register;
        }

        public static IServiceRegister RegisterTypeIfMissing<TFrom, TTo>(this IServiceRegister register, LifetimeScope scope) where TTo : TFrom
        {
            bool isRegistered = register.IsRegistered<TFrom>();
            if (!isRegistered)
                register.RegisterType<TFrom, TTo>(scope);
            return register;
        }

        public static IServiceRegister RegisterTypeIfMissing<TFrom, TTo>(this IServiceRegister register) where TTo : TFrom
        {
            return register.RegisterTypeIfMissing<TFrom, TTo>(LifetimeScope.Transient);
        }

        public static IServiceRegister RegisterTypeIfMissing<TFrom>(this IServiceRegister register, LifetimeScope scope)
        {
            var typeToRegister = typeof(TFrom);
            bool isRegistered = register.IsRegistered(typeToRegister);
            if (!isRegistered)
                register.RegisterType(typeToRegister, scope);
            return register;
        }

        public static IServiceRegister RegisterTypeIfMissing<TFrom>(this IServiceRegister register)
        {
            return register.RegisterTypeIfMissing<TFrom>(LifetimeScope.Transient);
        }

        public static IServiceRegister RegisterTypeIfMissing(this IServiceRegister register, Type typeFrom, Type typeTo, LifetimeScope scope)
        {
            var typeToRegister = typeFrom ?? typeTo;
            bool isRegistered = register.IsRegistered(typeToRegister);
            if (!isRegistered)
                register.RegisterType(typeFrom, typeTo, scope);
            return register;
        }

        public static IServiceRegister RegisterTypeIfMissing(this IServiceRegister register, Type typeFrom, Type typeTo, string serviceName, LifetimeScope scope)
        {
            var typeToRegister = typeFrom ?? typeTo;
            bool isRegistered = register.IsRegistered(typeToRegister, serviceName);
            if (!isRegistered)
                register.RegisterType(typeFrom, typeTo, serviceName, scope);
            return register;
        }

        public static IServiceRegister RegisterType<TRegistered>(this IServiceRegister register)
        {
            Type registeredType = typeof (TRegistered);
            register.RegisterType(registeredType);
            return register;
        }

        public static IServiceRegister RegisterType<TRegistered>(this IServiceRegister register, LifetimeScope scope)
        {
            Type registeredType = typeof(TRegistered);
            register.RegisterType(registeredType, scope);
            return register;
        }
    }
}
