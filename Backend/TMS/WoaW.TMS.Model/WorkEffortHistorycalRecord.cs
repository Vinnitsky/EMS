using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.TMS.Model
{
    public class WorkEffortHistorycalRecord
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string ManagerId { get; set; }
        public string EmployeeId { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public EWorkEffortStatus Status { get; set; }
        public string OperationComment { get; set; }
    }
}
