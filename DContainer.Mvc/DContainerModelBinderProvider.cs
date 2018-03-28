using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DContainer.Mvc
{
    /// <summary>
    /// Service Location implementation of the <see cref="IModelBinderProvider"/> interface.
    /// </summary>
    public class DContainerModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Metadata key for the supported model types.
        /// </summary>
        internal static readonly string MetadataKey = "SupportedModelTypes";

        /// <summary>
        /// Gets the model binder associated with the provided model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>An <see cref="IModelBinder"/> instance if found; otherwise, <c>null</c>.</returns>
        public IModelBinder GetBinder(Type modelType)
        {
            //var modelBinders = DependencyResolver.Current.GetServices<Meta<Lazy<IModelBinder>>>();

            //var modelBinder = modelBinders
            //    .Where(binder => binder.Metadata.ContainsKey(MetadataKey))
            //    .FirstOrDefault(binder => ((List<Type>)binder.Metadata[MetadataKey]).Contains(modelType));
            //return (modelBinder != null) ? modelBinder.Value.Value : null;
            return null;
        }
    }
}
