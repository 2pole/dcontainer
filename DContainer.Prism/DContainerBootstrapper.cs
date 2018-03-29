using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using DContainer.Prism.Properties;
using Prism;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using MicrosoftLocation = CommonServiceLocator;

namespace DContainer.Prism
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DContainerBootstrapper : Bootstrapper
    {
        private bool _useDefaultConfiguration = true;

        #region Properties

        public IServiceRegister ServiceRegister
        {
            get { return Locator.Register; }
        }

        public IServiceLocator ServiceLocator
        {
            get { return Locator.Current; }
        }

        #endregion

        #region Bootstrapper
        public override void Run(bool runWithDefaultConfiguration)
        {
            this._useDefaultConfiguration = runWithDefaultConfiguration;
            this.Logger = this.CreateLogger();
            if (this.Logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }
            this.Logger.Log(Resources.LoggerCreatedSuccessfully, Category.Debug, Priority.Low);

            this.Logger.Log(Resources.ValdatingServiceLocation, Category.Debug, Priority.Low);
            this.ValidationServiceLocation();

            this.Logger.Log(Resources.CreatingModuleCatalog, Category.Debug, Priority.Low);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null)
            {
                throw new InvalidOperationException(Resources.NullModuleCatalogException);
            }

            this.Logger.Log(Resources.ConfiguringModuleCatalog, Category.Debug, Priority.Low);
            this.ConfigureModuleCatalog();

            this.Logger.Log(Resources.ConfiguringSpringContainer, Category.Debug, Priority.Low);
            this.ConfigureServices();

            this.Logger.Log(Resources.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            this.ConfigureServiceLocator();

            this.Logger.Log(Resources.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
            this.ConfigureRegionAdapterMappings();

            this.Logger.Log(Resources.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
            this.ConfigureDefaultRegionBehaviors();

            this.Logger.Log(Resources.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);
            this.RegisterFrameworkExceptionTypes();

            this.Logger.Log(Resources.CreatingShell, Category.Debug, Priority.Low);
            this.Shell = this.CreateShell();
            if (this.Shell != null)
            {
                this.Logger.Log(Resources.SettingTheRegionManager, Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(this.Shell, ServiceLocator.Resolve<IRegionManager>());

                this.Logger.Log(Resources.UpdatingRegions, Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                this.Logger.Log(Resources.InitializingShell, Category.Debug, Priority.Low);
                this.InitializeShell();
            }

            if (ServiceRegister.IsRegistered<IModuleManager>())
            {
                this.Logger.Log(Resources.InitializingModules, Category.Debug, Priority.Low);
                this.InitializeModules();
            }

            this.Logger.Log(Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            Locator.SetCurrentLocatorProvider(() => Locator.Root);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Configures the IoC container for use
        /// </summary>
        protected virtual void ConfigureServices()
        {
            var register = Locator.Register;
            register.RegisterInstanceIfMissing<ILoggerFacade>(this.Logger);
            register.RegisterInstanceIfMissing<IModuleCatalog>(this.ModuleCatalog);
            register.RegisterInstanceIfMissing<IServiceContext>(Locator.Context);
            register.RegisterInstanceIfMissing<IServiceRegister>(Locator.Register);
            register.RegisterInstanceIfMissing<MicrosoftLocation.IServiceLocator>(Locator.Root as MicrosoftLocation.IServiceLocator);

            if (this._useDefaultConfiguration)
            {
                register.RegisterInstanceIfMissing<MicrosoftLocation.IServiceLocator>(Locator.Root as MicrosoftLocation.IServiceLocator);
                register.RegisterTypeIfMissing<IModuleInitializer, ModuleInitializer>(LifetimeScope.Singleton);
                register.RegisterTypeIfMissing<IModuleManager, ModuleManager>(LifetimeScope.Singleton);
                register.RegisterTypeIfMissing<RegionAdapterMappings>(LifetimeScope.Singleton);
                register.RegisterTypeIfMissing<IRegionManager, RegionManager>(LifetimeScope.Singleton);
                register.RegisterTypeIfMissing<IEventAggregator, EventAggregator>(LifetimeScope.Singleton);
                register.RegisterTypeIfMissing<IRegionViewRegistry, RegionViewRegistry>(LifetimeScope.Singleton);
                register.RegisterTypeIfMissing<IRegionBehaviorFactory, RegionBehaviorFactory>(LifetimeScope.Singleton);
                register.RegisterTypeIfMissing<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>(LifetimeScope.Transient);
                register.RegisterTypeIfMissing<IRegionNavigationJournal, RegionNavigationJournal>(LifetimeScope.Transient);
                register.RegisterTypeIfMissing<IRegionNavigationService, RegionNavigationService>(LifetimeScope.Transient);
                register.RegisterTypeIfMissing<IRegionNavigationContentLoader, RegionNavigationContentLoader>(LifetimeScope.Singleton);
            }
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = ServiceLocator.Resolve<IModuleManager>();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                {
                    throw new InvalidOperationException(Resources.NullModuleCatalogException);
                }

                throw;
            }

            manager.Run();
        }

        protected abstract ILoggerFacade CreateLogger();

        #endregion

        #region Private methods
        protected virtual void ValidationServiceLocation()
        {
            if (Locator.Context == null)
                throw new InvalidOperationException(Resources.NullServiceContextException);

            if (Locator.Root == null)
                throw new InvalidOperationException(Resources.NullServiceLocatorException);

            if (Locator.Register == null)
                throw new InvalidOperationException(Resources.NullServiceRegisterException);
        }
        #endregion
    }
}
