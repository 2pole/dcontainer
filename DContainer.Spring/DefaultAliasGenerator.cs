using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Spring
{
    public class DefaultAliasGenerator : IAliasGenerator
    {
        public string GetAlias(Type type)
        {
            return type.FullName;
        }
    }
}
