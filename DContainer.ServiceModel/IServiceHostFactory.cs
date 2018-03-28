using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DContainer.ServiceModel
{
    public interface IServiceHostFactory
    {
        ServiceHostCollection CreateServiceHosts(params Type[] serviceTypes);

        //ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses);
    }
}
