using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer
{
    public enum LifetimeScope
    {
        /// <summary>
        /// instantiated per usage
        /// </summary>
        Transient,

        /// <summary>
        /// only one instance
        /// </summary>
        Singleton,

        /// <summary>
        /// instantiated per IoC container
        /// </summary>
        PerContainer,
    }
}
