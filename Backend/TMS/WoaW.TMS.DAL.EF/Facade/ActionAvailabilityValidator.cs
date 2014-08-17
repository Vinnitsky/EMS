using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using EzBpm.Tms.ConfigModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WoaW.CMS.DAL.EF;
using WoaW.Tms.DAL.EF;
using WoaW.TMS.Model;
using WoaW.TMS.Model.DAL;

namespace WoaW.TMS.DAL.EF.Facade
{
    public sealed class ActionAvailabilityValidator : IActionAvailabilityValidator
    {
        #region attributes
        private readonly DbContext _dbContext;
        private readonly ConfigModel _model;
        #endregion

        public ActionAvailabilityValidator(DbContext dbContext, ConfigModel model)
        {
            _dbContext = dbContext;
            _model = model;
        }

        #region IActionAvailabilityValidator
        /// <summary>
        /// создает задачу.
        /// </summary>
        /// <remarks>
        /// возможность запускать метод кому попало это не правильно - получается я будучи уборшиком узнав 
        /// идентификатор менеджера - могу запустить процесс от его имени... и потом пусть менеджер жокажет 
        /// что это не он запускал ... 
        /// </remarks>
        /// <param name="currentUserId">менеджер который создает задачу</param>
        /// <param name="taskTypeId">идентификатор типа задачи</param>
        /// <returns></returns>
        public bool CanCreate(string taskTypeId)
        {
            #region parameter validation
            if (string.IsNullOrWhiteSpace(taskTypeId))
                throw new ArgumentNullException("taskTypeId");
            #endregion

            //убеждается что тип задачи существуует в базе
            var taskType = _dbContext.Set<WorkEffortType>().SingleOrDefault(t => t.Id == taskTypeId);
            if (taskType == null)
                throw new ArgumentException(string.Format("can not fined WorkEffortType by id={0}", taskTypeId));

            //убеждаемся что в модели есть запись для такого типа задачи
            TaskModel taskModel = GetTaskModel(taskTypeId);
            if (taskModel == null)
                throw new ObjectNotFoundException();

            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            var currentUser = userManager.FindByName(System.Threading.Thread.CurrentPrincipal.Identity.Name);

            //убеждаемся что сотрудник является членом группы менеджер
            var isManager = userManager.IsInRole(currentUser.Id, taskModel.Manager);
            if (isManager == false)
                return false;

            return true;
        }
        /// <summary>
        /// задача находиться в состоянии Wait и текущий сотрудник входит в роль Manager‏
        /// </summary>
        /// <remarks>
        /// по сути этот метод проверяет: - имеет ли сотрудник с идентификатором (currentUserId) 
        /// право нзаначать задачи кому бы то нибыло - такого типа, как тип задачи taskTypeId.
        /// 
        /// Note: при этом нужно понимать, что право наначать проверяется не для конкретного экземпляра задачи 
        /// а на весь тип задач. 
        /// Note: остается не понмятным зачаем возможность пользователя назначать каго либо на данный типа задач 
        /// объеденил с проверкой состояния.  так как наличие у задачи состояния Wait воввсе на значит возможность быть назначеной
        /// 
        /// при запуске метода он должен проверять что:
        /// - currentUserId - не пустой
        /// - currentUserId входит в роль менеджера 
        /// - taskTypeId не пустой 
        /// - задача с таким taskTypeId существует в базе
        /// true - свободен, false - занят 
        /// </remarks>
        /// <param name="currentUserId">пользователь право которого проверяется</param>
        /// <param name="taskId"> задача для типа которой проверяется разрешение</param>
        /// <returns> наличие права назначать на данный тип задач струдников. 
        /// true - можно, false - нельзя </returns>
        public bool AllowAssign(string taskId)
        {
            if (HasPermissions(taskId, t => t.Manager) == false)
                return false;

            //убеждаемся что задача в состоянии ожидения
            var assigmnent = _dbContext.Set<Task>().SingleOrDefault(a => a.Id == taskId);
            if (assigmnent.Status != EWorkEffortStatus.Wait)
                return false;

            return true;
        }
        /// <summary>
        /// вернуть всех пользователей в системе которые входят в роль Employee для задачи
        /// </summary>
        /// <remarks>
        /// странно, зачем при таких условиях первый параметр currentUserId
        /// </remarks>
        /// <param name="currentUserId"> без понятия здачем этот парамтер - в текущей реализации игнорируется</param>
        /// <param name="taskTypeId">Идентификатор типа задачи</param>
        /// <returns>список пользователей</returns>
        public IList<PartyIdentity> GetEmployees(string taskTypeId)
        {
            #region parameter validation
            if (string.IsNullOrWhiteSpace(taskTypeId))
                throw new ArgumentNullException("taskTypeId");
            #endregion

            //получаем конфигурационную информацию о типе задачи
            TaskModel taskModel = GetTaskModel(taskTypeId);
            if (taskModel == null)
                throw new ObjectNotFoundException();

            //получаем всех членов группы Employee
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            var roleManager = new RoleManager<IdentityRole, string>(new RoleStore<IdentityRole>((DbContext)_dbContext));
            var users = userManager.GetUsersForRoleName(roleManager, taskModel.Employee);

            return users;
        }
        /// <summary>
        /// проверять что задача находиться в состоянии Wait и текущий сотрудник входит в роль Manager, а toEmployeeId входит в роль Employee‏
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="employeeId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool CanAssign(string employeeId, string taskId)
        {
            #region validation parameters
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentNullException("employeeId");
            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentNullException("taskId");
            #endregion

            //убеждаемся, что currentUserId может назначать задачу данного типа и что здача в состоянии Wait
            var isAllowAssign = AllowAssign(taskId);
            if (isAllowAssign != true)
                return false;

            ////убеждаемся что сотрудник есть в базе, что сотрудник свободен, и состояние задачи 'назначена'
            //if (DoesCircumstancesAllow( taskId, true, t => t == EWorkEffortStatus.Wait, employee.UserName) == false)
            //    return false;

            ////получаем конфигурационную информацию о типе задачи
            var effort = _dbContext.Set<WorkEffort>().SingleOrDefault(a => a.Id == taskId);
            if (effort == null)
                throw new ArgumentException(string.Format("can not fined task by id={0}", taskId));

            var taskTypeId = effort.Type.Id;
            TaskModel taskModel = GetTaskModel(taskTypeId);
            if (taskModel == null)
                throw new ObjectNotFoundException();

            //убеждаемся, что сотрудник существует в базе 
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            var employee = userManager.FindById(employeeId);
            if (employee == null)
                throw new ArgumentException(string.Format("can not find user by id={0}", employeeId));

            //убеждаемся, что сотрудник является членом группы "сотрудник" для этого типа задач. 
            var isEmployee = userManager.IsInRole(employee.Id, taskModel.Employee);
            if (isEmployee == false)
                return false;

            if(employee.Party.IsBusy == true)
                return false;

            return true;
        }
        public bool CanAccept(string taskId)
        {
            if (HasPermissions(taskId, t => t.Employee) == false)
                return false;

            //убеждаемся что сотрудник есть в базе, что сотрудник свободен, и состояние задачи 'назначена'
            if (DoesCircumstancesAllow(taskId, true, t => t == EWorkEffortStatus.Assigned) == false)
                return false;

            return true;
        }
        public bool CanReject(string taskId)
        {
            return CanAccept(taskId);
        }
        public bool CanGetToWork(string taskId)
        {
            if (HasPermissions(taskId, t => t.Employee) == false)
                return false;

            //убеждаемся что сотрудник есть в базе, что сотрудник свободен, и состояние задачи 'принята'
            if (DoesCircumstancesAllow(taskId, true, t => t == EWorkEffortStatus.Accepted) == false)
                return false;

            return true;
        }
        public bool CanPause(string taskId)
        {
            if (HasPermissions(taskId, t => t.Employee) == false)
                return false;

            //убеждаемся что сотрудник занят, и состояние задачи 'исполняется'
            if (DoesCircumstancesAllow(taskId, false, t => t == EWorkEffortStatus.InWork) == false)
                return false;

            return true;
        }
        public bool CanResume(string taskId)
        {
            if (HasPermissions(taskId, t => t.Employee) == false)
                return false;

            //убеждаемся что сотрудник занят, и состояние задачи 'на паузе'
            if (DoesCircumstancesAllow(taskId, false, t => t == EWorkEffortStatus.Paused) == false)
                return false;

            return true;
        }
        public bool CanClose(string taskId)
        {
            if (HasPermissions(taskId, t => t.Employee) == false)
                return false;

            //закрыть можно задачу если 
            // - принял - нельзя - cancel
            // - работаешь 
            // - назначена - нельзя - cancel
            // - в паузе - нельзя - cancel
            //убеждаемся что сотрудник есть в базе, что сотрудник занят, и состояние задачи 'принята'
            if (DoesCircumstancesAllow(taskId, false, t => t == EWorkEffortStatus.InWork) == false)
                return false;

            return true;
        }
        public bool CanChangeManager(string taskId)
        {
            if (HasPermissions(taskId, t => t.Superviser) == false)
                return false;

            return true;
        }
        public bool CanEdit(string taskId)
        {
            if (HasPermissions(taskId, t => t.Manager) == false)
                return false;

            return true;
        }
        public bool CanCancel(string taskId)
        {
            if (HasPermissions(taskId, t => t.Manager) == false)
                return false;

            return true;
        }
        public bool CanChangeDueDateTime(string taskId)
        {
            if (HasPermissions(taskId, t => t.Manager) == false)
                return false;

            return true;
        }
        public bool CanChangeScheduleDateTime(string taskId)
        {
            if (HasPermissions(taskId, t => t.Manager) == false)
                return false;

            return true;
        }
        public bool CanChangePriority(string taskId)
        {
            if (HasPermissions(taskId, t => t.Manager) == false)
                return false;

            return true;
        }

        #endregion

        #region implementation
        /// <summary>
        /// метод проверяет что пользователь является членом указанной грппы для переданного идентификатора задач
        /// </summary>
        /// <remarks>
        /// метод получает индентификатор задачи как параметр. по этому идентификатору задачи метод получает конфигурационную 
        /// информацию о типа задачи. получив информацию о типе задачи метод проверяет нахродиться ли текущий пользователь 
        /// в указанной в параметре roleProperty - роли. 
        /// 
        /// проверки:
        /// - идентификатор задачи не пустой
        /// - ссылка на свойство не пустое
        /// - сотрудник существует в системе 
        /// - задача существует в базе
        /// - что сотрудник является членом группы 
        /// </remarks>
        /// <param name="taskId">идентификатор задчи для которой проверяются права пользователя</param>
        /// <param name="roleProperty"> ссылка на свойство которое содержит название группы пользователей</param>
        /// <returns> наличие или отсвутсвие прав 
        /// true - у текущего пользователя системы есть право выполнить дейсвтие 
        /// false - в противном случае
        /// </returns>
        public bool HasPermissions(string taskId, Func<TaskModel, string> roleProperty)
        {
            #region validation parameters
            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentNullException("taskId");
            if (roleProperty == null)
                throw new ArgumentNullException("roleProperty");
            #endregion

            //убеждаемся, что сотрудник существует в базе 
            //var currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            //var userManager = new UserManager(_dbContext);
            //var employee = userManager.FindUserByName(currentUserName);
            //if (employee == null)
            //    throw new ArgumentException(string.Format("can not find user by id={0}", currentUserName));

            // убеждаемся, что задача существует в базе 
            var effort = _dbContext.Set<WorkEffort>().SingleOrDefault(a => a.Id == taskId);
            if (effort == null)
                throw new ArgumentException(string.Format("can not fined task by id={0}", taskId));

            //получаем идентификатор типа задачи 
            var taskTypeId = effort.Type.Id;

            //получаем конфигурационную информацию о типе задачи
            TaskModel taskModel = GetTaskModel(taskTypeId);
            if (taskModel == null)
                throw new ObjectNotFoundException();

            //убеждаемся, что сотрудник является членом группы "менеджер" для этого типа задач. 
            var isManager = System.Threading.Thread.CurrentPrincipal.IsInRole(roleProperty(taskModel));
            //var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            //var isEmployee = userManager.IsInRole(employee.Id, taskModel.Employee);
            if (isManager == false)
                return false;

            return true;
        }
        /// <summary>
        /// метод убеждается, что текущее состояние среды достаточное для выполнения акции
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="taskId"></param>
        /// <param name="isBussy"></param>
        /// <param name="status">
        /// статус в котором система должна прибывать для успешного завершения метода.
        /// 
        /// в качестве параметра используется функция для того чтобы можно было проверить 
        /// состояния с учетом логических операций. 
        /// например система s!=А && !s!=B. или система в состоянии s==А || s==B... </param>
        /// <returns></returns>
        public bool DoesCircumstancesAllow(string taskId, bool isBussy, Func<EWorkEffortStatus, bool> status, string userName = null)
        {
            var currentUserName = userName;
            if (string.IsNullOrWhiteSpace(currentUserName) == true)
                currentUserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            //убеждаемся, что сотрудник существует в базе 
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(_dbContext));
            var employee = userManager.FindByName(currentUserName);
            if (employee == null)
                throw new ArgumentException(string.Format("can not find user by name={0}", currentUserName));

            var assigmnent = _dbContext.Set<WorkEffortPartyAssignment>().SingleOrDefault(a => a.WorkEffort.Id == taskId);

            //убеждаемся, что задача в нужном состоянии, иначе - выходим
            if (status(assigmnent.Status) == false)
                return false;

            //убеждаемся что задача на кого-то назначена, в противном случае не понятно кто ее будет исполнять
            if (assigmnent.AssignedTo == null)
                return false;

            //убеждаемся что задача назначена на текущего пользователя 
            var party = assigmnent.AssignedTo.Party;
            if (party.Id != employee.Id)
                return false;

            //убеждаемся, что сотрудник не занят
            //var party = _dbContext.Set<Party>().SingleOrDefault(t => t.Id == employee.Id);
            if (party.IsBusy == isBussy)
                return false;

            return true;
        }
        /// <summary>
        /// метод возвращает конфигурационную информацию о типе задачи по его идентификатору
        /// </summary>
        /// <param name="taskTypeId">идентификатор типа задачи</param>
        /// <returns> конфигурационная информация о типе задачи</returns>
        public TaskModel GetTaskModel(string taskTypeId)
        {
            #region parameter validation
            if (string.IsNullOrWhiteSpace(taskTypeId) == true)
                throw new ArgumentNullException("taskTypeId");
            #endregion

            return this._model.Tasks.SingleOrDefault(t => t.Id == taskTypeId);
        }
        #endregion
    }
}
