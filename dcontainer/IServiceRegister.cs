using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public interface IServiceRegister
    {
        IServiceContext ServiceContext { get; }

        #region Register Instance
        IServiceRegister RegisterInstance(object instance);
        IServiceRegister RegisterInstance(Type t, object instance);
        IServiceRegister RegisterInstance(Type t, string name, object instance);

        IServiceRegister RegisterInstance<TInterface>(TInterface instance);
        IServiceRegister RegisterInstance<TInterface>(string name, TInterface instance);
        #endregion

        #region Register Type
        IServiceRegister RegisterType(Type t);
        IServiceRegister RegisterType(Type t, LifetimeScope scope);
        IServiceRegister RegisterType(Type t, string name);
        IServiceRegister RegisterType(Type t, string name, LifetimeScope scope);

        IServiceRegister RegisterType(Type fromType, Type toType);
        IServiceRegister RegisterType(Type fromType, Type toType, LifetimeScope scope);
        IServiceRegister RegisterType(Type fromType, Type toType, string name);
        IServiceRegister RegisterType(Type fromType, Type toType, string name, LifetimeScope scope);

        IServiceRegister RegisterType<TFrom, TTo>() where TTo : TFrom;
        IServiceRegister RegisterType<TFrom, TTo>(LifetimeScope scope) where TTo : TFrom;
        IServiceRegister RegisterType<TFrom, TTo>(string name) where TTo : TFrom;
        IServiceRegister RegisterType<TFrom, TTo>(string name, LifetimeScope scope) where TTo : TFrom;
        #endregion

        #region IsRegistered
        bool IsRegistered(Type typeToCheck);
        bool IsRegistered(Type typeToCheck, string nameToCheck);
        bool IsRegistered<T>();
        bool IsRegistered<T>(string nameToCheck);
        #endregion
    }
}
