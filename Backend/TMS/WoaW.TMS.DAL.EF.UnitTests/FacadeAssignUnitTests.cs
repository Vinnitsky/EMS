using System;
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
using WoaW.Ems.Dal.EF;
using WoaW.NS;
using WoaW.TMS.DAL.EF.Facade;
using WoaW.TMS.Model;
using WoaW.TMS.Model.DAL;

namespace WoaW.Tms.DAL.EF.UnitTests
{
    [TestClass]
    public class FacadeAssignUnitTests
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

        public FacadeAssignUnitTests()
        {
            Database.SetInitializer<EmsDbContext>(new MyDropCreateDatabaseAlways());

            _initActions["AssignTask_Menually_SuccessTest"] = AssignTask_Menually_SuccessTest_Init;
            _initActions["CanAssignTask_SuccessTest"] = CanAssignTask_SuccessTest_Init;
            _initActions["CanAssignTask_UserOk_TaskOk_WrongStatus_FailTest"] = CanAssignTask_UserOk_TaskOk_WrongStatus_FailTest_Init;
            _initActions["CanAssignTask_UserBusy_FailTest"] = CanAssignTask_UserBusy_FailTest_Init;
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void AssignTask_Menually_SuccessTest()
        {
            //arrange 
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var taskManagerFacade = new TaskManagerFacade(Context, model, new NotificationCenter());

            //act
            var task = taskManagerFacade.Assign(employeeId, taskId);

            //assert
            Assert.IsNotNull(task);
            Assert.IsNotNull(task.AssignedToParty);
            var assignment = (from a in Context.Set<WorkEffortPartyAssignment>() where a.WorkEffort.Id == task.Id select a).SingleOrDefault();
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(Context));
            var user = userManager.FindById(employeeId).Party;
            Assert.IsNotNull(assignment);
            Assert.IsNotNull(assignment.WorkEffort.CreationTime);
            Assert.IsNotNull(assignment.AssignedAt);
            Assert.IsNotNull(assignment.AssignedTo);
            Assert.IsNotNull(user);
            Assert.AreEqual(false, user.IsBusy);
            Assert.AreEqual(user.Id, (assignment.WorkEffort as Task).AssignedToParty.Id);
            Assert.IsNotNull((assignment.WorkEffort as Task).AssignedToParty);
            Assert.AreEqual(user.Id, assignment.AssignedTo.Party.Id);
            Assert.AreEqual(1, assignment.History.Count);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment.Status);
        }
        private void AssignTask_Menually_SuccessTest_Init()
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

            //устанавливаем что сотрудник свободен
            employee.Party.IsBusy = false;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Wait;

            //сохраняем в базе
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { managerRoleName });
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void CanAssignTask_SuccessTest()
        {
            //arrange 
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            bool canAssign = validator.CanAssign(employeeId, taskId);
            Assert.AreEqual(true, canAssign);
        }
        private void CanAssignTask_SuccessTest_Init()
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

            //устанавливаем что сотрудник свободен
            employee.Party.IsBusy = false;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Wait;

            //сохраняем в базе
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { managerRoleName });
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void CanAssignTask_UserOk_TaskOk_WrongStatus_FailTest()
        {
            //arrange 
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            bool canAssign = validator.CanAssign(employeeId, taskId);
            Assert.AreEqual(false, canAssign);
        }
        private void CanAssignTask_UserOk_TaskOk_WrongStatus_FailTest_Init()
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

            //устанавливаем что сотрудник свободен
            employee.Party.IsBusy = false;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Created;

            //сохраняем в базе
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { managerRoleName });
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void CanAssignTask_UserBusy_FailTest()
        {
            //arrange 
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            bool canAssign = validator.CanAssign(employeeId, taskId);
            Assert.AreEqual(false, canAssign);
        }
        private void CanAssignTask_UserBusy_FailTest_Init()
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

            //устанавливаем что сотрудник занят
            employee.Party.IsBusy = true;

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Wait;

            //сохраняем в базе
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { managerRoleName });
        }
    }
}
