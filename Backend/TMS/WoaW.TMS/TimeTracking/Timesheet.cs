using System;
using System.Collections.Generic;
using WoaW.CRM.Model;

namespace WoaW.TMS.Tasks.TimeTracking
{
    /// <summary>
    /// класс инкапулирует абстракцию календаря. в данной реализации заглушка 
    /// </summary>
    public class Timesheet
    {
        public DateTime From { get; set; }
        public DateTime? Thru { get; set; }
        public List<TimeEntry> ComposedOf { get; set; }
        public Party Party1 { get; set; }

        public Timesheet()
        {
            ComposedOf = new List<TimeEntry>();
        }

    }
}
