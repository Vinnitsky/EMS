using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EzBpm.Tms.ConfigModel
{
    [DataContract]
    [Serializable]
    public class TaskModel 
    {
        [DataMember]
        [XmlAttribute]
        public string Title { get; set; }

        [DataMember]
        [XmlArray("results")]
        [XmlArrayItem("result")]
        public List<TaskResultEnumItemModel> Results { get; set; }

        [DataMember]
        [XmlAttribute]
        public string Superviser { get; set; }

        [DataMember]
        [XmlAttribute]
        public string Employee { get; set; }

        [DataMember]
        [XmlAttribute]
        public string Manager { get; set; }

        [DataMember]
        [XmlAttribute]
        public int ExpectedDurationMin { get; set; }

        public string Id { get; set; }
    }
}