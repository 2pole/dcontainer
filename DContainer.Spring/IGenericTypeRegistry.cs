using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Spring
{
    public interface IGenericTypeRegistry
    {
        bool IsRegistered(Type fromType, string key);

        void RegisterGenericTypeDefinition(Type fromType, Type toType, string key);

        object Resolve(Type serviceType, string key);
    }
}
