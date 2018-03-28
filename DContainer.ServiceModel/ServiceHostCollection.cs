using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DContainer.ServiceModel
{
    public class ServiceHostCollection : Collection<ServiceHost>, IServiceHost
    {
        #region Implementation of IServiceHost

        public void Open()
        {
            foreach (var serviceHost in Items)
            {
                serviceHost.Open();
            }
        }

        public void Close()
        {
            foreach (var serviceHost in Items)
            {
                serviceHost.Close();
            }
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach (var serviceHost in Items)
                {
                    if(serviceHost is IDisposable)
                        ((IDisposable)serviceHost).Dispose();
                    else
                        serviceHost.Close();
                }
                //Items.Clear();
            }
        }

        ~ServiceHostCollection()
        {
            Dispose(false);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
