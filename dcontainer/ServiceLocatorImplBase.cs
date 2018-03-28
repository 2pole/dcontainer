using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DContainer.Properties;
using MicrosoftLocator = CommonServiceLocator.IServiceLocator;

namespace DContainer
{
    /// <summary>
    /// This class is a helper that provides a default implementation
    /// for most of the methods of <see cref="IServiceLocator"/>.
    /// </summary>
    public abstract class ServiceLocatorImplBase : IServiceLocator, MicrosoftLocator
    {
        #region IServiceLocator
        public IServiceContext ServiceContext { get; protected set; }

        protected ServiceLocatorImplBase(IServiceContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            this.ServiceContext = context;
        }

        public virtual TService Resolve<TService>()
        {
            return Resolve<TService>(null);
        }

        public virtual TService Resolve<TService>(string key)
        {
            return (TService)Resolve(typeof(TService), key);
        }

        public virtual object Resolve(Type serviceType)
        {
            return Resolve(serviceType, null);
        }

        public virtual object Resolve(Type serviceType, string key)
        {
            try
            {
                return DoGetInstance(serviceType, key);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivationExceptionMessage(ex, serviceType, key),
                    ex);
            }
        }

        public virtual IEnumerable<object> ResolveAll(Type serviceType)
        {
            try
            {
                return DoGetAllInstances(serviceType);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivateAllExceptionMessage(ex, serviceType),
                    ex);
            }
        }

        public virtual IEnumerable<TService> ResolveAll<TService>()
        {
            return ResolveAll(typeof(TService)).Cast<TService>();
        }
        #endregion

        #region IServiceProvider
        object IServiceProvider.GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of resolving
        /// the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        protected abstract object DoGetInstance(Type serviceType, string key);

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of
        /// resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        protected abstract IEnumerable<object> DoGetAllInstances(Type serviceType);

        /// <summary>
        /// Format the exception message for use in an <see cref="ActivationException"/>
        /// that occurs while resolving a single service.
        /// </summary>
        /// <param name="actualException">The actual exception thrown by the implementation.</param>
        /// <param name="serviceType">Type of service requested.</param>
        /// <param name="key">Name requested.</param>
        /// <returns>The formatted exception message string.</returns>
        protected virtual string FormatActivationExceptionMessage(Exception actualException, Type serviceType, string key)
        {
            return string.Format(CultureInfo.CurrentUICulture, Resources.ActivationExceptionMessage, serviceType.Name, key);
        }

        /// <summary>
        /// Format the exception message for use in an <see cref="ActivationException"/>
        /// that occurs while resolving multiple service instances.
        /// </summary>
        /// <param name="actualException">The actual exception thrown by the implementation.</param>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>The formatted exception message string.</returns>
        protected virtual string FormatActivateAllExceptionMessage(Exception actualException, Type serviceType)
        {
            return string.Format(CultureInfo.CurrentUICulture, Resources.ActivateAllExceptionMessage, serviceType.Name);
        }
        #endregion

        #region Microsoft.Practices.ServiceLocation.IServiceLocator
        object MicrosoftLocator.GetInstance(Type serviceType)
        {
            return this.Resolve(serviceType);
        }

        object MicrosoftLocator.GetInstance(Type serviceType, string key)
        {
            return this.Resolve(serviceType, key);
        }

        IEnumerable<object> MicrosoftLocator.GetAllInstances(Type serviceType)
        {
            return this.ResolveAll(serviceType);
        }

        TService MicrosoftLocator.GetInstance<TService>()
        {
            return this.Resolve<TService>();
        }

        TService MicrosoftLocator.GetInstance<TService>(string key)
        {
            return this.Resolve<TService>(key);
        }

        IEnumerable<TService> MicrosoftLocator.GetAllInstances<TService>()
        {
            return this.ResolveAll<TService>();
        }
        #endregion

        #region IDisposable
        public virtual void Dispose()
        {
            if (ServiceContext != null)
            {
                ServiceContext.Dispose();
                ServiceContext = null;
            }
        }

        #endregion
    }

    public abstract class ServiceLocatorImplBase<TContainer> : ServiceLocatorImplBase
        where TContainer : class
    {
        protected ServiceLocatorImplBase(IServiceContext<TContainer> context)
            : base(context)
        {
        }

        public TContainer Container
        {
            get { return (TContainer)ServiceContext.Container; }
        }
    }
}
