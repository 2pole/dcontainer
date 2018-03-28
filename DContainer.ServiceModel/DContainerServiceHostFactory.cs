using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Common.Logging;
using DContainer;

namespace DContainer.ServiceModel
{
    public class DContainerServiceHostFactory : System.ServiceModel.Activation.ServiceHostFactory, IServiceHostFactory
    {
        public ILog Logger { get; set; }

        public DContainerServiceHostFactory()
        {
            Logger = LogManager.GetLogger(this.GetType());
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Logger.DebugFormat("Creating Service Host:[{0}], Service Type: {1}, Base Addresses: {2}",
                typeof(DContainerServiceHost).FullName,
                serviceType == null ? null : serviceType.FullName,
                baseAddresses == null ? null : string.Join(", ", baseAddresses.Select(d => d.ToString())));

            if (baseAddresses == null)
                return new DContainerServiceHost(serviceType);
            else
                return new DContainerServiceHost(serviceType, baseAddresses);
        }

        public ServiceHostCollection CreateServiceHosts(params Type[] serviceTypes)
        {
            AssertUtils.ArgumentNotNull(serviceTypes, "serviceTypes");
            var hostCollection = new ServiceHostCollection();
            foreach (var serviceType in serviceTypes)
            {
                var serviceHost = CreateServiceHost(serviceType, null);
                hostCollection.Add(serviceHost);
            }
            return hostCollection;
        }
    }
}
