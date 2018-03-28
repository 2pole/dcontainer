using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Handlers;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.MicroKernel.SubSystems.Conversion;
using Castle.Windsor;

namespace DContainer.Castle
{
    public class PerContainerScopeAccessor : IScopeAccessor
    {
        private readonly static Func<CreationContext, IKernel> KernelGetter;
        private readonly ThreadSafeDictionary<IKernel, ILifetimeScope> _items = new ThreadSafeDictionary<IKernel, ILifetimeScope>();

        static PerContainerScopeAccessor()
        {
            KernelGetter = BuildKernelGetter();
        }

        public void Dispose()
        {
            var values = _items.EjectAllValues();
            foreach (var item in values.Reverse())
            {
                item.Dispose();
            }
        }

        public ILifetimeScope GetScope(CreationContext context)
        {
            var currentKernel = GetCurrentKernel(context);
            return _items.GetOrAdd(currentKernel, CreateLifetimeScope);
        }

        protected virtual ILifetimeScope CreateLifetimeScope(IKernel kernel)
        {
            kernel.RemovedAsChildKernel += RemovedAsChildKernel;
            return new DefaultLifetimeScope();
        }

        private void RemovedAsChildKernel(object sender, EventArgs e)
        {
            var kernel = sender as IKernel;
            if (kernel != null)
            {
                _items.Remove(kernel, scope => scope.Dispose());

                kernel.RemovedAsChildKernel -= RemovedAsChildKernel;
            }
        }

        protected virtual IKernel GetCurrentKernel(CreationContext context)
        {
            var kernel = KernelGetter(context);
            return kernel;
        }

        #region Build Kernel Getter
        private static Func<CreationContext, IKernel> BuildKernelGetter()
        {
            var pa = Expression.Parameter(typeof(CreationContext), "pa");
            var returnValue = Expression.Variable(typeof(IKernel), "kernel");
            var returnTarget = Expression.Label(typeof(IKernel));

            var blockForTypeConverter = BuildKernelGetterFromTypeConverter(pa, returnValue);
            var blockForHandler = BuildKernelGetterFromHandler(pa, returnValue);
            var kernelGetterFromResolutionStack = BuildKernelGetterFromResolutionStack(pa, returnValue);
            var blockExp = Expression.Block(
                new[] { returnValue },
                blockForTypeConverter,
                Expression.IfThen(Expression.NotEqual(returnValue, Expression.Constant(null, typeof(IKernel))),
                    Expression.Return(returnTarget, returnValue)),
                kernelGetterFromResolutionStack,
                Expression.IfThen(Expression.NotEqual(returnValue, Expression.Constant(null, typeof(IKernel))),
                    Expression.Return(returnTarget, returnValue)),
                blockForHandler,
                Expression.Return(returnTarget, returnValue),
                Expression.Label(returnTarget, Expression.Constant(null, typeof(IKernel)))
            );

            var body = Expression.Lambda<Func<CreationContext, IKernel>>(blockExp, pa);
            var func = body.Compile();
            return func;
        }

        private static BlockExpression BuildKernelGetterFromTypeConverter(ParameterExpression pa, ParameterExpression kernelPa)
        {
            var converterVar = Expression.Variable(typeof(ITypeConverter), "converter");
            var converterField = typeof(CreationContext).GetField("converter", BindingFlags.NonPublic | BindingFlags.Instance);
            var blockExp = Expression.Block(
                new[] { converterVar },
                Expression.Assign(converterVar, Expression.Field(pa, converterField)),
                Expression.IfThen(
                    Expression.NotEqual(converterVar, Expression.Constant(null, typeof(ITypeConverter))),
                    Expression.Assign(kernelPa,
                            Expression.Property(
                                Expression.Property(converterVar, "Context"), "Kernel")))
            );
            return blockExp;
        }

        private static BlockExpression BuildKernelGetterFromHandler(ParameterExpression pa, ParameterExpression kernalPa)
        {
            var kernelProperty = typeof(AbstractHandler).GetProperty("Kernel", BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerVar = Expression.Variable(typeof(AbstractHandler), "handler");
            var blockForHandler = BuildAbstractHandlerGetter(pa, handlerVar);
            var blockExp = Expression.Block(
                new[] { handlerVar },
                blockForHandler,
                Expression.IfThen(
                    Expression.NotEqual(handlerVar, Expression.Constant(null, typeof(AbstractHandler))),
                    Expression.Assign(kernalPa, Expression.Property(handlerVar, kernelProperty)))
            );
            return blockExp;
        }

        private static Expression BuildKernelGetterFromResolutionStack(ParameterExpression pa, ParameterExpression kernelPa)
        {
            var resolutionsField = typeof(CreationContext).GetField("resolutionStack", BindingFlags.Instance | BindingFlags.NonPublic);
            var resolutionsFieldExp = Expression.Field(pa, resolutionsField);

            var selectMethod = typeof(Enumerable).GetGenericMethod("Select", new[] { typeof(CreationContext.ResolutionContext), typeof(IKernel) }, new[] { typeof(IEnumerable<CreationContext.ResolutionContext>), typeof(Func<CreationContext.ResolutionContext, IKernel>) });
            var firstOrDefaultMethod = typeof(Enumerable).GetGenericMethod("FirstOrDefault", new[] { typeof(IKernel) }, new[] { typeof(IEnumerable<IKernel>), typeof(Func<IKernel, bool>) });

            var resolutionPa = Expression.Parameter(typeof(CreationContext.ResolutionContext));
            var kernelGetterExp = BuildKernelGetterFromResolution(resolutionPa);
            var kernelGetterBody = Expression.Lambda<Func<CreationContext.ResolutionContext, IKernel>>(kernelGetterExp, resolutionPa);
            var kernelGetter = kernelGetterBody.Compile();

            var kernelFilterPa = Expression.Parameter(typeof(IKernel));
            var kernelFilterExp = Expression.NotEqual(kernelFilterPa, Expression.Constant(null, typeof(IKernel)));
            var kernelFilterBody = Expression.Lambda<Func<IKernel, bool>>(kernelFilterExp, kernelFilterPa);
            var kernelFilter = kernelFilterBody.Compile();

            var selectExp = Expression.Call(selectMethod, resolutionsFieldExp, Expression.Constant(kernelGetter));
            var firstExp = Expression.Call(firstOrDefaultMethod, selectExp, Expression.Constant(kernelFilter));
            var bodyExp = Expression.Assign(kernelPa, firstExp);
            return bodyExp;
        }

        private static Expression BuildKernelGetterFromResolution(ParameterExpression pa)
        {
            var returnTarget = Expression.Label(typeof(IKernel));
            var contextVar = Expression.Variable(typeof(CreationContext), "context");
            var kernelVar = Expression.Variable(typeof(IKernel), "kernel");

            var kernelGetterExp = BuildKernelGetterFromTypeConverter(contextVar, kernelVar);
            var bodyExp = Expression.Block(
                new[] { contextVar, kernelVar },
                Expression.Assign(contextVar, Expression.Property(pa, "Context")),
                Expression.IfThen(
                    Expression.NotEqual(contextVar, Expression.Constant(null, typeof(CreationContext))),
                    kernelGetterExp),
                Expression.Return(returnTarget, kernelVar),
                Expression.Label(returnTarget, Expression.Constant(null, typeof(IKernel)))
           );

            return bodyExp;
        }

        private static BlockExpression BuildAbstractHandlerGetter(ParameterExpression pa, ParameterExpression handler)
        {
            var abstractHandlerVar = Expression.Variable(typeof(AbstractHandler), "handler");
            var parentWrapperHandlerVar = Expression.Variable(typeof(ParentHandlerWrapper), "parentWrapperHandler");
            var parentHandlerField = typeof(ParentHandlerWrapper).GetField("parentHandler", BindingFlags.Instance | BindingFlags.NonPublic);
            var handlerPropertyExp = Expression.Property(pa, "Handler");
            var blockExp = Expression.Block(
                new[] { abstractHandlerVar, parentWrapperHandlerVar },
                Expression.Assign(abstractHandlerVar, Expression.TypeAs(handlerPropertyExp, typeof(AbstractHandler))),
                Expression.IfThen(
                    Expression.Equal(abstractHandlerVar, Expression.Constant(null, typeof(AbstractHandler))),
                    Expression.Block(
                        Expression.Assign(parentWrapperHandlerVar, Expression.TypeAs(handlerPropertyExp, typeof(ParentHandlerWrapper))),
                        Expression.IfThen(
                            Expression.NotEqual(parentWrapperHandlerVar, Expression.Constant(null, typeof(ParentHandlerWrapper))),
                            Expression.Assign(abstractHandlerVar, Expression.TypeAs(Expression.Field(parentWrapperHandlerVar, parentHandlerField), typeof(AbstractHandler)))
                        ))),
                Expression.Assign(handler, abstractHandlerVar));
            return blockExp;
        }

        #endregion
    }
}
