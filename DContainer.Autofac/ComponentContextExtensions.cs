using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Builder;

namespace DContainer.Autofac
{
    public static class ComponentContextExtensions
    {
        #region Register Instance
        public static IComponentContext RegisterInstance<TInterface>(this IComponentContext container, TInterface instance) where TInterface : class
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance<TInterface>(instance);
            builder.Update(container.ComponentRegistry);
            return container;
        }

        public static IComponentContext RegisterInstance(this IComponentContext container, object instance, Type typeToRegister, string name)
        {
            AssertUtils.ArgumentNotNull(instance, "instance");
            AssertUtils.ArgumentNotNull(typeToRegister, "typeToRegister");

            ContainerBuilder builder = new ContainerBuilder();
            var register = builder.RegisterInstance(instance).As(typeToRegister);

            if (!string.IsNullOrEmpty(name))
                register.Named(name, typeToRegister);

            builder.Update(container.ComponentRegistry);
            return container;
        }

        #endregion

        #region Register Type
        public static IComponentContext RegisterType(this IComponentContext container, Type fromType, Type toType, string name, LifetimeScope scope)
        {
            AssertUtils.ArgumentNotNull(fromType, "fromType");
            AssertUtils.ArgumentNotNull(toType, "toType");

            if (fromType.IsGenericTypeDefinition && toType.IsGenericTypeDefinition)
            {
                return container.RegisterGenericType(fromType, toType, name, scope);
            }
            else
            {
                return container.RegisterCommonType(fromType, toType, name, scope);
            }
        }

        private static IComponentContext RegisterGenericType(this IComponentContext container, Type fromType, Type toType, string name, LifetimeScope scope)
        {
            ContainerBuilder builder = new ContainerBuilder();
            var register = builder.RegisterGeneric(toType).As(fromType);

            if (!string.IsNullOrEmpty(name))
                register.Named(name, fromType);

            register.SetLifetime(scope);
            builder.Update(container.ComponentRegistry);
            return container;
        }

        private static void SetLifetime<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder, LifetimeScope scope)
        {
            switch (scope)
            {
                case LifetimeScope.Singleton:
                    builder.SingleInstance();
                    break;
                case LifetimeScope.Transient:
                    builder.InstancePerDependency();
                    break;
                case LifetimeScope.PerContainer:
                    builder.InstancePerLifetimeScope();
                    break;
                default:
                    throw new NotSupportedException(string.Format("The LifetimeScope {0} is not support.", scope.ToString("D")));
            }
        }

        private static IComponentContext RegisterCommonType(this IComponentContext container, Type fromType, Type toType, string name, LifetimeScope scope)
        {
            ContainerBuilder builder = new ContainerBuilder();
            var register = builder.RegisterType(toType).As(fromType);

            if (!string.IsNullOrEmpty(name))
                register.Named(name, fromType);

            register.SetLifetime(scope);

            builder.Update(container.ComponentRegistry);
            return container;
        }

        public static IComponentContext RegisterType(this IComponentContext container, Type typeToRegister, string name, LifetimeScope scope)
        {
            AssertUtils.ArgumentNotNull(typeToRegister, "typeToRegister");

            if (typeToRegister.IsGenericTypeDefinition)
            {
                return container.RegisterGenericType(typeToRegister, typeToRegister, name, scope);
            }
            else
            {
                return container.RegisterCommonType(typeToRegister, typeToRegister, name, scope);
            }
        }

        #endregion

        #region Resolve
        public static object ResolveOptional(this IComponentContext container, Type typeToRegister, string key)
        {
            object instance = null;
            if (string.IsNullOrEmpty(key))
                instance = container.ResolveOptional(typeToRegister);
            else
                container.TryResolveNamed(key, typeToRegister, out instance);
            return instance;
        }

        public static object ResolveRequired(this IComponentContext container, Type typeToRegister, string key)
        {
            if (string.IsNullOrEmpty(key))
                return container.Resolve(typeToRegister);
            else
                return container.ResolveNamed(key, typeToRegister);
        }

        #endregion

        #region Resolve All
        public static IEnumerable<T> ResolveAll<T>(this IComponentContext container)
        {
            return container.Resolve<IEnumerable<T>>();
        }

        public static IEnumerable<object> ResolveAll(this IComponentContext container, Type typeItem)
        {
            AssertUtils.ArgumentNotNull(typeItem, "typeItem");
            Type t = typeof(IEnumerable<>).MakeGenericType(new[] { typeItem });
            var instance = container.Resolve(t);
            if (instance != null && instance is IEnumerable)
                return ((IEnumerable)instance).Cast<object>();
            return Enumerable.Empty<object>();
        }

        #endregion
    }
}
