using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WoaW.TMS.Tasks.Rules;
using WoaW.NS;

namespace WoaW.TMS.Tasks
{
    public class ResourceManager 
    {
        #region attributes
        private object _rootSync = new object();
        private System.Threading.Timer _timer;
        private INotificationCenter _incidentManager;
        #endregion

        #region properties
        public ObservableCollection<BaseRule> Rules { get; private set; }
        /// <summary>
        /// TODO: подумать над :
        /// 1)инкапссуляцией этой структуры;
        /// 2)когдаотсюда убирать элементы.
        /// </summary>
        public ObservableCollection<WorkEffortPartyAssignment> Assignments { get; private set; }
        public ObservableCollection<WorkEffortPartyAssignment> History { get; set; }
        public ObservableCollection<WorkEffort> WorkEfforts { get; private set; }
        public ObservableCollection<EmployeeRole> Persons { get; private set; }
        public ObservableCollection<BaseTimeValidator> TimeValidators { get; set; }

        public int MaxHoldPersonalTasks { get; set; }
        public TimeSpan MaxTimeBeforeAcceptTask { get; set; }
        public TimeSpan MaxTimeBeforeAssignTask { get; set; }
        public TimeSpan MaxTimeInExecuteTask { get; set; }
        public TimeSpan RuleMonitotingIntervalTime { get; set; }

        public bool InManualMode { get; set; }
        #endregion

        #region constructors
        public ResourceManager(INotificationCenter incidentManager, IEnumerable<EmployeeRole> partyRoles = null)
        {
            _incidentManager = incidentManager;
            Rules = new ObservableCollection<BaseRule>();
            Assignments = new ObservableCollection<WorkEffortPartyAssignment>();
            History = new ObservableCollection<WorkEffortPartyAssignment>();
            WorkEfforts = new ObservableCollection<WorkEffort>();
            Persons = new ObservableCollection<EmployeeRole>();
            TimeValidators = new ObservableCollection<BaseTimeValidator>();
            if (partyRoles != null)
            {
                foreach (var item in partyRoles)
                {
                    Persons.Add(item);
                }
            }

            MaxHoldPersonalTasks = 3;
            MaxTimeBeforeAssignTask = TimeSpan.FromSeconds(3);
            MaxTimeBeforeAcceptTask = TimeSpan.FromSeconds(3);
            MaxTimeInExecuteTask = TimeSpan.FromSeconds(3);
            RuleMonitotingIntervalTime = TimeSpan.FromSeconds(3);

            Assignments.CollectionChanged += Assignments_CollectionChanged;
            WorkEfforts.CollectionChanged += WorkEfforts_CollectionChanged;
            Rules.CollectionChanged += Rules_CollectionChanged;
            Persons.CollectionChanged += Users_CollectionChanged;
        }
        #endregion

        #region public API
        /// <summary>
        /// это асинхронный метод - он запускает поток мониторинга
        /// </summary>
        public void StartRuleMonitoring()
        {
            if (_timer != null)
            {
                StopRuleMonitoring();
            }

            _timer = new System.Threading.Timer((x) => ValidateRulesForTimer(x), null, new TimeSpan(-1), RuleMonitotingIntervalTime);
            _timer.Change(new TimeSpan(0), RuleMonitotingIntervalTime);
        }
        public void StopRuleMonitoring()
        {
            _timer.Change(new TimeSpan(-1), RuleMonitotingIntervalTime);
            _timer.Dispose();
        }
        #endregion

        #region ITaskManager implementation

        /// <summary>
        /// метод предназначен для того чтобы руководитель назначил сотруднику задачу.
        /// возможные варианты:
        /// сотрудник не занят и задача не выполняется никем
        /// сотрудник не занят и задача выполняется другим сотрудником
        /// сотрудник занят и задача не выполняется никем 
        /// сорудник занят и задачча выполняется другим сотрудником
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="worker"></param>
        /// <param name="effort"></param>
        public WorkEffortPartyAssignment AssignTask(EmployeeRole manager, EmployeeRole worker, WorkEffort effort)
        {
            #region parameter validation
            if (manager == null)
                throw new ArgumentNullException("manager");
            if (worker == null)
                throw new ArgumentNullException("worker");
            if (effort == null)
                throw new ArgumentNullException("container");
            //если у пользователя уже есть максимально возможное количество задач помещенных в его стек, то генерируем ошибку... 
            // это можно быо бы сделать указав размер стека при его инициализации. 
            if (worker.Tasks.Count == MaxHoldPersonalTasks)
                throw new ArgumentOutOfRangeException(string.Format("worker can not have more then {0} tasks on hold", MaxHoldPersonalTasks));
            if (worker.Tasks.Count > 0 && worker.Tasks.Count(x => x.WorkEffort == effort) > 0)
                throw new ArgumentException("you can not assign a task which already on hold in users's stack");

            var alreadyAssigmentUser = Assignments.Where(c => c.WorkEffort == effort &&
                (c.Status == EWorkEffortStatus.Assigned ||
                c.Status == EWorkEffortStatus.OnHold ||
                c.Status == EWorkEffortStatus.Accepted)).Select(x => x.AssignedTo).SingleOrDefault();
            if (alreadyAssigmentUser != null)
                throw new ArgumentException(string.Format("you can not assign a task.Id={0} which already performs by another user.Id={1}", effort.Id, alreadyAssigmentUser.Id));

            //TODO: проверить руководителя является ли он руководителем и может ли он вызывать этот метод
            #endregion

            //проверяем если пользователь имеет задачу то ее нужно поставить на холд и для этого пользователя
            //добавить в стек а после окончания этой вернуть пользователю на выполнение 
            if (worker.IsBussy)
            {
                //получаем задачу котороую выполняет пользователь
                //var alreadyAssigment = (from c in Assignments where (c.AssignedTo == worker && c.Status == EWorkEffortStatus.Accepted) select c).SingleOrDefault();
                var alreadyAssigment = Assignments.SingleOrDefault(c => c.AssignedTo == worker);
                if (alreadyAssigment != null)
                {
                    alreadyAssigment.Status = EWorkEffortStatus.OnHold;
                    worker.Tasks.Push(alreadyAssigment);
                }
            }

            var assignment = new WorkEffortPartyAssignment(worker, effort);
            worker.IsBussy = true;
            Assignments.Add(assignment);

            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = manager.Id,
                WorkerId = worker.Id,
                Time = assignment.AssignedAt,
                Status = assignment.Status
            };
            assignment.History.Add(historyRecord);

            return assignment;
        }
        /// <summary>
        /// система сама назначает на пользователя задачу
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="container"></param>
        public WorkEffortPartyAssignment AssignTask(EmployeeRole worker, WorkEffort effort)
        {
            #region parameter validation
            if (worker == null)
                throw new ArgumentNullException("worker");
            if (effort == null)
                throw new ArgumentNullException("effort");
            var alreadyAssignment = Assignments.FirstOrDefault(a => a.WorkEffort == effort);
            if (alreadyAssignment != null)
                throw new ArgumentNullException(string.Format("before assign a task.Id={0} to the employee.Id={1} you should release the task from employee.Id={2}",
                    alreadyAssignment.WorkEffort.Id, worker.Id, alreadyAssignment.AssignedTo.Id));
            //TODO: проверить руководителя является ли он руководителем и может ли он вызывать этот метод
            #endregion

            WorkEfforts.Remove(effort);

            var assignment = new WorkEffortPartyAssignment(worker, effort);
            worker.IsBussy = true;
            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                WorkerId = worker.Id,
                Time = assignment.AssignedAt,
                Status = assignment.Status
            };
            assignment.History.Add(historyRecord);

            Assignments.Add(assignment);
            return assignment;
        }
        /// <summary>
        /// пользователь отклонил задачу назначенную ему
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="assigment"></param>
        public void AcceptTask(WorkEffortPartyAssignment assignment)
        {
            #region parameter validation
            if (assignment == null)
                throw new ArgumentNullException("container");
            if (assignment.AssignedTo == null)
                throw new ArgumentNullException("container.AssignedTo");
            if (assignment.WorkEffort == null)
                throw new ArgumentNullException("container.Task");

            #endregion

            assignment.Status = EWorkEffortStatus.Accepted;

            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                WorkerId = assignment.AssignedTo.Id,
                Time = assignment.AssignedAt,
                Status = assignment.Status
            };
            assignment.History.Add(historyRecord);
        }
        public void RejectTask(WorkEffortPartyAssignment assigment)
        {
            #region parameter validation
            if (assigment == null)
                throw new ArgumentNullException("container");
            if (assigment.AssignedTo == null)
                throw new ArgumentNullException("container.AssignedTo");
            if (assigment.WorkEffort == null)
                throw new ArgumentNullException("container.Task");

            #endregion

            assigment.Status = EWorkEffortStatus.Rejected;
            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                WorkerId = assigment.AssignedTo.Id,
                Time = assigment.AssignedAt,
                Status = assigment.Status
            };
            assigment.History.Add(historyRecord);

        }
        /// <summary>
        /// менеджер снимает задачу с пользователя
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="worker"></param>
        /// <param name="assignment"></param>
        public void RemoveTask(EmployeeRole manager, WorkEffortPartyAssignment assignment)
        {
            #region parameter validation
            if (manager == null)
                throw new ArgumentNullException("manager");
            if (assignment == null)
                throw new ArgumentNullException("container");
            if (assignment.AssignedTo == null)
                throw new ArgumentNullException("container.AssignedTo");
            if (assignment.WorkEffort == null)
                throw new ArgumentNullException("container.Task");

            #endregion

            assignment.Status = EWorkEffortStatus.Closed;
            assignment.AssignedTo.IsBussy = false;

            //Assignments.Remove(assignment);

            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = manager.Id,
                Time = assignment.AssignedAt,
                Status = assignment.Status
            };
            assignment.History.Add(historyRecord);

        }
        public void CloseTask(WorkEffortPartyAssignment assignment)
        {
            #region parameter validation
            if (assignment == null)
                throw new ArgumentNullException("container");
            if (assignment.AssignedTo == null)
                throw new ArgumentNullException("container.AssignedTo");
            if (assignment.WorkEffort == null)
                throw new ArgumentNullException("container.Task");

            #endregion

            assignment.Status = EWorkEffortStatus.Closed;
            assignment.AssignedTo.IsBussy = false;

            //Assignments.Remove(assignment);

            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                WorkerId = assignment.AssignedTo.Id,
                Time = assignment.AssignedAt,
                Status = assignment.Status
            };
            assignment.History.Add(historyRecord);
        }
        /// <summary>
        /// пользователь самостоятельно взял на себя задачу
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WorkEffortPartyAssignment AcceptTaskManually(EmployeeRole worker, WorkEffort effort)
        {
            #region parameter validation
            if (worker == null)
                throw new ArgumentNullException("worker");
            if (effort == null)
                throw new ArgumentNullException("effort");
            if (worker.IsBussy == true)
                throw new ArgumentException("worker.IsBussy == true");
            if (effort.AllowManualAcceptance == false)
                throw new ArgumentException("effort.AllowManualAcceptance == false");
            var alreadyAssignment = Assignments.SingleOrDefault(r => r.AssignedTo != null || r.WorkEffort == effort);
            if (alreadyAssignment != null)
                throw new ArgumentException("r.AssignedTo != null || r.WorkEffort ==effort");
            #endregion

            var assignment = new WorkEffortPartyAssignment(worker, effort);
            assignment.Status = EWorkEffortStatus.Accepted;
            WorkEfforts.Remove(effort);
            Assignments.Add(assignment);

            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                WorkerId = worker.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.History.Add(historyRecord);

            return assignment;
        }
        public void AddCommentToTask(WorkEffort container, string comment)
        {
            #region parameter validation
            if (container == null)
                throw new ArgumentNullException("container");
            if (container.Container == null)
                throw new ArgumentNullException("container.Task");
            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentNullException("comment");
            #endregion

            container.Comments.Add(comment);
        }
        #endregion

        #region implementation
        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (InManualMode == true)
                return;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ValidateRulesForTimer(e.NewItems);
            }
        }
        private void Rules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (InManualMode == true)
                return;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ValidateRulesForTimer(e.NewItems);
            }
        }
        private void WorkEfforts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (InManualMode == true)
                return;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ValidateRulesForEvent(e.NewItems);

                //var list = new WorkEffort[e.NewItems.Count];
                //e.NewItems.CopyTo(list, 0);
                //var validator = new WaitBeforeAccept(_incidentManager);
                //validator.Setup(this, list);
            }
        }
        private void Assignments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (InManualMode == true)
                return;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                //var list = new WorkEffortPartyAssignment[e.NewItems.Count];
                //e.NewItems.CopyTo(list, 0);

                //var validator = new WaitBeforeAccept(_incidentManager);
                //validator.Setup(this, list);
            }
        }

        /// <summary>
        /// метод сделан публичным только с целью тестирования
        /// </summary>
        /// <param name="param"></param>
        public virtual void ValidateRulesForTimer(object param)
        {
            lock (_rootSync)
            {
                //foreach (var item in WorkEfforts)
                //{
                //    var effort = item as WorkEffort;
                //    //rec.Status = EWorkEffortStatus.Created;

                //    foreach (var rule in Rules)
                //    {
                //        var assignment = rule.Execute(this, effort);
                //        if (assignment != null)
                //        {
                //            AssignTaskAtUser(user, effort);
                //        }
                //    }
                //}
            }
        }
        public virtual void ValidateRulesForEvent(System.Collections.IList list)
        {
            lock (_rootSync)
            {
                foreach (var item in list)
                {
                    var effort = item as WorkEffort;

                    foreach (var rule in Rules)
                    {
                        rule.Execute(this, effort);
                    }
                }
            }
        }
        #endregion

    }
}
