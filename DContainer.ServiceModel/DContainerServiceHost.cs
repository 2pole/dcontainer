using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using DContainer;

namespace DContainer.ServiceModel
{
    public class DContainerServiceHost : System.ServiceModel.ServiceHost
    {
        public readonly ILog Logger = LogManager.GetLogger<DContainerServiceHost>();

        public DContainerServiceHost()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorServiceHost"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public DContainerServiceHost(TypeCode serviceType)
            : base(serviceType)
        {
        }

         /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorServiceHost"/> class.
        /// </summary>
        /// <param name="singletonInstance">The singleton instance.</param>
        public DContainerServiceHost(object singletonInstance)
            : base(singletonInstance)
        {
        }

        public DContainerServiceHost(Type serviceType, params Uri[] baseAddress)
            : base(serviceType, baseAddress)
        {
        }

        protected override void OnOpening()
        {
            base.OnOpening();

            if (this.Description.Behaviors.Find<DContainerServiceBehavior>() == null)
            {
                Logger.DebugFormat("Adding service behavior:{0}", typeof(DContainerServiceBehavior).FullName);
                this.Description.Behaviors.Add(new DContainerServiceBehavior());
            }
        }
    }
}
