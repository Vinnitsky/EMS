using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EzBpm.Tms.ConfigModel
{
    [DataContract]
    [Serializable]
    public class TaskResultEnumItemModel
    {
        [DataMember]
        [XmlAttribute]
        public int Value { get; set; }

        [DataMember]
        [XmlAttribute]
        public string Title { get; set; }

        [DataMember]
        [XmlAttribute]
        public bool Success { get; set; }
    }
}