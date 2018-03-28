using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using Spring.Util;

namespace DContainer.Spring
{
    public static class IApplicationContextExtensions
    {
        #region Register Type
        public static IApplicationContext RegisterType(this IApplicationContext context,
            Type fromType, Type toType, string alias, LifetimeScope scope)
        {
            AssertUtils.ArgumentNotNull(fromType, "fromType");
            if (toType == null)
                toType = fromType;

            if(fromType.IsGenericTypeDefinition && toType.IsGenericTypeDefinition)
            {
                var typeRegistry = context.GetGenericTypeRegistry();
                typeRegistry.RegisterGenericTypeDefinition(fromType, toType, alias);
            }

            if (string.IsNullOrEmpty(alias))
                alias = GenerateAlias(fromType);

            IConfigurableApplicationContext configurableContext = context as IConfigurableApplicationContext;
            // Only try to register the new definition objects when the context can be modified
            if (configurableContext != null)
            {
                DefaultObjectDefinitionFactory definitionFactory = new DefaultObjectDefinitionFactory();
                ObjectDefinitionBuilder builder = ObjectDefinitionBuilder.RootObjectDefinition(definitionFactory, toType);
                //builder.SetSingleton(singleton);
                builder.SetLifetime(scope);
                builder.SetAutowireMode(AutoWiringMode.Constructor);
                //builder.SetDependencyCheck(DependencyCheckingMode.All);

                configurableContext.ObjectFactory.RegisterObjectDefinition(alias, builder.ObjectDefinition);
                //Only refresh when possible
                //configurableContext.Refresh();
            }
            return context;
        }

        public static IApplicationContext RegisterType(this IApplicationContext context, 
            Type typeToRegister, string alias, LifetimeScope scope)
        {
            AssertUtils.ArgumentNotNull(typeToRegister, "typeToRegister");
            return context.RegisterType(typeToRegister, null, alias, scope);
        }
        #endregion

        #region Register Instance
        public static IApplicationContext RegisterInstance<TInterface>(this IApplicationContext context, TInterface instance)
        {
            return context.RegisterInstance(typeof(TInterface), null, instance);
        }

        public static IApplicationContext RegisterInstance(this IApplicationContext context, Type typeToRegister, string alias, object instance)
        {
            AssertUtils.ArgumentNotNull(typeToRegister, "typeToRegister");
            AssertUtils.ArgumentNotNull(instance, "instance");

            if (string.IsNullOrEmpty(alias))
                alias = GenerateAlias(typeToRegister);

            var configurableContext = context as IConfigurableApplicationContext;
            if (configurableContext != null)
                configurableContext.ObjectFactory.RegisterSingleton(alias, instance);

            return context;
        }

        #endregion

        #region Resolve
        public static T Resolve<T>(this IApplicationContext context)
        {
            return (T)context.Resolve(typeof(T), null);
        }

        public static object Resolve(this IApplicationContext context, Type serviceType, string alias)
        {
            AssertUtils.ArgumentNotNull(serviceType, "serviceType");

            try
            {
                if(serviceType.IsGenericType)
                {
                    var typeRegistry = context.GetGenericTypeRegistry();
                    bool isRegistered = typeRegistry.IsRegistered(serviceType, alias);
                    if(isRegistered)
                        return typeRegistry.Resolve(serviceType, alias);
                }

                if (string.IsNullOrEmpty(alias))
                    alias = GenerateAlias(serviceType);

                IConfigurableApplicationContext configurableContext = null;
                if (context.ContainsObjectDefinition(alias))
                    return context.GetObject(alias, serviceType);

                if ((configurableContext = context as IConfigurableApplicationContext) != null &&
                    configurableContext.ObjectFactory.ContainsSingleton(alias))
                    return configurableContext.ObjectFactory.GetSingleton(alias);

                return context.ResolveAll(serviceType).FirstOrDefault();
            }
            catch (Exception)
            {
                return context.ResolveAll(serviceType).FirstOrDefault();
            }
        }

        public static IEnumerable<object> ResolveAll(this IApplicationContext context, Type serviceType)
        {
            AssertUtils.ArgumentNotNull(serviceType, "serviceType");
            return context.GetObjectsOfType(serviceType).Values.OfType<object>();
        }
        #endregion

        #region IsRegistered
        public static bool IsRegistered(this IApplicationContext context, Type typeToCheck, string nameToCheck)
        {
            AssertUtils.ArgumentNotNull(typeToCheck, "typeToCheck");

            if(typeToCheck.IsGenericType)
            {
                var genericTypeRegistry = context.GetGenericTypeRegistry();
                bool isGeneric = genericTypeRegistry.IsRegistered(typeToCheck, nameToCheck);
                if (isGeneric)
                    return true;
            }

            if (string.IsNullOrEmpty(nameToCheck))
                nameToCheck = GenerateAlias(typeToCheck);
           
            var configurableContext = context as IConfigurableApplicationContext;
            if (configurableContext != null)
            {
                bool isSingleton = configurableContext.ObjectFactory.ContainsSingleton(nameToCheck);
                if (isSingleton)
                    return true;
            }

            bool isRegistered = context.ContainsObjectDefinition(nameToCheck);
            if (isRegistered)
                return true;

            return false;
        }
        #endregion

        private static IGenericTypeRegistry GetGenericTypeRegistry(this IApplicationContext context)
        {
            var t = typeof (IGenericTypeRegistry);
            var alias = GenerateAlias(t);
            var typeRegistry = context.GetObject(alias, t);
            return typeRegistry as IGenericTypeRegistry;
        }

        internal static string GenerateAlias(Type type)
        {
            return new DefaultAliasGenerator().GetAlias(type);
        }

        private static ObjectDefinitionBuilder SetLifetime(this ObjectDefinitionBuilder builder, LifetimeScope scope)
        {
            switch (scope)
            {
                case LifetimeScope.Transient:
                    builder.SetSingleton(false);
                    break;
                case LifetimeScope.Singleton:
                    builder.SetSingleton(true);
                    break;
                case LifetimeScope.PerContainer:
                    //todo: Only Support Web Request
                    builder.RawObjectDefinition.Scope = SpringObjectScope.Request;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("scope");
            }
            return builder;
        }
    }
}
