using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DContainer.Configuration
{
    public class DContainerElement : ConfigurationElement
    {
        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty("type", IsRequired=false)]
        public Type Type
        {
            get
            {
                return (Type)this["type"]; 
            }
        }

        public TInstance CreateInstance<TInstance>()
        {
            return (TInstance)Activator.CreateInstance(this.Type);
        }
    }
}
