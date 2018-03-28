using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using Common.Logging;

namespace DContainer
{
    /// <summary>
    /// This class provides the ambient container for this application. If your
    /// framework defines such an ambient container, use ServiceLocator.Current
    /// to get it.
    /// </summary>
    public static class Locator
    {
        #region Private Static Fields

        private static IServiceContextProvider _serviceContextProvider;
        private static IServiceLocator _serviceLocator;
        private static IServiceRegister _serviceRegister;
        private static IServiceContext _serviceContext;
        private static Func<IServiceLocator> _currentLocatorProvider;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Public Static Properties
        public static ILog Log { get; private set; }

        public static IServiceLocator Root
        {
            get
            {
                if (_serviceLocator == null)
                {
                    lock (SyncRoot)
                    {
                        if (_serviceLocator == null)
                        {
                            _serviceLocator = CreateServiceLocator();
                        }
                    }
                }
                return _serviceLocator;
            }
            private set
            {
                lock (SyncRoot)
                {
                    _serviceLocator = value;
                }
            }
        }

        public static IServiceLocator Current
        {
            get { return _currentLocatorProvider(); }
        }

        public static IServiceRegister Register
        {
            get
            {
                if (_serviceRegister == null)
                {
                    lock (SyncRoot)
                    {
                        if (_serviceRegister == null)
                        {
                            _serviceRegister = CreateServiceRegister();
                        }
                    }
                }
                return _serviceRegister;
            }
            private set
            {
                lock (SyncRoot)
                {
                    _serviceRegister = value;
                }
            }
        }

        /// <summary>
        /// The current ambient container.
        /// </summary>
        public static IServiceContext Context
        {
            get
            {
                if (_serviceContext == null)
                {
                    lock (SyncRoot)
                    {
                        if (_serviceContext == null)
                        {
                            _serviceContext = CreateServiceContext();
                        }
                    }
                }
                return _serviceContext;
            }
            private set
            {
                lock (SyncRoot)
                {
                    _serviceContext = value;
                }
            }
        }

        public static IServiceContextProvider ContextProvider
        {
            get
            {
                if (_serviceContextProvider == null)
                {
                    lock (SyncRoot)
                    {
                        if (_serviceContextProvider == null)
                        {
                            _serviceContextProvider = GetServiceContextProvider();
                        }
                    }
                }
                return _serviceContextProvider;
            }
            private set
            {
                lock (SyncRoot)
                {
                    _serviceContextProvider = value;
                }
            }
        }

        #endregion

        static Locator()
        {
            Log = LogManager.GetLogger(typeof(Locator));
            SetCurrentLocatorProvider(() => Root);
        }

        #region Public Static Methods
        public static void SetCurrentLocatorProvider(Func<IServiceLocator> provider)
        {
            AssertUtils.ArgumentNotNull(provider, "provider");
            _currentLocatorProvider = provider;
            CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => (CommonServiceLocator.IServiceLocator)Locator.Current);
        }

        public static void SetContextProvider(IServiceContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public static IServiceLocator CreateChildLocator()
        {
            return CreateChildLocator(Root.ServiceContext);
        }

        public static IServiceLocator CreateChildLocator(IServiceLocator locator)
        {
            return CreateChildLocator(locator.ServiceContext);
        }

        public static IServiceLocator CreateChildLocator(IServiceContext context)
        {
            AssertUtils.ArgumentNotNull(context, "context");

            var childContext = ContextProvider.CreateChildServiceContext(context);
            var locator = ContextProvider.CreateServiceLocator(childContext);
            return locator;
        }

        public static void Clear()
        {
            ContextProvider = null;
            Context = null;
            Root = null;
            Register = null;
            _currentLocatorProvider = null;
        }
        #endregion

        #region Private Static Methods
        private static Configuration.DContainerSection GetConfigurationSection()
        {
            var section = (Configuration.DContainerSection)ConfigurationManager.GetSection(Configuration.DContainerSection.DContainerSectionName);
            return section;
        }

        private static IServiceRegister CreateServiceRegister()
        {
            if (ContextProvider != null)
            {
                try
                {
                    var register = ContextProvider.CreateServiceRegister(Context);
                    return register;
                }
                catch (Exception exp)
                {
                    Log.Error("Creating service register occur error.", exp);
                }
            }

            return null;
        }

        private static IServiceLocator CreateServiceLocator()
        {
            if (ContextProvider != null)
            {
                try
                {
                    var locator = ContextProvider.CreateServiceLocator(Context);
                    return locator;
                }
                catch (Exception exp)
                {
                    Log.Error("Creating service locator occur error.", exp);
                }
            }

            return null;
        }

        private static IServiceContext CreateServiceContext()
        {
            if (ContextProvider != null)
            {
                try
                {
                    var container = ContextProvider.CreateServiceContext();
                    return container;
                }
                catch (Exception exp)
                {
                    Log.Error("Creating service context occur error.", exp);
                }
            }

            return null;
        }

        private static IServiceContextProvider GetServiceContextProvider()
        {
            var section = GetConfigurationSection();
            if (section.ContextProvider != null && section.ContextProvider.Type != null)
            {
                //throw new ConfigurationErrorsException(string.Format("Configure {0}/{1} has not value.",
                //    Configuration.DContainerSection.DContainerSectionName,
                //    Configuration.DContainerSection.ServiceLocatorProviderElementName));
                try
                {
                    var provider = section.ContextProvider.CreateInstance<IServiceContextProvider>();
                    return provider;
                }
                catch (Exception exp)
                {
                    Log.Error("Creating service context provider occur error.", exp);
                }
            }
            return null;
        }

        #endregion
    }
}
