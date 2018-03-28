using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DContainer.ServiceModel
{
    public class DContainerInstanceContext : IExtension<InstanceContext>, IDisposable
    {
        private IServiceLocator _childLocator;

        #region Implementation of IExtension<InstanceContext>

        /// <summary>
        /// Enables an extension object to find out when it has been aggregated. Called when the extension is added to the <see cref="P:System.ServiceModel.IExtensibleObject`1.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Attach(InstanceContext owner)
        {
        }

        /// <summary>
        /// Enables an object to find out when it is no longer aggregated. Called when an extension is removed from the <see cref="P:System.ServiceModel.IExtensibleObject`1.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Detach(InstanceContext owner)
        {
        }

        #endregion

        public IServiceLocator ChildLocator
        {
            get { return _childLocator ?? (_childLocator = Locator.CreateChildLocator()); }
        }

        public void Dispose()
        {
            if (_childLocator != null)
            {
                _childLocator.Dispose();
                _childLocator = null;
            }
        }
    }
}
