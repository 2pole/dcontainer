using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DContainer.ServiceModel.ConsoleHost.Services
{
    [ServiceContract(Namespace = Constants.Namespace)]
    public interface IResourceService
    {
        [OperationContract]
        ResourceInfo GetResource(int resourceId);

        [OperationContract]
        IList<ResourceInfo> GetResources(int[] resourceKeys);
    }
}
