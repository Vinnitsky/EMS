using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using EzBpm.Tms.ConfigModel;
using EzBpm.Tms.DAL.EF.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoaW.Ems.Dal.EF;
using WoaW.NS;
using WoaW.TMS.DAL.EF.Facade;
using WoaW.TMS.Model;
using WoaW.TMS.Model.DAL;

namespace WoaW.Tms.DAL.EF.UnitTests
{
    [TestClass]
    public class FacadeCreateUnitTests
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
        public FacadeCreateUnitTests()
        {
            Database.SetInitializer<EmsDbContext>(new MyDropCreateDatabaseAlways());

            _initActions["CreateTask_SuccessTest"] = CreateTask_SuccessTest_Init;
            _initActions["CanCreateTask_UserOk_TaskIdEmpty_SuccessTest"] = CanCreateTask_UserOk_TaskIdEmpty_SuccessTest_Init;
            _initActions["CanCreateTask_UserOk_TaskNotExist_FailTest"] = CanCreateTask_UserOk_TaskNotExist_FailTest_Init;

            _initActions["WhichTasksUserCanCreate_SuccessTest"] = WhichTasksUserCanCreate_SuccessTest_Init;
        }

        /// <summary>
        /// начальные условия 
        /// 1 - пользователь существует
        /// 2 - пользоваель в роли менеджера
        /// 3 - идентификатор типа задачи - пустой
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanCreateTask_UserOk_TaskIdEmpty_SuccessTest()
        {
            var taskTypeId = TestContext.Properties["TaskTypeId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            var task = validator.CanCreate(taskTypeId);
        }
        private void CanCreateTask_UserOk_TaskIdEmpty_SuccessTest_Init()
        {
            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();


            //регестрируем менеджера 
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
            { 
                new TaskModel()
                {
                    Id = taskType.Id,
                    Manager = managerRoleName
                }
            };
            TestContext.Properties["TaskTypeId"] = null;
            TestContext.Properties["ConfigModel"] = model;

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("user.manager"), new string[0]);
        }

        /// <summary>
        /// начальные условия 
        /// 1 - идентификатор типа задачи - нормальный 
        /// 2 - задачи с указанным идентификатором не существует в базе
        /// 2 - идентификато протзователя нормальный
        /// 3 - пользователь существует в базе
        /// 4 - пользователь выполняет роль менеджера
        /// 
        /// пользователя нет в группе менеджеров
        /// в базе нет такой задачи
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        [ExpectedException(typeof(ArgumentException))]
        public void CanCreateTask_UserOk_TaskNotExist_FailTest()
        {
            var taskTypeId = TestContext.Properties["TaskTypeId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var validator = new ActionAvailabilityValidator(Context, model);

            //act
            var task = validator.CanCreate(taskTypeId);
        }
        private void CanCreateTask_UserOk_TaskNotExist_FailTest_Init()
        {
            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();

            //регестрируем менеджера 
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);


            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
            { 
                new TaskModel()
                {
                    Id = taskType.Id,
                    Manager = managerRoleName
                }
            };
            TestContext.Properties["TaskTypeId"] = taskType.Id+"1";
            TestContext.Properties["ConfigModel"] = model;

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[0]);

        }

        /// <summary>
        /// тестируем создание задачи. начальные условия: 
        /// 1 - идентификатор задачи нормальный
        /// 2 - тип задачи существует в базе
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void CreateTask_SuccessTest()
        {
            var managerRoleName = TestContext.Properties["role.manager"] as string;
            var managerId = TestContext.Properties["user.manager"] as string;
            var taskTypeId = TestContext.Properties["TaskTypeId"] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            //arrange
            var taskManagerFacade = new TaskManagerFacade(Context, model, new NotificationCenter());

            //act
            var task = taskManagerFacade.Create(taskTypeId);

            //assert
            Assert.IsNotNull(task);
            var task2 = Context.Set<Task>().SingleOrDefault(t => t.Id == task.Id);
            Assert.IsNotNull(task2);
            Assert.IsFalse(string.IsNullOrWhiteSpace(task2.Id));
            Assert.IsNull(task2.AssignedToParty);
            Assert.AreEqual(EWorkEffortStatus.Wait, task2.Status);
        }
        private void CreateTask_SuccessTest_Init()
        {
            //регестрируем менеджера 
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            //регестрируем сотрудника 
            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeId"] = taskType.Id;

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
            { 
                new TaskModel()
                {
                    Id = TestContext.Properties["TaskTypeId"] as string,
                    Manager = managerRoleName
                }
            };
            TestContext.Properties["ConfigModel"] = model;


            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { manager.UserName });
        }

        /// <summary>
        /// не знаю зачем этот метод нужно посмотреть где Антог его использует 
        ///  метод тестирует функцию которая возвращает список TaskModedl для пользователя который может создавать задчи
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void WhichTasksUserCanCreate_SuccessTest()
        {
            var roleName = "role.manager";
            //arrange
            var managerId = TestContext.Properties[roleName] as string;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var taskManagerFacade = new TaskManagerFacade(Context, model, new NotificationCenter());

            //act
            var taskModels = taskManagerFacade.WhichTasksUserCanCreate().ToList();

            //accert
            Assert.IsNotNull(taskModels);
            Assert.AreEqual(1, taskModels.Count);
            Assert.AreEqual(roleName, taskModels[0].Manager);
        }
        private void WhichTasksUserCanCreate_SuccessTest_Init()
        {
            //регестрируем менеджера 
            var managerRoleName = "role.manager";
            var manager = TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            //регестрируем сотрудника 
            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //регестрируем менеджера 
            var managerRoleName2 = "role.manager2";
            TestHelper.RegisterUser(Context, TestContext, "user.manager2", managerRoleName2);

            //регестрируем сотрудника 
            var employeeRoleName2 = "role.employee2";
            var employee2 = TestHelper.RegisterUser(Context, TestContext, "user.employee2", employeeRoleName2);
            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
                { 
                    new TaskModel()
                    {
                        Id = "1",
                        Manager = managerRoleName,
                        Employee = employeeRoleName,
                    },
                    new TaskModel()
                    {
                        Id = "2",
                        Manager = managerRoleName2,
                        Employee = employeeRoleName2,
                    }
                };
            TestContext.Properties["ConfigModel"] = model;

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(manager.UserName), new string[] { manager.UserName });
        }

    }
}
