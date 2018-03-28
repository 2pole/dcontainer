using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public abstract class ServiceRegisterImplBase : IServiceRegister
    {
        public IServiceContext ServiceContext { get; protected set; }

        protected ServiceRegisterImplBase(IServiceContext context)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            this.ServiceContext = context;
        }

        #region RegisterInstance
        public virtual IServiceRegister RegisterInstance(object instance)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            return RegisterInstance(instance.GetType(), null, instance);
        }

        public virtual IServiceRegister RegisterInstance(Type t, object instance)
        {
            return RegisterInstance(t, null, instance);
        }

        public virtual IServiceRegister RegisterInstance<TInterface>(TInterface instance)
        {
            return RegisterInstance<TInterface>(null, instance);
        }

        public IServiceRegister RegisterInstance<TInterface>(string name, TInterface instance)
        {
            return RegisterInstance(typeof(TInterface), name, instance);
        }

        #endregion

        #region RegisterType
        public virtual IServiceRegister RegisterType(Type t)
        {
            return RegisterType(t, (string)null, LifetimeScope.Transient);
        }

        public virtual IServiceRegister RegisterType(Type t, LifetimeScope scope)
        {
            return RegisterType(t, (string)null, scope);
        }

        public virtual IServiceRegister RegisterType(Type t, string name)
        {
            return RegisterType(t, name, LifetimeScope.Transient);
        }

        public virtual IServiceRegister RegisterType(Type fromType, Type toType)
        {
            return RegisterType(fromType, toType, null, LifetimeScope.Transient);
        }

        public virtual IServiceRegister RegisterType(Type fromType, Type toType, LifetimeScope scope)
        {
            return RegisterType(fromType, toType, null, scope);
        }

        public virtual IServiceRegister RegisterType(Type fromType, Type toType, string name)
        {
            return RegisterType(fromType, toType, name, LifetimeScope.Transient);
        }

        public virtual IServiceRegister RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            return RegisterType<TFrom, TTo>(LifetimeScope.Transient);
        }

        public virtual IServiceRegister RegisterType<TFrom, TTo>(LifetimeScope scope) where TTo : TFrom
        {
            return RegisterType<TFrom, TTo>(null, scope);
        }

        public virtual IServiceRegister RegisterType<TFrom, TTo>(string name) where TTo : TFrom
        {
            return RegisterType<TFrom, TTo>(name, LifetimeScope.Transient);
        }

        public virtual IServiceRegister RegisterType<TFrom, TTo>(string name, LifetimeScope scope) where TTo : TFrom
        {
            var typeFrom = typeof(TFrom);
            var typeTo = typeof(TTo);
            return RegisterType(typeFrom, typeTo, name, scope);
        }
        #endregion

        #region IsRegistered
        public virtual bool IsRegistered(Type typeToCheck)
        {
            return IsRegistered(typeToCheck, null);
        }

        public virtual bool IsRegistered<T>()
        {
            return IsRegistered<T>(null);
        }

        public virtual bool IsRegistered<T>(string nameToCheck)
        {
            return IsRegistered(typeof(T), nameToCheck);
        }
        #endregion

        public abstract bool IsRegistered(Type typeToCheck, string nameToCheck);
        public abstract IServiceRegister RegisterType(Type fromType, Type toType, string name, LifetimeScope scope);
        public abstract IServiceRegister RegisterType(Type typeToRegister, string name, LifetimeScope scope);
        public abstract IServiceRegister RegisterInstance(Type typeToRegister, string name, object instance);
    }

    public abstract class ServiceRegisterImplBase<TContainer> : ServiceRegisterImplBase
        where TContainer : class 
    {
        protected ServiceRegisterImplBase(IServiceContext<TContainer> context)
            : base(context)
        {
        }

        public TContainer Container
        {
            get { return (TContainer)base.ServiceContext.Container; }
        }
    }
}
