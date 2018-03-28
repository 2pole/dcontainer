using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace DContainer.Mvc
{
    /// <summary>
    /// Container class for the ASP.NET application startup method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PreApplicationStartCode
    {
        private static bool _startWasCalled;

        /// <summary>
        /// Performs ASP.NET application startup logic early in the pipeline.
        /// </summary>
        public static void Start()
        {
            // Guard against multiple calls. All Start calls are made on the same thread, so no lock needed here.
            if (_startWasCalled) return;

            _startWasCalled = true;
            DynamicModuleUtility.RegisterModule(typeof(RequestLifetimeHttpModule));
        }
    }
}
