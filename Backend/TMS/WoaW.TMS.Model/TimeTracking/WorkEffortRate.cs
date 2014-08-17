using System;

namespace WoaW.TMS.Model.TimeTracking
{
    public class WorkEffortRate
    {
        public string Id { get; set; }
        public DateTime From { get; set; }
        public DateTime? Thru { get; set; }
        public float Rate { get; set; }
        public string Comment { get; set; }
        public EWorkEffortRateType Type { get; set; }
    }
}
