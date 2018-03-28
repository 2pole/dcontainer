using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DContainer.Castle
{
    public static class TypeExtensions
    {
        public static MethodInfo GetGenericMethod(this Type type, string name, Type[] genericTypeArgs, Type[] paramTypes, bool complain = true)
        {
            foreach (MethodInfo m in type.GetMethods())
            {
                if (m.Name == name)
                {
                    ParameterInfo[] pa = m.GetParameters();
                    if (pa.Length == paramTypes.Length)
                    {
                        MethodInfo c = m.MakeGenericMethod(genericTypeArgs);
                        if (c.GetParameters().Select(p => p.ParameterType).SequenceEqual(paramTypes))
                            return c;
                    }
                }
            }
            if (complain)
                throw new Exception("Could not find a method matching the signature " + type + "." + name +
                  "<" + String.Join(", ", genericTypeArgs.AsEnumerable()) + ">" +
                  "(" + String.Join(", ", paramTypes.AsEnumerable()) + ").");
            return null;
        }
    }
}
