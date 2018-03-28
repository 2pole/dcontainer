using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DContainer
{
    public class AutowiringPropertyInjector
    {
        private static readonly IDictionary<Type, Action<IServiceLocator, IServiceRegister, object, bool>> InjectPropertiesSetters = new Dictionary<Type, Action<IServiceLocator, IServiceRegister, object, bool>>();

        private static Action<IServiceLocator, IServiceRegister, object, bool> BuildPropertiesSetter(Type type)
        {
            var properties = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             let pt = p.PropertyType
                             where
                                 p.CanWrite &&
                                 !(pt.IsValueType || pt.IsEnum) &&
                                 p.GetIndexParameters().Length == 0
                             select p;

            var locatorPar = Expression.Parameter(typeof(IServiceLocator), "locator");
            var registerPar = Expression.Parameter(typeof(IServiceRegister), "register");
            var instancePar = Expression.Parameter(typeof(object), "instance");
            var overrideSetPar = Expression.Parameter(typeof(bool), "overrideSet");
            var instanceVar = Expression.Variable(type, "instance");

            List<Expression> expressions = new List<Expression>();
            var isRegisteredMethod = typeof(IServiceRegister).GetMethod("IsRegistered", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Type) }, new ParameterModifier[0]);
            var resolveMethod = typeof(IServiceLocator).GetMethod("Resolve", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], new ParameterModifier[0]);

            expressions.Add(Expression.Assign(instanceVar, Expression.TypeAs(instancePar, type)));
            foreach (var p in properties)
            {
                Expression andExp = null;
                var isRegisterExp = Expression.Call(registerPar, isRegisteredMethod, Expression.Constant(p.PropertyType));
                var getValueExp = Expression.Call(locatorPar, resolveMethod.MakeGenericMethod(p.PropertyType));
                var propertySetterExp = Expression.Call(instanceVar, p.GetSetMethod(), getValueExp);
                if (p.CanRead)
                {
                    var propertyIsNullExp = Expression.Equal(Expression.Property(instanceVar, p), Expression.Constant(null, p.PropertyType));
                    var needSetPropertyExp = Expression.Or(overrideSetPar, propertyIsNullExp);
                    andExp = Expression.And(isRegisterExp, needSetPropertyExp);
                }
                else
                {
                    andExp = Expression.And(isRegisterExp, overrideSetPar);
                }
                var ifThenExp = Expression.IfThen(andExp, propertySetterExp);
                expressions.Add(ifThenExp);
            }

            var blockExp = Expression.Block(new[] { instanceVar }, expressions);
            var parameters = new[] {locatorPar, registerPar, instancePar, overrideSetPar};
            var body = Expression.Lambda<Action<IServiceLocator, IServiceRegister, object, bool>>(blockExp, parameters);
            var action = body.Compile();
            return action;
        }

        private static Action<IServiceLocator, IServiceRegister, object, bool> GetOrCreatePropertiesSetter(Type type)
        {
            Action<IServiceLocator, IServiceRegister, object, bool> action = null;
            if (!InjectPropertiesSetters.TryGetValue(type, out action))
            {
                action = BuildPropertiesSetter(type);
                InjectPropertiesSetters[type] = action;
            }
            return action;
        }

        public static void InjectProperties(IServiceLocator locator, object instance, bool overrideSetValues)
        {
            if (locator == null)
                throw new ArgumentNullException("locator");
            if (instance == null)
                throw new ArgumentNullException("instance");

            var register = locator.Resolve<IServiceRegister>() ?? Locator.Register;
            var action = GetOrCreatePropertiesSetter(instance.GetType());
            action(locator, register, instance, overrideSetValues);
        }

        private static void InjectPropertiesByReflection(IServiceLocator locator, object instance, bool overrideSetValues)
        {
            if (locator == null)
                throw new ArgumentNullException("locator");
            if (instance == null)
                throw new ArgumentNullException("instance");
            var serviceRegister = Locator.Register;

            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(pi => pi.CanWrite))
            {
                Type propertyType = propertyInfo.PropertyType;
                if ((!propertyType.IsValueType || propertyType.IsEnum) && (propertyInfo.GetIndexParameters().Length == 0 && serviceRegister.IsRegistered(propertyType)))
                {
                    MethodInfo[] accessors = propertyInfo.GetAccessors(false);
                    if ((accessors.Length != 1 || !(accessors[0].ReturnType != typeof(void))) && (overrideSetValues || accessors.Length != 2 || propertyInfo.GetValue(instance, null) == null))
                    {
                        object obj = locator.Resolve(propertyType);
                        propertyInfo.SetValue(instance, obj, null);
                    }
                }
            }
        }
    }
}
