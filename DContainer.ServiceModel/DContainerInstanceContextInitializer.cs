using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace DContainer.ServiceModel
{
    public class DContainerInstanceContextInitializer : IInstanceContextInitializer
    {
        #region Implementation of IInstanceContextInitializer

        /// <summary>
        /// Provides the ability to modify the newly created <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The system-supplied instance context.</param><param name="message">The message that triggered the creation of the instance context.</param>
        public void Initialize(InstanceContext instanceContext, Message message)
        {
            instanceContext.Extensions.Add(new DContainerInstanceContext());
        }

        #endregion
    }
}
