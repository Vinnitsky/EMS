using System;
using System.Collections.Generic;
using WoaW.CMS.Model;

namespace WoaW.TMS.Model.TimeTracking
{
    /// <summary>
    /// класс инкапулирует абстракцию календаря. в данной реализации заглушка 
    /// </summary>
    public class Timesheet
    {
        public DateTime From { get; set; }
        public DateTime? Thru { get; set; }
        virtual public List<TimeEntry> ComposedOf { get; set; }
        virtual public Party Party { get; set; }

        public Timesheet()
        {
            ComposedOf = new List<TimeEntry>();
        }

    }
}
