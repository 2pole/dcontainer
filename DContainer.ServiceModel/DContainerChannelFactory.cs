using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common.Logging;

namespace DContainer.ServiceModel
{
    public class ChannelFactory<TService> : System.ServiceModel.ChannelFactory<TService>
    {
        public ILog Logger { get; set; }

        public ChannelFactory()
            : base()
        {
            Logger = LogManager.GetLogger(this.GetType());
        }

        public ChannelFactory(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
            Logger = LogManager.GetLogger(this.GetType());
        }

        public override TService CreateChannel(EndpointAddress address, Uri via)
        {
            Logger.DebugFormat("Create Channel: [{0}], Endpoint: [{1}], Uri: [{2}]",
                typeof(TService).FullName,
                address,
                via);

            return base.CreateChannel(address, via);
        }
    }
}
