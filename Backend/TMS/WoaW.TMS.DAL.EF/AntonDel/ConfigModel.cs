using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using EzBpm.Tms.ConfigModel;

namespace WoaW.Tms.DAL.EF
{
    [XmlRoot("model", IsNullable = false)]
    [Serializable]
    public class ConfigModel
    {

        [XmlArray("tasks")]
        [XmlArrayItem("task")]
        public List<TaskModel> Tasks { get; set; }

    }
}