using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DContainer.Configuration
{
    public class DContainerSection : ConfigurationSection
    {
        public const string DContainerSectionName = "dcontainer";
        public const string ServiceContextProviderElementName = "contextProvider";

        [ConfigurationProperty(ServiceContextProviderElementName, IsRequired = false)]
        public DContainerElement ContextProvider
        {
            get { return (DContainerElement)base[ServiceContextProviderElementName]; }
            set { base[ServiceContextProviderElementName] = value; }
        }
    }
}
