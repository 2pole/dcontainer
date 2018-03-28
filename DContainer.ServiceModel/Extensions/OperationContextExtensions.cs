using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DContainer.ServiceModel
{
    public static class OperationContextExtensions
    {
        public static IServiceLocator CurrentLocator(this OperationContext context)
        {
            if (context == null)
                return Locator.Root;

            var instanceContext = context.InstanceContext.Extensions.Find<DContainerInstanceContext>();
            if (instanceContext == null)
                return Locator.Root;

            return instanceContext.ChildLocator;
        }
    }
}
