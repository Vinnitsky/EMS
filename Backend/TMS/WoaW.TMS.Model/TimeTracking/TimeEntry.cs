using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.TMS.Model.TimeTracking
{
    /// <summary>
    /// класс инкапсулирует абстракцию одной записи в Timesheet
    /// </summary>
    public class TimeEntry
    {
        public DateTime From { get; set; }
        public DateTime? Thru { get; set; }
        public TimeSpan? Hours { get; set; }
        public string Comment { get; set; }

        public Guid SpentOnWorkEffortId { get; set; }
    }
}
