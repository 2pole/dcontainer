using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using Common.Logging;

namespace DContainer.ServiceModel
{
    public class DContainerInstanceProvider : IInstanceProvider
    {
        public ILog Logger { get; set; }
        public Type ServiceType { get; private set; }

        public DContainerInstanceProvider(Type serviceType)
        {
            AssertUtils.ArgumentNotNull(serviceType, "serviceType");

            this.ServiceType = serviceType;
            this.Logger = LogManager.GetLogger<DContainerInstanceProvider>();
        }

        /// <summary>
        /// Get Service instace via service locator
        /// </summary>
        /// <param name="instanceContext"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var typeToResolve = this.ServiceType ?? instanceContext.Host.Description.ServiceType;
            AssertUtils.ArgumentNotNull(typeToResolve, "typeToResolve");
            Logger.DebugFormat("Get Instance [{0}] for service lcoator, Message: [{1}]", typeToResolve.FullName, message);
            
            return Locator.Current.Resolve(typeToResolve);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            var dcontainerInstanceContex = instanceContext.Extensions.Find<DContainerInstanceContext>();
            if (dcontainerInstanceContex != null)
                dcontainerInstanceContex.Dispose();
        }
    }
}
