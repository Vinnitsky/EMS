﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using EzBpm.Tms.ConfigModel;
using EzBpm.Tms.DAL.EF.UnitTests;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoaW.CMS.DAL.EF;
using WoaW.CMS.Model;
using WoaW.CMS.Model.Repationships;
using WoaW.Ems.Dal.EF;
using WoaW.NS;
using WoaW.TMS.DAL.EF.Facade;
using WoaW.TMS.Model;
using WoaW.TMS.Model.DAL;

namespace WoaW.Tms.DAL.EF.UnitTests
{
    [TestClass]
    public class FacadeRejectUnitTests
    {
        readonly IDictionary<string, Action> _initActions = new Dictionary<string, Action>();
        DbContext Context { get; set; }
        #region Additional test attributes

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        // Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            try
            {
                Context = new EmsDbContext();

                Action action = null;
                if (_initActions.TryGetValue(TestContext.TestName, out action) == true)
                    action();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            Context.Dispose();

            using (var db = new EmsDbContext())
            {
                if (db.Database.Exists())
                    db.Database.Delete();
            }
        }

        #endregion

        public FacadeRejectUnitTests()
        {
            Database.SetInitializer<EmsDbContext>(new MyDropCreateDatabaseAlways());

            _initActions["RejectTask_SuccessTest"] = RejectTask_SuccessTest_Init;
            _initActions["CanRejectTask_SuccessTest"] = CanRejectTask_SuccessTest_Init;
            _initActions["CanRejectTask_WrongState_FailTest"] = CanRejectTask_WrongState_FailTest_Init;
            _initActions["CanRejectTask_UserBusy_FailTest"] = CanRejectTask_UserBusy_FailTest_Init;
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void RejectTask_SuccessTest()
        {
            //arrange 
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var taskManagerFacade = new TaskManagerFacade(Context, model, new NotificationCenter());

            //act
            var task = taskManagerFacade.Reject(taskId);

            //assert
            Assert.IsNotNull(task);
            Assert.IsNotNull(task.AssignedToParty);
            var assignment = (from a in Context.Set<WorkEffortPartyAssignment>() where a.WorkEffort.Id == task.Id select a).SingleOrDefault();
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(Context));
            var user = userManager.FindById(employeeId).Party;
            Assert.IsNotNull(assignment);
            Assert.IsNotNull(assignment.WorkEffort.CreationTime);
            Assert.IsNotNull(assignment.AssignedAt);
            Assert.IsNotNull(assignment.AcceptedAt);
            Assert.IsNotNull(assignment.RejectedAt);
            Assert.IsNotNull(assignment.AssignedTo);
            Assert.IsNotNull((assignment.WorkEffort as Task).AssignedToParty);
            Assert.IsNotNull(user);
            Assert.AreEqual(false, user.IsBusy);
            Assert.AreEqual(user.Id, assignment.AssignedTo.Party.Id);
            Assert.AreEqual(user.Id, (assignment.WorkEffort as Task).AssignedToParty.Id);
            Assert.AreEqual(2, assignment.History.Count);
            Assert.AreEqual(EWorkEffortStatus.Rejected, assignment.Status);
        }
        private void RejectTask_SuccessTest_Init()
        {
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeId"] = taskType.Id;

            //создаем задачу
            var effort = new Task(taskType);
            TestContext.Properties["TaskId"] = effort.Id;
            Context.SaveChanges();

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
                { 
                    new TaskModel()
                    {
                        Id = taskType.Id,
                        Manager = managerRoleName,
                        Employee = employeeRoleName,
                    }
                };
            TestContext.Properties["ConfigModel"] = model;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Assigned;

            //сохраняем в базе задачу
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            //получаем парти
            var party = Context.Set<Party>().SingleOrDefault(t => t.Id == employee.Id);
            party.IsBusy = false;
            //создаю WorkEffortPartyAssignment
            var employeeRole = new EmployeeRole(new RoleType("role.employee"), party);
            var assignment = new WorkEffortPartyAssignment(employeeRole, effort);
            //устанавливая время
            assignment.AssignedAt = DateTime.Now;
            //устанавливая время
            assignment.AcceptedAt = DateTime.Now;
            //устанавливаю состояние
            assignment.Status = EWorkEffortStatus.Assigned;
            //устанавливаю исполнителя
            effort.AssignedToParty = employee;
            //записываю историю
            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = employee.UserName,
                EmployeeId = employee.Id,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);
            //сохраняю в базу WorkEffortPartyAssignment
            Context.Set<WorkEffortPartyAssignment>().Add(assignment);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(employee.UserName), new string[] { employeeRoleName });
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void CanRejectTask_SuccessTest()
        {
            //arrange 
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            var canAccept = validator.CanReject(taskId);
            Assert.AreEqual(true, canAccept);
        }
        private void CanRejectTask_SuccessTest_Init()
        {
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeId"] = taskType.Id;

            //создаем задачу
            var effort = new Task(taskType);
            TestContext.Properties["TaskId"] = effort.Id;
            Context.SaveChanges();

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
                { 
                    new TaskModel()
                    {
                        Id = taskType.Id,
                        Manager = managerRoleName,
                        Employee = employeeRoleName,
                    }
                };
            TestContext.Properties["ConfigModel"] = model;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Assigned;

            //сохраняем в базе задачу
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            //получаем парти
            var party = Context.Set<Party>().SingleOrDefault(t => t.Id == employee.Id);
            party.IsBusy = false;
            //создаю WorkEffortPartyAssignment
            var employeeRole = new EmployeeRole(new RoleType("role.employee"), party);
            var assignment = new WorkEffortPartyAssignment(employeeRole, effort);
            //устанавливая время
            assignment.AssignedAt = DateTime.Now;
            //устанавливаю состояние
            assignment.Status = EWorkEffortStatus.Assigned;
            //устанавливаю исполнителя
            effort.AssignedToParty = employee;
            //записываю историю
            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = employee.UserName,
                EmployeeId = employee.Id,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);
            //сохраняю в базу WorkEffortPartyAssignment
            Context.Set<WorkEffortPartyAssignment>().Add(assignment);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(employee.UserName), new string[] { employeeRoleName });
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void CanRejectTask_WrongState_FailTest()
        {
            //arrange 
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            var canAccept = validator.CanReject(taskId);
            Assert.AreEqual(false, canAccept);
        }
        private void CanRejectTask_WrongState_FailTest_Init()
        {
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeId"] = taskType.Id;

            //создаем задачу
            var effort = new Task(taskType);
            TestContext.Properties["TaskId"] = effort.Id;
            Context.SaveChanges();

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
                { 
                    new TaskModel()
                    {
                        Id = taskType.Id,
                        Manager = managerRoleName,
                        Employee = employeeRoleName,
                    }
                };
            TestContext.Properties["ConfigModel"] = model;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Paused;

            //сохраняем в базе задачу
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            //получаем парти
            var party = Context.Set<Party>().SingleOrDefault(t => t.Id == employee.Id);
            party.IsBusy = false;
            //создаю WorkEffortPartyAssignment
            var employeeRole = new EmployeeRole(new RoleType("role.employee"), party);
            var assignment = new WorkEffortPartyAssignment(employeeRole, effort);
            //устанавливая время
            assignment.AssignedAt = DateTime.Now;
            //устанавливаю состояние
            assignment.Status = EWorkEffortStatus.Paused;
            //устанавливаю исполнителя
            effort.AssignedToParty = employee;
            //записываю историю
            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = employee.UserName,
                EmployeeId = employee.Id,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);
            //сохраняю в базу WorkEffortPartyAssignment
            Context.Set<WorkEffortPartyAssignment>().Add(assignment);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(employee.UserName), new string[] { employeeRoleName });
        }
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void CanRejectTask_UserBusy_FailTest()
        {
            //arrange 
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            var canAccept = validator.CanReject(taskId);
            Assert.AreEqual(false, canAccept);
        }
        private void CanRejectTask_UserBusy_FailTest_Init()
        {
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeId"] = taskType.Id;

            //создаем задачу
            var effort = new Task(taskType);
            TestContext.Properties["TaskId"] = effort.Id;
            Context.SaveChanges();

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
                { 
                    new TaskModel()
                    {
                        Id = taskType.Id,
                        Manager = managerRoleName,
                        Employee = employeeRoleName,
                    }
                };
            TestContext.Properties["ConfigModel"] = model;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Paused;

            //сохраняем в базе задачу
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            //получаем парти
            var party = Context.Set<Party>().SingleOrDefault(t => t.Id == employee.Id);
            party.IsBusy = true;
            //создаю WorkEffortPartyAssignment
            var employeeRole = new EmployeeRole(new RoleType("role.employee"), party);
            var assignment = new WorkEffortPartyAssignment(employeeRole, effort);
            //устанавливая время
            assignment.AssignedAt = DateTime.Now;
            //устанавливаю состояние
            assignment.Status = EWorkEffortStatus.Paused;
            //устанавливаю исполнителя
            effort.AssignedToParty = employee;
            //записываю историю
            var historyRecord = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = employee.UserName,
                EmployeeId = employee.Id,
                TaskId = assignment.Id,
                Time = DateTime.Now,
                Status = assignment.Status
            };
            assignment.AddHistoryRecord(historyRecord);
            //сохраняю в базу WorkEffortPartyAssignment
            Context.Set<WorkEffortPartyAssignment>().Add(assignment);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(employee.UserName), new string[] { employeeRoleName });
        }
    }
}