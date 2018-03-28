using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace DContainer.Castle
{
    public class ServiceRegisterAdapter : ServiceRegisterImplBase<IWindsorContainer>
    {
        public ServiceRegisterAdapter(IServiceContext<IWindsorContainer> context)
            : base(context)
        {
        }

        public override bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            if (string.IsNullOrEmpty(nameToCheck))
                return Container.Kernel.HasComponent(typeToCheck);
            else
                return Container.Kernel.HasComponent(nameToCheck);
        }

        public override IServiceRegister RegisterType(Type fromType, Type toType, string name, LifetimeScope scope)
        {
            if (toType == null)
                toType = fromType;

            var registration = Component.For(fromType).ImplementedBy(toType).SetLifeTime(scope);
            if (!string.IsNullOrWhiteSpace(name))
                registration.Named(name);
            Container.Register(registration);

            return this;
        }

        public override IServiceRegister RegisterInstance(Type t, string name, object instance)
        {
            if (t == null)
                t = instance.GetType();
            var registration = Component.For(t).Instance(instance);
            if (!string.IsNullOrWhiteSpace(name))
                registration.Named(name);
            Container.Register(registration);

            return this;
        }

        public override IServiceRegister RegisterType(Type typeToRegister, string name, LifetimeScope scope)
        {
            var registration = Component.For(typeToRegister).SetLifeTime(scope);
            if (!string.IsNullOrWhiteSpace(name))
                registration.Named(name);
            Container.Register(registration);

            return this;
        }
    }
}
