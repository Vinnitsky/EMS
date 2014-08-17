using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using EzBpm.Tms.ConfigModel;
using Microsoft.AspNet.Identity;
using WoaW.CMS.DAL.EF;
using WoaW.CMS.Model;
using WoaW.CMS.Model.Repationships;
using WoaW.NS;
using WoaW.Tms.DAL.EF;
using WoaW.TMS.Model;
using WoaW.TMS.Model.DAL;

namespace WoaW.TMS.DAL.EF.Facade
{
    public class TaskManagerFacade : ITaskManagerFacade
    {
        #region attributes
        private DbContext _dbContext;
        private ConfigModel _model;
        private ActionAvailabilityValidator _validator;
        #endregion

        #region constructors
        public TaskManagerFacade()
        {

        }
        public TaskManagerFacade(DbContext dbContext, ConfigModel model, INotificationCenter notificationCenter)
            : this()
        {
            #region paramenetr validation
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            if (dbContext == null)
                throw new ArgumentNullException("notificationCenter");
            if (dbContext == null)
                throw new ArgumentNullException("securityFacade");
            #endregion

            _dbContext = dbContext;
            _model = model;

            _validator = new ActionAvailabilityValidator(dbContext, model);
        }
        #endregion

        public ITask Create(string taskTypeId)
        {
            var currentUser = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanCreate(taskTypeId) == false)
                throw new AccessViolationException(string.Format("user with name={0} is not a managers for task type id={1}", currentUser, taskTypeId));

            //получаем экзепляр типа задачи
            var taskType = _dbContext.Set<WorkEffortType>().SingleOrDefault(t => t.Id == taskTypeId);

            //создаем экземпляр задачи
            var effort = new Task(taskType);

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояниеc
            effort.Status = EWorkEffortStatus.Wait;

            //делаем запись в журнале
            //var historyRecord = new WorkEffortHistorycalRecord
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    ManagerId = currentUser,
            //    TaskId = effort.Id,
            //    Time = DateTime.Now,
            //    Status = assignment.Status
            //};
            //assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.Set<WorkEffort>().Add(effort);
            _dbContext.SaveChanges();

            return effort;
        }

        public ITask Assign(string employeeId, string taskId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            //убеждаемся что currentUserId имеет право назначать и задача в состоянии Wait и employeeId имеет право исполнять
            if (_validator.CanAssign(employeeId, taskId) == false)
                throw new AccessViolationException(string.Format("user with name={0} can not assign task id={1}", currentUserName, taskId));

            //получаю задачу
            var effort = _dbContext.Set<Task>().SingleOrDefault(a => a.Id == taskId);
            if (effort == null)
                throw new ArgumentException(string.Format("can not find task by id={0}", taskId));

            //получаю сотрудника
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            var employee = userManager.FindById(employeeId);
            if (employee == null)
                throw new ArgumentException(string.Format("can not find user by id={0}", employeeId));

            //убеждаемся, что задачу никто не выполняет 
            if (effort.AssignedToParty != null)
                throw new ArgumentNullException(string.Format("you can't assign this task = {0} for user={1} because task already performs by other employee ",
                    effort.Id, effort.AssignedToParty.UserName));

            //убеждаемся, что сотрудник свободный
            var party = _dbContext.Set<Party>().SingleOrDefault(t => t.Id == employee.Id);
            if (party != null && party.IsBusy)
                throw new ArgumentException(string.Format("User is bussy"));

            //TODO: назначать нужную роль
            var employeeRole = new EmployeeRole(new RoleType("Employee"), party);
            var assignment = new WorkEffortPartyAssignment(employeeRole, effort)
            {
                AssignedAt = DateTime.Now,
                Status = EWorkEffortStatus.Assigned
            };
            effort.AssignedToParty = employee;

            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = currentUserName,
                EmployeeId = employeeId,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);

            _dbContext.Set<WorkEffortPartyAssignment>().Add(assignment);
            _dbContext.SaveChanges();

            return assignment.WorkEffort as Task;
        }

        public ITask Accept(string taskId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanAccept(taskId) == false)
                throw new ApplicationException(string.Format("user id={0} can't accept task id={1}", currentUserName, taskId));

            //убеждаемся, что задача существует в базе
            var assignment = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);
            if (assignment == null)
                throw new Exception("assignment");

            //устанавливаем время 
            assignment.AcceptedAt = DateTime.Now;

            //устанавливаем состояние
            assignment.Status = EWorkEffortStatus.Accepted;

            //делаем запись в дурнале
            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = currentUserName,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.SaveChanges();
            return assignment.WorkEffort as Task;
        }

        public ITask Reject(string taskId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanReject(taskId) == false)
                throw new ApplicationException(string.Format("user name={0} can't reject task id={1}", currentUserName, taskId));

            //убеждаемся, что задача существует в базе
            var assignment = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);
            if (assignment == null)
                throw new Exception("assignment");

            //устанавливаем время 
            assignment.RejectedAt = DateTime.Now;

            //устанавливаем состояние
            assignment.Status = EWorkEffortStatus.Rejected;

            //делаем запись в дурнале
            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = currentUserName,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.SaveChanges();
            return assignment.WorkEffort as Task;
        }

        public ITask GetToWork(string taskId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanGetToWork(taskId) == false)
                throw new ApplicationException(string.Format("user name={0} can't perform task id={1}", currentUserName, taskId));

            //убеждаемся, что задача существует в базе
            var assignment = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);
            if (assignment == null)
                throw new Exception("assignment");

            //устанавливаем время 
            assignment.WorkEffort.ActualStartTime = DateTime.Now;

            //устанавливаем состояние
            assignment.Status = EWorkEffortStatus.InWork;

            //устанавливаем флаг о том, что пользователь занят
            assignment.AssignedTo.Party.IsBusy = true;

            //делаем запись в дурнале
            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = currentUserName,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.SaveChanges();
            return assignment.WorkEffort as Task;
        }

        public ITask Pause(string taskId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanPause(taskId) == false)
                throw new ApplicationException(string.Format("user name={0} can't reject task id={1}", currentUserName, taskId));

            //убеждаемся, что задача существует в базе
            var assignment = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);
            if (assignment == null)
                throw new Exception("assignment");

            //устанавливаем время 
            //assignment.WorkEffort.ActualCompletionTime = DateTime.Now;

            //устанавливаем состояние
            assignment.Status = EWorkEffortStatus.Paused;

            //делаем запись в дурнале
            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = currentUserName,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.SaveChanges();
            return assignment.WorkEffort as Task;
        }

        public ITask Resume(string taskId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanResume(taskId) == false)
                throw new ApplicationException(string.Format("user name={0} can't reject task id={1}", currentUserName, taskId));

            //убеждаемся, что задача существует в базе
            var assignment = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);
            if (assignment == null)
                throw new Exception("assignment");

            //устанавливаем время 
            assignment.WorkEffort.ActualStartTime = DateTime.Now;

            //устанавливаем состояние
            assignment.Status = EWorkEffortStatus.InWork;

            //делаем запись в дурнале
            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = currentUserName,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.SaveChanges();
            return assignment.WorkEffort as Task;
        }

        public ITask Close(string taskId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanClose(taskId) == false)
                throw new ApplicationException(string.Format("user name={0} can't reject task id={1}", currentUserName, taskId));

            //убеждаемся, что задача существует в базе
            var assignment = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);
            if (assignment == null)
                throw new Exception("assignment");

            //устанавливаем время 
            assignment.WorkEffort.ActualFinishTime = DateTime.Now;

            //устанавливаем состояние
            assignment.Status = EWorkEffortStatus.Closed;

            assignment.AssignedTo.Party.IsBusy = false;
            //assignment.AssignedTo = null;

            //делаем запись в дурнале
            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = currentUserName,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.SaveChanges();
            return assignment.WorkEffort as Task;
        }

        public IEnumerable<TaskModel> WhichTasksUserCanCreate()
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            var user = userManager.FindByName(currentUserName);
            return (from m in _model.Tasks where userManager.IsInRole(user.Id, m.Manager) select m).ToList();
            //var currentPrincipal = System.Threading.Thread.CurrentPrincipal;
            //return (from m in _model.Tasks where currentPrincipal.IsInRole(m.Manager) select m).ToList();
        }

        public ITask ChangeManager(string taskId, string managerId)
        {
            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            if (_validator.CanChangeManager(taskId) == false)
                throw new ApplicationException(string.Format("user name={0} can't change manager for task id={1}", currentUserName, taskId));

            // убеждаемся, что задача существует в базе 
            var assignment = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);
            if (assignment == null)
                throw new ArgumentException(string.Format("can not fined task by id={0}", taskId));

            //получаем идентификатор типа задачи 
            var taskTypeId = assignment.WorkEffort.Type.Id;

            //получаем конфигурационную информацию о типе задачи
            TaskModel taskModel = _validator.GetTaskModel(taskTypeId);
            if (taskModel == null)
                throw new ObjectNotFoundException();

            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));

            //убеждаемся что ManagerId не является членом группы taskModel.Superviser
            if (userManager.IsInRole(managerId, taskModel.Superviser) == false)
            {
                //добавляем managerId в группу Supervisor
                userManager.AddToRole(managerId, taskModel.Superviser);
            }

            //делаем запись в журнал
            var historyRecord = new WorkEffortHistorycalRecord
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = currentUserName,
                EmployeeId = managerId,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status,
                OperationComment = "manager was changed"
            };
            assignment.AddHistoryRecord(historyRecord);

            //сохраняем в базе
            _dbContext.SaveChanges();
            return assignment.WorkEffort as Task;
        }

        public QueryTaskResult Query(QueryTaskRequest request)
        {
            #region parameter validation
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.TaskTypeIDs == null)
                request.TaskTypeIDs = new List<string>();

            #endregion
            const string @ascending = "asc";
            if (string.IsNullOrWhiteSpace(request.SortDirection))
                request.SortDirection = ascending;

            var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            //получаем список всех типов задач, где пользователь является членом одной из грпп: Manager, Employee, Supervisor 
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            var currentUser = userManager.FindByName(currentUserName);
            var userId = currentUser.Id;

            var taskModels = from m in _model.Tasks
                             where (userManager.IsInRole(userId, m.Employee) || userManager.IsInRole(userId, m.Manager) || userManager.IsInRole(userId, m.Superviser))
                             && request.TaskTypeIDs.Contains(m.Id)
                             select m;

            if (request.TaskTypeIDs.Count > 0)
            {
                taskModels = taskModels.Where(x => request.TaskTypeIDs.Contains(x.Id));
            }

            //получаем список задач с типом request.TaskTypeIDs
            var allowViewTaskTypeId = taskModels.Select(x => x.Id).ToList();

            var tasksWithRequiredType = _dbContext.Set<WorkEffortPartyAssignment>().Where(t => allowViewTaskTypeId.Any(c => c == t.WorkEffort.Type.Id));

            var filter = new List<Filter>();

            #region ActualStartTime
            if (request.ActualStartTime.From.HasValue && request.ActualStartTime.Till.HasValue == false)
            {
                filter.Add(new Filter { PropertyName = "ActualStartTime", Operation = Op.LessThan, Value = request.ActualStartTime.From, Type = typeof(DateTime?) });
            }
            if (request.ActualStartTime.From.HasValue && request.ActualStartTime.Till.HasValue)
            {
                filter.Add(new Filter { PropertyName = "ActualStartTime", Operation = Op.LessThan, Value = request.ActualStartTime.From, Type = typeof(DateTime?) });
                filter.Add(new Filter { PropertyName = "ActualStartTime", Operation = Op.GreaterThan, Value = request.ActualStartTime.Till, Type = typeof(DateTime?) });
            }
            if (request.ActualStartTime.From.HasValue == false && request.ActualStartTime.Till.HasValue)
            {
                filter.Add(new Filter { PropertyName = "ActualStartTime", Operation = Op.GreaterThan, Value = request.ActualStartTime.From, Type = typeof(DateTime?) });
            }
            #endregion

            #region CreatedOn
            if (request.CreatedOn.From.HasValue && request.CreatedOn.Till.HasValue == false)
            {
                filter.Add(new Filter { PropertyName = "CreatedAt", Operation = Op.LessThan, Value = request.CreatedOn.From, Type = typeof(DateTime?) });
            }
            if (request.CreatedOn.From.HasValue && request.CreatedOn.Till.HasValue)
            {
                filter.Add(new Filter { PropertyName = "CreatedAt", Operation = Op.LessThan, Value = request.CreatedOn.From, Type = typeof(DateTime?) });
                filter.Add(new Filter { PropertyName = "CreatedAt", Operation = Op.GreaterThan, Value = request.CreatedOn.Till, Type = typeof(DateTime?) });
            }
            if (request.CreatedOn.From.HasValue == false && request.CreatedOn.Till.HasValue)
            {
                filter.Add(new Filter { PropertyName = "CreatedAt", Operation = Op.GreaterThan, Value = request.CreatedOn.From, Type = typeof(DateTime?) });
            }
            #endregion

            #region ScheduledStartTime
            if (request.ScheduledStartTime.From.HasValue && request.ScheduledStartTime.Till.HasValue == false)
            {
                filter.Add(new Filter { PropertyName = "ScheduledStartTime", Operation = Op.LessThan, Value = request.CreatedOn.From, Type = typeof(DateTime?) });
            }
            if (request.ScheduledStartTime.From.HasValue && request.ScheduledStartTime.Till.HasValue)
            {
                filter.Add(new Filter { PropertyName = "ScheduledStartTime", Operation = Op.LessThan, Value = request.ScheduledStartTime.From, Type = typeof(DateTime?) });
                filter.Add(new Filter { PropertyName = "ScheduledStartTime", Operation = Op.GreaterThan, Value = request.ScheduledStartTime.Till, Type = typeof(DateTime?) });
            }
            if (request.CreatedOn.From.HasValue == false && request.CreatedOn.Till.HasValue)
            {
                filter.Add(new Filter { PropertyName = "ScheduledStartTime", Operation = Op.GreaterThan, Value = request.ScheduledStartTime.From, Type = typeof(DateTime?) });
            }
            #endregion

            #region Status - not finished
            //if (request.Statuses != null)
            //{
            //    foreach (var item in request.Statuses)
            //    {
            //        filter.Add(new Filter { PropertyName = "Status", Operation = Op.Contains, Value = item, Type = typeof(EWorkEffortStatus) });
            //    }
            //}

            #endregion

            //#region Subject
            //if (string.IsNullOrWhiteSpace(request.Subject) == false)
            //{
            //    filter.Add(new Filter { PropertyName = "Subject", Operation = Op.StartsWith, Value = item, Type = typeof(string) });
            //}
            //#endregion

            var filteredCollection = filter.Count == 0 ? _dbContext.Set<Task>()
                : _dbContext.Set<Task>().Where(ExpressionBuilder.GetExpression<Task>(filter));

            var result = new QueryTaskResult
            {
                TotalCount = filteredCollection.Count(),
                Request = request,
            };

            // TODO: 
            if (string.IsNullOrWhiteSpace(request.SortField))
                request.SortField = "Id";


            var pagedFilteredCollection = filteredCollection.OrderBy(t=>t.Id).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ToList();
            result.Items = pagedFilteredCollection.Cast<ITask>().ToArray();

            return result;
        }

        public ITask GetTaskById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is not provided", "id");

            var effort = _dbContext.Set<Task>().SingleOrDefault(x => x.Id == id);
            return effort;
        }

        public ITask Cancel()
        {
            throw new NotImplementedException();
        }

        #region implementation
        #endregion
    }
}
