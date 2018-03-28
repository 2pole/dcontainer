using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Lifetime;

namespace DContainer.Unity
{
    public class ServiceRegisterAdapter : ServiceRegisterImplBase<IUnityContainer>
    {
        public ServiceRegisterAdapter(IServiceContext<IUnityContainer> context)
            : base(context)
        {
        }

        public override bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            if (string.IsNullOrEmpty(nameToCheck))
                return Container.IsRegistered(typeToCheck);
            else
                return Container.IsRegistered(typeToCheck, nameToCheck);
        }

        public override IServiceRegister RegisterType(Type fromType, Type toType, string name, LifetimeScope scope)
        {
            var lifetimeManager = GetLifetimeManager(scope);
            Container.RegisterType(fromType, toType, name, lifetimeManager);
            return this;
        }

        public override IServiceRegister RegisterType(Type typeToRegister, string name, LifetimeScope scope)
        {
            var lifetimeManager = GetLifetimeManager(scope);
            Container.RegisterType(typeToRegister, name, lifetimeManager);
            return this;
        }

        public override IServiceRegister RegisterInstance(Type typeToRegister, string name, object instance)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (typeToRegister == null)
                    Container.RegisterInstance(instance);
                else
                    Container.RegisterInstance(typeToRegister, instance);
            }
            else
            {
                if (typeToRegister == null)
                    Container.RegisterInstance(name, instance);
                else
                    Container.RegisterInstance(typeToRegister, name, instance);
            }
            return this;
        }

        protected virtual LifetimeManager GetLifetimeManager(LifetimeScope scope)
        {
            switch (scope)
            {
                case LifetimeScope.Transient:
                    return new PerResolveLifetimeManager();
                case LifetimeScope.Singleton:
                    return new ContainerControlledLifetimeManager();
                case LifetimeScope.PerContainer:
                    return new HierarchicalLifetimeManager();
                default:
                    throw new ArgumentOutOfRangeException("scope");
            }
        }
    }
}
