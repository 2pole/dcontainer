using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DContainer.Mvc.Mock
{
    public class DContainerHttpContext : HttpContextWrapper
    {
        public DContainerHttpContext()
            : base(HttpContext.Current)
        {
        }
    }
}
