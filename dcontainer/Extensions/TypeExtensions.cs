using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DContainer
{
    /// <summary>
    /// Contains various extension methods for types.
    /// </summary>
    public static class TypeExtensions
    {
        public static bool IsDelegate(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return type.IsSubclassOf(typeof(Delegate));
        }

        public static bool CanResolve(this Type type)
        {
            return type.IsClass && !type.IsAbstract && !type.IsGenericTypeDefinition;
        }
    }
}
