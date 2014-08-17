using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Security.Principal;
using EzBpm.Tms.ConfigModel;
using EzBpm.Tms.DAL.EF.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoaW.Ems.Dal.EF;
using WoaW.TMS.DAL.EF.Facade;
using WoaW.TMS.Model;

namespace WoaW.Tms.DAL.EF.UnitTests
{
    [TestClass]
    public class ActionAvailabilityValidatorUnitTests
    {
        DbContext Context { get; set; }
        readonly IDictionary<string, Action> _initActions = new Dictionary<string, Action>();
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
        public ActionAvailabilityValidatorUnitTests()
        {
            Database.SetInitializer(new MyDropCreateDatabaseAlways());

            _initActions["HasPermissions_SucessTest"] = HasPermissions_SucessTest_Init;
            _initActions["HasPermissions_EmptyGroup_FailTest"] = HasPermissions_EmptyGroup_FailTest_Init;
            _initActions["HasPermissions_WrongGroup_FailTest"] = HasPermissions_WrongGroup_FailTest_Init;
        }


        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void HasPermissions_SucessTest()
        {
            //arrange
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act 
            var has = validator.HasPermissions(taskId, t => t.Manager);

            //assert
            Assert.AreEqual(true, has);
        }
        private void HasPermissions_SucessTest_Init()
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
            var effort = new WoaW.TMS.Model.DAL.Task(taskType);
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
            Context.Set<WoaW.TMS.Model.DAL.Task>().Add(effort);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { managerRoleName });
        }
        
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void HasPermissions_EmptyGroup_FailTest()
        {
            //arrange
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act 
            var has = validator.HasPermissions(taskId, t => t.Superviser);

            //assert
            Assert.AreEqual(false, has);
        }
        private void HasPermissions_EmptyGroup_FailTest_Init()
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
            var effort = new WoaW.TMS.Model.DAL.Task(taskType);
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
            Context.Set<WoaW.TMS.Model.DAL.Task>().Add(effort);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { managerRoleName });
        }
        
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void HasPermissions_WrongGroup_FailTest()
        {
            //arrange
            var employeeId = TestContext.Properties["user.employee"] as string;
            var taskId = TestContext.Properties["TaskId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act 
            var has = validator.HasPermissions(taskId, t => t.Superviser);

            //assert
            Assert.AreEqual(false, has);
        }
        private void HasPermissions_WrongGroup_FailTest_Init()
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
            var effort = new WoaW.TMS.Model.DAL.Task(taskType);
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
            Context.Set<WoaW.TMS.Model.DAL.Task>().Add(effort);
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { managerRoleName });
        }
    }
}
