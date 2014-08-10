using System;
using System.Collections.Generic;

namespace WoaW.TMS.Tasks
{
    /// <summary>
    /// этот класс связывает задачу с сотрудником, учитывая роль сотрудника
    /// </summary>
    public class WorkEffortPartyAssignment
    {
        #region attributes
        private DateTime? _createdAt;
        private DateTime? _assignedAt;
        private DateTime? _acceptedAt;
        private EmployeeRole _partyRole;
        private WorkEffort _workEffort;
        private EWorkEffortStatus _status = EWorkEffortStatus.Created;
        private List<WorkEffortHistorycalRecord> _history;
        #endregion

        #region properties
        public string Id { get; set; }
        /// <summary>
        /// задача считается созданой тогда когда ее добавили в список задач в 
        /// TaskManager. до этого момента это просто объект в начальном состоянии
        /// </summary>
        public DateTime? CreatedAt { get { return _createdAt; } }
        /// <summary>
        /// время когда задача была назначена сотруднику, 
        /// но он ее еще и не принял и не отклонил
        /// </summary>
        public DateTime? AssignedAt { get { return _assignedAt; } }
        /// <summary>
        /// время когда сотрудник принял назначенную задачу, 
        /// но но еще не начал ее выполнять
        /// </summary>
        public DateTime? AcceptedAt { get { return _acceptedAt; } }
        /// <summary>
        /// время запланированое менеджером на выполнение задачи 
        /// </summary>
        public List<WorkEffortHistorycalRecord> History { get { return _history; } }
        /// <summary>
        /// может работать с непосредственно таким же свойством задачи
        /// </summary>
        public EmployeeRole AssignedTo
        {
            get { return _partyRole; }
            set
            {

                _partyRole = value;
                if (_partyRole == null)
                {
                    _assignedAt = null;
                    _acceptedAt = null;
                }
                else
                {
                    _status = EWorkEffortStatus.Assigned;
                    _partyRole.IsBussy = true;
                    _assignedAt = DateTime.Now;
                }
            }
        }
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
                            _createdAt = DateTime.Now;
                            //TODO: запускаем таймер на ожидание времени
                        }
                        break;
                    case EWorkEffortStatus.Assigned:
                        {
                            _assignedAt = DateTime.Now;
                            //TODO: запускаем таймер на ожидание времени
                        }
                        break;
                    case EWorkEffortStatus.Accepted:
                        {
                            //TODO: запускаем таймер на ожидание
                            _acceptedAt = DateTime.Now;
                            //в данной реализации считаем что время принятия задачи является актуальным временем начала задачи,. т.е. как принял - значит начал выполнять
                            WorkEffort.ActualStartDatetime = _acceptedAt;
                        }
                        break;
                    case EWorkEffortStatus.Rejected:
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.Canceled:
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.OnHold:
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.Paused:
                        //TODO: ???
                        break;
                    case EWorkEffortStatus.Closed:
                        //TODO: выключаем таймер на ожидание
                        WorkEffort.ActualCompletionDatetime = DateTime.Now;
                        WorkEffort.ActualTime = WorkEffort.ActualStartDatetime - WorkEffort.ActualCompletionDatetime;
                        break;
                    default:
                        break;
                }
            }
        }

        public WorkEffort WorkEffort
        {
            get { return _workEffort; }
            set { _workEffort = value; }
        }
        #endregion

        #region constructors
        public WorkEffortPartyAssignment()
        {
            Id = Guid.NewGuid().ToString();
            _history = new List<WorkEffortHistorycalRecord>();
            Status = EWorkEffortStatus.Created;

        }
        public WorkEffortPartyAssignment(EmployeeRole partyRole, WorkEffort effort)
            : this()
        {
            _partyRole = partyRole;
            _workEffort = effort;
            Status = EWorkEffortStatus.Assigned;
        }
        #endregion
    }
}
