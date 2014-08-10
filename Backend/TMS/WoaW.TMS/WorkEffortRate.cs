using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoaW.TMS.Tasks
{
    public class WorkEffortRate
    {
        public DateTime From { get; set; }
        public DateTime? Thru { get; set; }
        public float Rate { get; set; }
        public string Comment { get; set; }
        public EWorkEffortRateType Type { get; set; }
    }
}
