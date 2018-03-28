using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DContainer.Mvc.Mock
{
    public class DContainerHttpBrowserCapabilities : HttpBrowserCapabilitiesWrapper
    {
        public DContainerHttpBrowserCapabilities()
            : base(HttpContext.Current.Request.Browser)
        {
        }
    }
}
