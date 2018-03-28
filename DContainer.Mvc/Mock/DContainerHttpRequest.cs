using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DContainer.Mvc.Mock
{
    public class DContainerHttpRequest : HttpRequestWrapper
    {
        public DContainerHttpRequest()
            : base(HttpContext.Current.Request)
        {
        }
    }
}
