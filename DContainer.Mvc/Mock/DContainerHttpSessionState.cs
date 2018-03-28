using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DContainer.Mvc.Mock
{
    public class DContainerHttpSessionState : HttpSessionStateWrapper 
    {
        public DContainerHttpSessionState()
            : base(HttpContext.Current.Session)
        {
        }
    }
}
