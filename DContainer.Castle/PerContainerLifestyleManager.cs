using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace DContainer.Castle
{
    public class PerContainerLifestyleManager : ScopedLifestyleManager
    {
        public PerContainerLifestyleManager()
            : base(new PerContainerScopeAccessor())
        {
        }
    }
}
