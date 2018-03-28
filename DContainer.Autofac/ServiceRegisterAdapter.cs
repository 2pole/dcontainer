using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DContainer.Autofac
{
    public class ServiceRegisterAdapter : ServiceRegisterImplBase<IComponentContext>
    {
        public ServiceRegisterAdapter(IServiceContext<IComponentContext> context)
            : base(context)
        {
        }

        public override bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            if (string.IsNullOrEmpty(nameToCheck))
                return Container.IsRegistered(typeToCheck);
            else
                return Container.IsRegisteredWithName(nameToCheck, typeToCheck);
        }

        public override IServiceRegister RegisterType(Type fromType, Type toType, string name, LifetimeScope scope)
        {
            Container.RegisterType(fromType, toType, name, scope);
            return this;
        }

        public override IServiceRegister RegisterInstance(Type t, string name, object instance)
        {
            Container.RegisterInstance(instance, t, name);
            return this;
        }

        public override IServiceRegister RegisterType(Type typeToRegister, string name, LifetimeScope scope)
        {
            Container.RegisterType(typeToRegister, name, scope);
            return this;
        }
    }
}
