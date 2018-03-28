using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Util;

namespace DContainer.Spring
{
    public class ServiceRegisterAdapter : ServiceRegisterImplBase<IApplicationContext>
    {
        public ServiceRegisterAdapter(IServiceContext<IApplicationContext> context)
            :base(context)
        {
        }
        
        public override bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            return Container.IsRegistered(typeToCheck, nameToCheck);
        }

        public override IServiceRegister RegisterType(Type typeToRegister, string name, LifetimeScope scope)
        {
            Container.RegisterType(typeToRegister, name, scope);
            return this;
        }

        public override IServiceRegister RegisterType(Type fromType, Type toType, string name, LifetimeScope scope)
        {
            Container.RegisterType(fromType, toType, name, scope);
            return this;
        }

        public override IServiceRegister RegisterInstance(Type t, string name, object instance)
        {
            Container.RegisterInstance(t, name, instance);
            return this;
        }
    }
}
