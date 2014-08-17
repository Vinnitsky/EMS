using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using EzBpm.Tms.ConfigModel;
using EzBpm.Tms.DAL.EF.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoaW.CMS.Model;
using WoaW.CMS.Model.Repationships;
using WoaW.Ems.Dal.EF;
using WoaW.NS;
using WoaW.TMS.DAL.EF.Facade;
using WoaW.TMS.Model;
using WoaW.TMS.Model.DAL;

namespace WoaW.Tms.DAL.EF.UnitTests
{
    class A
    {
        public int F1 { get; set; }
        public int F2 { get; set; }
    }

    [TestClass]
    public class QueryUnitTest
    {
        DbContext Context { get; set; }
        readonly IDictionary<string, Action> _initActions = new Dictionary<string, Action>();
        public QueryUnitTest()
        {
            Database.SetInitializer(new MyDropCreateDatabaseAlways());

            _initActions["Query_Simple_SuccessTest"] = Query_Simple_SuccessTest_Init;
            _initActions["Query_ActualStartTime_SuccessTest"] = Query_ActualStartTime_SuccessTest_Init;
            _initActions["Query_Status_SuccessTest"] = Query_Status_SuccessTest_Init;
        }

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
        [TestInitialize]
        public void MyTestInitialize()
        {
            try
            {
                Context = new EmsDbContext();

                Action action;
                if (_initActions.TryGetValue(TestContext.TestName, out action))
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
        [TestCleanup]
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

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void Query_Simple_SuccessTest()
        {
            //arrange 
            var taskTypeIds = TestContext.Properties["TaskTypeIds"] as List<string>;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;

            var taskManagerFacade = new TaskManagerFacade(Context, model, new NotificationCenter());

            var queryData = new QueryTaskRequest
            {
                TaskTypeIDs = taskTypeIds,
                PageIndex = 0, 
                PageSize = 50,
                SortField = "Id",
            };
            //act
            var tasks = taskManagerFacade.Query( queryData);

            //assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Items.Length);
        }
        /// <summary>
        /// здесь пользователь входит в нужные группы, а может быть такое что он не будет входить в нужные группы для типа задач
        /// </summary>
        private void Query_Simple_SuccessTest_Init()
        {
            //регестрируем менеджера 
            const string managerRoleName = "role.manager";
            TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            //регестрируем сотрудника 
            const string employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //регестрируем супервизора 
            const string supervisorRoleName = "role.supervisor";
            TestHelper.RegisterUser(Context, TestContext, "user.supervisor", supervisorRoleName);

            //регестрируем левого сотрудника который не войдет ни в одну из групп 
            var employeeRoleName2 = "role.employee2";
            TestHelper.RegisterUser(Context, TestContext, "user.employee2", employeeRoleName2);

            //создаю тип задачи
            var taskType = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeId"] = taskType.Id;

            //получаем парти
            var party = Context.Set<Party>().SingleOrDefault(t => t.Id == employee.Id);
            var employeeRole = new EmployeeRole(new RoleType("role.employee"), party);

            #region task 1

            //создаем задачу
            var effort = new Task(taskType);
            TestContext.Properties["TaskId"] = effort.Id;
            Context.SaveChanges();

            //устанавливаем время 
            effort.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort.Status = EWorkEffortStatus.Assigned;

            //сохраняем в базе задачу
            Context.Set<Task>().Add(effort);
            Context.SaveChanges();

            var assignment = new WorkEffortPartyAssignment(employeeRole, effort);
            //устанавливая время
            assignment.AssignedAt = DateTime.Now;
            //устанавливаю состояние
            assignment.Status = EWorkEffortStatus.Assigned;
            //устанавливаю исполнителя
            effort.AssignedToParty = employee;
            //записываю историю
            var historyRecord = new WorkEffortHistorycalRecord
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
            #endregion

            #region task 2

            //создаем задачу
            var effort2 = new Task(taskType);
            TestContext.Properties["TaskId2"] = effort2.Id;
            Context.SaveChanges();

            //устанавливаем время 
            effort2.CreationTime = DateTime.Now;

            //устанавливаем состояние
            effort2.Status = EWorkEffortStatus.Assigned;

            //сохраняем в базе задачу
            Context.Set<Task>().Add(effort2);
            Context.SaveChanges();

            //создаю WorkEffortPartyAssignment
            var assignment2 = new WorkEffortPartyAssignment(employeeRole, effort2);
            //устанавливая время
            assignment2.AssignedAt = DateTime.Now;
            //устанавливаю состояние
            assignment2.Status = EWorkEffortStatus.Assigned;
            //устанавливаю исполнителя
            effort2.AssignedToParty = employee;
            //записываю историю
            var historyRecord2 = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                ManagerId = employee.UserName,
                EmployeeId = employee.Id,
                TaskId = assignment2.Id,
                Time = DateTime.Now,
                Status = assignment2.Status
            };
            assignment2.AddHistoryRecord(historyRecord);
            //сохраняю в базу WorkEffortPartyAssignment
            Context.Set<WorkEffortPartyAssignment>().Add(assignment2);
            Context.SaveChanges();
            #endregion

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

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(employee.UserName), new string[] { employeeRoleName });
        }

        //[TestMethod]
        //[TestCategory("TMS.EF.Facade")]
        public void Query_ActualStartTime_SuccessTest()
        {
            //arrange 
            var taskTypeIds = TestContext.Properties["TaskTypeIds"] as List<string>;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;
            var subject = TestContext.Properties["Subject"] as string;

            var taskManagerFacade = new TaskManagerFacade(Context, model, new NotificationCenter());

            var queryData = new QueryTaskRequest()
            {
                TaskTypeIDs = taskTypeIds,
                ActualStartTime = new DateTimeRange { From = new DateTime(12, 12, 12) },
            };
            //act
            var tasks = taskManagerFacade.Query( queryData);

            //assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Items.Length);
        }
        /// <summary>
        /// здесь пользователь входит в нужные группы, а может быть такое что он не будет входить в нужные группы для типа задач
        /// </summary>
        private void Query_ActualStartTime_SuccessTest_Init()
        {
            //регестрируем менеджера 
            var managerRoleName = "role.manager";
            TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            //регестрируем сотрудника 
            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //регестрируем супервизора 
            var supervisorRoleName = "role.supervisor";
            var supervisor = TestHelper.RegisterUser(Context, TestContext, "user.supervisor", supervisorRoleName);

            //регестрируем левого сотрудника который не войдет ни в одну из групп 
            var employeeRoleName2 = "role.employee2";
            var employee2 = TestHelper.RegisterUser(Context, TestContext, "user.employee2", employeeRoleName2);


            //создаем задачу которая использует ранее созданю роль для RequerdRole и которую менеджер будует назначать пользователю 
            //создаю тип задачи
            var taskType1 = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType1);
            var taskType2 = new WorkEffortType("TaskType2", "2");
            Context.Set<WorkEffortType>().Add(taskType2);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeIds"] = new List<string>() { taskType1.Id, taskType2.Id };

            //получаем пользователя который назначен выполнять указанную задачу
            var user = (from r in Context.Set<Party>() where r.Id == employee.Id select r).SingleOrDefault();
            Assert.IsNotNull(user);

            //создаем заготовку назначения типа задачи. это сделано потому что с текущей моделью интерфейсов по другому невозможно
            var effort1 = new Task(taskType1){ Subject = "Subject1" };
            var employeeRole = new EmployeeRole(new RoleType("Employee"), user);
            var assignment1 = new WorkEffortPartyAssignment(employeeRole, effort1) ;
            Context.Set<WorkEffortPartyAssignment>().Add(assignment1);
            TestContext.Properties["Subject"] = (assignment1.WorkEffort as Task).Subject;
            //устанавливаем время 
            assignment1.CreatedAt = DateTime.Now;
            assignment1.AssignedAt = DateTime.Now;
            //устанавливаем состояние
            assignment1.Status = EWorkEffortStatus.Assigned;

            //делаем запись в дурнале
            var historyRecord1 = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = employee.Id,
                TaskId = assignment1.Id,
                Time = DateTime.Now,
                Status = assignment1.Status
            };
            assignment1.AddHistoryRecord(historyRecord1);

            var effort2 = new Task(taskType2){ Subject = "Subject2" };
            var assignment2 = new WorkEffortPartyAssignment(employeeRole, effort2) ;
            Context.Set<WorkEffortPartyAssignment>().Add(assignment2);
            //устанавливаем время 
            assignment2.CreatedAt = DateTime.Now;
            assignment2.AssignedAt = DateTime.Now;
            assignment2.AcceptedAt = DateTime.Now;
            //устанавливаем состояние
            assignment2.Status = EWorkEffortStatus.Accepted;

            //делаем запись в дурнале
            var historyRecord2 = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = employee.Id,
                TaskId = assignment1.Id,
                Time = DateTime.Now,
                Status = assignment1.Status
            };
            assignment2.AddHistoryRecord(historyRecord2);

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
                { 
                    new TaskModel()
                    {
                        Id = taskType1.Id,
                        Manager = managerRoleName,
                        Employee = employeeRoleName,
                        Superviser = supervisorRoleName
                    }
                };
            TestContext.Properties["ConfigModel"] = model;

            //сохраняем в базе
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(employee.UserName), new string[] { employeeRoleName });
        }

        //[TestMethod]
        //[TestCategory("TMS.EF.Facade")]
        public void Query_Status_SuccessTest()
        {

            //arrange 
            var taskTypeIds = TestContext.Properties["TaskTypeIds"] as List<string>;
            var model = TestContext.Properties["ConfigModel"] as ConfigModel;
            var subject = TestContext.Properties["Subject"] as string;

            var taskManagerFacade = new TaskManagerFacade(Context, model, new NotificationCenter());

            var queryData = new QueryTaskRequest()
            {
                TaskTypeIDs = taskTypeIds,
                ActualStartTime = new DateTimeRange { From = new DateTime(12, 12, 12) },
                Statuses = new List<EWorkEffortStatus>() { EWorkEffortStatus.Assigned }
            };
            //act
            var tasks = taskManagerFacade.Query(queryData);

            //assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Items.Length);
        }
        private void Query_Status_SuccessTest_Init()
        {
            //регестрируем менеджера 
            var managerRoleName = "role.manager";
            TestHelper.RegisterUser(Context, TestContext, "user.manager", managerRoleName);

            //регестрируем сотрудника 
            var employeeRoleName = "role.employee";
            var employee = TestHelper.RegisterUser(Context, TestContext, "user.employee", employeeRoleName);

            //регестрируем супервизора 
            var supervisorRoleName = "role.supervisor";
            var supervisor = TestHelper.RegisterUser(Context, TestContext, "user.supervisor", supervisorRoleName);

            //регестрируем левого сотрудника который не войдет ни в одну из групп 
            var employeeRoleName2 = "role.employee2";
            var employee2 = TestHelper.RegisterUser(Context, TestContext, "user.employee2", employeeRoleName2);


            //создаем задачу которая использует ранее созданю роль для RequerdRole и которую менеджер будует назначать пользователю 
            //создаю тип задачи
            var taskType1 = new WorkEffortType("TaskType1", "1");
            Context.Set<WorkEffortType>().Add(taskType1);
            var taskType2 = new WorkEffortType("TaskType2", "2");
            Context.Set<WorkEffortType>().Add(taskType2);
            Context.SaveChanges();
            TestContext.Properties["TaskTypeIds"] = new List<string>() { taskType1.Id, taskType2.Id };

            //получаем пользователя который назначен выполнять указанную задачу
            var user = (from r in Context.Set<Party>() where r.Id == employee.Id select r).SingleOrDefault();
            Assert.IsNotNull(user);

            //создаем заготовку назначения типа задачи. это сделано потому что с текущей моделью интерфейсов по другому невозможно
            var effort1 = new Task(taskType1){ Subject = "Subject1" };
            var employeeRole = new EmployeeRole(new RoleType("Employee"), user);
            var assignment1 = new WorkEffortPartyAssignment(employeeRole, effort1) ;
            Context.Set<WorkEffortPartyAssignment>().Add(assignment1);
            TestContext.Properties["Subject"] = (assignment1.WorkEffort as Task).Subject;
            //устанавливаем время 
            assignment1.CreatedAt = DateTime.Now;
            assignment1.AssignedAt = DateTime.Now;
            //устанавливаем состояние
            assignment1.Status = EWorkEffortStatus.Assigned;

            //делаем запись в дурнале
            var historyRecord1 = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = employee.Id,
                TaskId = assignment1.Id,
                Time = DateTime.Now,
                Status = assignment1.Status
            };
            assignment1.AddHistoryRecord(historyRecord1);

            var effort2 = new Task(taskType2){ Subject = "Subject2" };
            var assignment2 = new WorkEffortPartyAssignment(employeeRole, effort2) ;
            Context.Set<WorkEffortPartyAssignment>().Add(assignment2);
            //устанавливаем время 
            assignment2.CreatedAt = DateTime.Now;
            assignment2.AssignedAt = DateTime.Now;
            assignment2.AcceptedAt = DateTime.Now;
            //устанавливаем состояние
            assignment2.Status = EWorkEffortStatus.Accepted;

            //делаем запись в дурнале
            var historyRecord2 = new WorkEffortHistorycalRecord()
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = employee.Id,
                TaskId = assignment1.Id,
                Time = DateTime.Now,
                Status = assignment1.Status
            };
            assignment2.AddHistoryRecord(historyRecord2);

            //создаю модель 
            var model = new ConfigModel();
            model.Tasks = new List<TaskModel>() 
                { 
                    new TaskModel()
                    {
                        Id = taskType1.Id,
                        Manager = managerRoleName,
                        Employee = employeeRoleName,
                        Superviser = supervisorRoleName
                    }
                };
            TestContext.Properties["ConfigModel"] = model;

            //сохраняем в базе
            Context.SaveChanges();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(employee.UserName), new string[] { employeeRoleName });
        }

        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void DateTimeComparison_SuccessTest()
        {
            var d1 = new DateTime(2014, 8, 1);
            var d2 = new DateTime(2014, 8, 2);

            Assert.IsTrue(d1 < d2);
        }
        [TestMethod]
        [TestCategory("TMS.EF.Facade")]
        public void List_SuccessTest()
        {
            List<A> list = new List<A>{
            new A{F1=1, F2=2}, 
            new A{F1=3, F2=4}};

            var r1 = from a in list where a.F1 >= 1 && a.F1 <= 3 select a;

            foreach (var item in r1)
            {
                System.Diagnostics.Debug.WriteLine("F1={0}, F2={1}", item.F1, item.F2);
            }

            var r2 = from a in r1 where a.F1 == 3 select a;
            foreach (var item in r2)
            {
                System.Diagnostics.Debug.WriteLine("F1={0}, F2={1}", item.F1, item.F2);
            }

        }
    }
}
