using System;
using System.Collections.Generic;

namespace WoaW.TMS.Model
{
    /// <summary>
    /// этот класс связывает задачу с сотрудником, учитывая роль сотрудника
    /// </summary>
    public class WorkEffortPartyAssignment
    {
        #region attributes
        private EWorkEffortStatus _status = EWorkEffortStatus.Wait;
        #endregion

        #region properties
        public string Id { get; set; }

        /// <summary>
        /// задача считается созданой тогда когда ее добавили в список задач в 
        /// TaskManager. до этого момента это просто объект в начальном состоянии
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// время когда задача была назначена сотруднику, 
        /// но он ее еще и не принял и не отклонил
        /// </summary>
        public DateTime? AssignedAt { get; set; }
        /// <summary>
        /// время когда сотрудник принял назначенную задачу, 
        /// но но еще не начал ее выполнять
        /// </summary>
        public DateTime? AcceptedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public DateTime? CanceleddAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        /// <summary>
        /// может работать с непосредственно таким же свойством задачи
        /// </summary>
        public EWorkEffortStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                switch (_status)
                {
                    case EWorkEffortStatus.Created:
                        {
                            CreatedAt = DateTime.Now;
                            //TODO: запускаем таймер на ожидание времени
                        }
                        break;
                    case EWorkEffortStatus.Assigned:
                        {
                            AssignedAt = DateTime.Now;
                            //TODO: запускаем таймер на ожидание времени
                        }
                        break;
                    case EWorkEffortStatus.Accepted:
                        {
                            //TODO: запускаем таймер на ожидание
                            AcceptedAt = DateTime.Now;
                            //в данной реализации считаем что время принятия задачи является актуальным временем начала задачи,. т.е. как принял - значит начал выполнять
                            WorkEffort.ActualStartTime = AcceptedAt;
                        }
                        break;
                    case EWorkEffortStatus.Rejected:
                        RejectedAt = DateTime.Now;
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.Canceled:
                        CanceleddAt = DateTime.Now;
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.OnHold:
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.Paused:
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.Closed:
                        ClosedAt = DateTime.Now;
                        //TODO: выключаем таймер на ожидание
                        WorkEffort.ActualFinishTime = DateTime.Now;
                        WorkEffort.ActualTime = WorkEffort.ActualStartTime - WorkEffort.ActualFinishTime;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// может работать с непосредственно таким же свойством задачи
        /// </summary>
        virtual public EmployeeRole AssignedTo { get; set; }
        virtual public WorkEffort WorkEffort { get; set; }
        /// <summary>
        /// время запланированое менеджером на выполнение задачи 
        /// </summary>
        virtual public List<WorkEffortHistorycalRecord> History { get; private set; }
        #endregion

        #region constructors
        public WorkEffortPartyAssignment()
        {
            Id = Guid.NewGuid().ToString();
            History = new List<WorkEffortHistorycalRecord>();
            Status = EWorkEffortStatus.Created;

        }
        public WorkEffortPartyAssignment(EmployeeRole partyRole, WorkEffort effort)
            : this()
        {
            AssignedTo = partyRole;
            WorkEffort = effort;
            Status = EWorkEffortStatus.Assigned;
        }
        #endregion

        public void AddHistoryRecord(WorkEffortHistorycalRecord historyRecord)
        {
            History.Add(historyRecord);
        }

    }
}
