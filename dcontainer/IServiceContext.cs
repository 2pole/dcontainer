using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public interface IServiceContext : IDisposable
    {
        object Container { get; }
    }

    public interface IServiceContext<TContainer> : IServiceContext
        where TContainer : class
    {
        TContainer Container { get;  }
    }
}
