using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DContainer
{
    public sealed class AssertUtils
    {
        public static void ArgumentNotNull(object argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name,
                    string.Format((IFormatProvider)CultureInfo.InvariantCulture, "Argument '{0}' cannot be null.", new object[1] { name }));
        }
    }
}
