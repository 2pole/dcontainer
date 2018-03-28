using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Mvc
{
    /// <summary>
    /// Indicates what types a model binder supports.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class ModelBinderTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the target types.
        /// </summary>
        public IEnumerable<Type> TargetTypes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBinderTypeAttribute"/> class.
        /// </summary>
        /// <param name="targetTypes">The target types.</param>
        public ModelBinderTypeAttribute(params Type[] targetTypes)
        {
            if (targetTypes == null) throw new ArgumentNullException("targetTypes");
            TargetTypes = targetTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBinderTypeAttribute"/> class.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        public ModelBinderTypeAttribute(Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");
            TargetTypes = new Type[] { targetType };
        }
    }
}
