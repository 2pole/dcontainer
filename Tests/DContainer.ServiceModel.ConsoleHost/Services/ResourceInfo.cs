using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DContainer.ServiceModel.ConsoleHost.Services
{
    [DataContract]
    public class ResourceInfo
    {
        [DataMember]
        public int ResourceId { get; set; }

        [DataMember]
        public string ResourceName { get; set; }

        [DataMember]
        public byte[] Content { get; set; }

        [DataMember]
        public int ResourceType { get; set; }

        [DataMember]
        public string ContentType { get; set; }

        [DataMember]
        public string Remark { get; set; }
    }
}
