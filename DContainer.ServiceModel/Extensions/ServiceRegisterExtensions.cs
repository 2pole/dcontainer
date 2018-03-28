using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DContainer.ServiceModel
{
    public static class ServiceRegisterExtensions
    {
        public static IServiceRegister RegisterCurrentLocatorProvider(this IServiceRegister register)
        {
            Locator.SetCurrentLocatorProvider(() =>
            {
                var operationContext = OperationContext.Current;
                if (operationContext == null)
                    return Locator.Root;

                var instanceContext = operationContext.InstanceContext.Extensions.Find<DContainerInstanceContext>();
                if (instanceContext == null)
                    return Locator.Root;

                return instanceContext.ChildLocator;
            });
            return register;
        }
    }
}
