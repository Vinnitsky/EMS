using System;
using WoaW.CMS.DAL.EF;

namespace WoaW.TMS.Model.DAL
{
    public interface ITask 
    {
        string Id { get; set; }
        string TaskTypeId { get; }

        string DataOjectId { get; }

        int Result { get; set; }

        string Subject { get; set; }

        string Text { get; set; }
        /// <summary>
        /// приорите задачи, это целое число с дискретностью 1, чем меньше число тем выше приоритет 
        /// т.е. зача с приоритетом 0 выше приоритетом чем задача с приоритетом 1.
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// время когда запланирован старт
        /// </summary>
        DateTime ScheduledStartTime { get; set; }
        /// <summary>
        /// Время когда задача должна быть выполнена
        /// </summary>
        DateTime? DueDateTime { get; set; }

        PartyIdentity AssignedToParty { get; }

        EWorkEffortStatus Status { get; }
    }
}
