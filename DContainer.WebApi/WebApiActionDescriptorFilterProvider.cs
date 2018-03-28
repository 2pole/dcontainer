using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DContainer.WebApi
{
    public class WebApiActionDescriptorFilterProvider : IFilterProvider
    {
        private readonly ActionDescriptorFilterProvider _filterProvider = new ActionDescriptorFilterProvider();

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var locator = Locator.Current;
            var filters = _filterProvider.GetFilters(configuration, actionDescriptor).ToArray();
            foreach (FilterInfo info in filters)
            {
                locator.InjectProperties(info.Instance);
            }
            return filters;
        }
    }
}
