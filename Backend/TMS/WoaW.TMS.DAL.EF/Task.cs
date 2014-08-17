using WoaW.CMS.DAL.EF;

namespace WoaW.TMS.Model.DAL
{
    public class Task : WorkEffort, ITask
    {
        #region ITask implementation
        public string TaskTypeId
        {
            get
            {
                return base.Type == null ? null : Type.Id;
            }
        }

        public string DataOjectId { get; set; }

        public int Result { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        /// <summary>
        /// ссылка на сотрудника который в данный момент связан с задачей
        /// </summary>
        /// <remarks>
        /// это поле создано для оптимизации запросов.  специфично только дляя реляционных баз данных 
        /// </remarks>
        virtual public PartyIdentity AssignedToParty { get; set; }

        /// <summary>
        /// это поле тоже сделано для оптимизации
        /// </summary>
        public EWorkEffortStatus Status { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// конструктуор для EF
        /// </summary>
        public Task()
        {

        }

        public Task(WorkEffortType taskType)
            : base(taskType)
        {
        }
        #endregion
    }
}
