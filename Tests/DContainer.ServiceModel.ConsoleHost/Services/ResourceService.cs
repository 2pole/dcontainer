using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DContainer;

namespace DContainer.ServiceModel.ConsoleHost.Services
{
    public class ResourceService : IResourceService
    {
        //private readonly IServiceLocator _locator;
        public ResourceService()
        {
            //_resourceRepository = resourceRepository;
        }

        #region Implementation of IResourceService

        public ResourceInfo GetResource(int resourceId)
        {
            //var dbResourceInfo = _resourceRepository.Get(d => d.ResourceId == resourceId);
            return new ResourceInfo() { ResourceName = "Resource Test"} ;
        }

        public IList<ResourceInfo> GetResources(int[] resourceKeys)
        {
            return new List<ResourceInfo>();
        }

        #endregion
    }
}
