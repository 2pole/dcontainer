using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Spring
{
    public interface IAliasGenerator
    {
        string GetAlias(Type type);
    }
}
