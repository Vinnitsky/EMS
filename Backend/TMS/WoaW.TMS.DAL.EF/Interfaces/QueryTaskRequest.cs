using System;
using System.Collections.Generic;

namespace WoaW.TMS.Model.DAL
{
    public struct DateTimeRange
    {
        public DateTime? From { get; set; }
        public DateTime? Till { get; set; }
    }

    public class QueryTaskRequest
    {
        /// <summary>
        /// Can be null
        /// </summary>
        public DateTimeRange CreatedOn { get; set; }

        /// <summary>
        /// Can ne null
        /// </summary>
        public DateTimeRange ScheduledStartTime { get; set; }

        /// <summary>
        /// Can ne null
        /// </summary>
        public DateTimeRange DueDateTime { get; set; }

        /// <summary>
        /// Can ne null
        /// </summary>
        public DateTimeRange ActualStartTime { get; set; }

        /// <summary>
        /// Can be null
        /// </summary>
        public List<string> TaskTypeIDs { get; set; }

        /// <summary>
        /// Can be null
        /// </summary>
        public List<string> AssignToIDs { get; set; }
        /// <summary>
        /// Can be null
        /// </summary>
        public List<string> ManagerIDs { get; set; }
        /// <summary>
        /// Can be null
        /// </summary>
        public List<EWorkEffortStatus> Statuses { get; set; }

        public bool? WithOverdue { get; set; }

        /// <summary>
        /// Contains
        /// </summary>
        public string Subject { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortField { get; set; }

        public string SortDirection { get; set; }
    }
}