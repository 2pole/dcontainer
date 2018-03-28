using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using Common.Logging;

namespace DContainer.ServiceModel
{
    public class DContainerServiceBehavior : IServiceBehavior
    {
        public ILog Logger { get; private set; }

        public DContainerServiceBehavior()
        {
            Logger = LogManager.GetLogger(typeof(DContainerServiceBehavior));
        }

        #region Implementation of IServiceBehavior

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param><param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param><param name="serviceHostBase">The host of the service.</param><param name="endpoints">The service endpoints.</param><param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param><param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var endpoints = serviceHostBase.ChannelDispatchers
                                .OfType<ChannelDispatcher>()
                                .SelectMany(dispatcher => dispatcher.Endpoints);

            var provider = new DContainerInstanceProvider(serviceDescription.ServiceType);
            var initializer = new DContainerInstanceContextInitializer();
            foreach (var endpoint in endpoints)
            {
                endpoint.DispatchRuntime.InstanceProvider = provider;
                endpoint.DispatchRuntime.InstanceContextInitializers.Add(initializer);
                Logger.DebugFormat("Apply dispatch behavior, ServiceType: [{0}]", serviceDescription.ServiceType.FullName);
            }
        }

        #endregion
    }
}
