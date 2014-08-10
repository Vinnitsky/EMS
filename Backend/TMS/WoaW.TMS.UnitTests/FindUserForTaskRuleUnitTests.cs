using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WoaW.CMS.Model.Persons;
using WoaW.CMS.Model.Repationships;
using WoaW.NS;
using WoaW.TMS.Tasks.Rules;

namespace WoaW.TMS.Tasks.UnitTests
{
    /// <summary>
    /// этот тест создан для того чтбы протестировать плавило FindUserForTaskRule
    /// </summary>
    /// <seealso cref="FindUserForTaskRule"/>
    [TestClass]
    public class FindUserForTaskRuleUnitTests
    {
        /// <summary>
        /// тестирование правила поиска пользователя
        /// 
        /// начальные условия: 
        /// 1) свободных пользователей 2. 
        /// 2) они умеют выполнять роль1 и роль2, 
        /// 3) для выполнения задачи нужен пользователь умеющий выполнять роль3 - такого пользователя нет
        /// 
        /// в результате система не должна назначить задачу ни одному пользователю
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_2UsersFree_0UserWithRequredRole_SuccesTest()
        {
            //arrage
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role1, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForTaskRule(rm));
            var task = new WorkEffort(new WorkEffortType("type1", "1")) { RequerdRole = new RoleType("1", "Role3") };

            //act
            rm.WorkEfforts.Add(task);

            //assert
            #region Validation
            var a1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == task);
            Assert.IsNull(a1);
            Assert.AreEqual(1, rm.WorkEfforts.Count);
            Assert.AreEqual(0, rm.Assignments.Count);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(0, w2.Tasks.Count);
            #endregion
        }
        /// <summary>
        /// тестирование правила поиска пользователя
        /// 
        /// начальные условия: 
        /// 1) свободных пользователей 2. 
        /// 2) они умеют выполнять сотрудникй = роль1 и роль2, сотрудник2 = роль3 и роль4, 
        /// 3) для выполнения задачи нужен пользователь умеющий выполнять роль1 - это сотрудник1
        /// 
        /// в результате система должна назначить задачу сотрудник1
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_2UsersFree_1UserWithRequredRole_SuccessTest()
        {
            //arrage
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role1, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForTaskRule(rm));
            var task = new WorkEffort(new WorkEffortType("type1", "1")) { RequerdRole = role1 };

            //act
            rm.WorkEfforts.Add(task);

            //assert
            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            var a = rm.Assignments.SingleOrDefault(t => t.WorkEffort == task);
            Assert.IsNotNull(a);
            Assert.AreEqual(w1, a.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a.Status);
            #endregion

        }
        /// <summary>
        /// тестирование правила поиска пользователя
        /// 
        /// начальные условия: 
        /// 1) свободных пользователей 2. 
        /// 2) они умеют выполнять сотрудникй = роль1 и роль2, сотрудник2 = роль1 и роль3, 
        /// 3) для выполнения задачи нужен пользователь умеющий выполнять роль1 - это сотрудник1 и сотрудник2
        /// 
        /// в результате система должна назначить задачу сотрудник1 или сотрудник2
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_2UserFree_2UsersWithRequredRole_SuceesTest()
        {
            //arrage
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role1, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForTaskRule(rm));
            var task = new WorkEffort(new WorkEffortType("type1", "1")) { RequerdRole = role1 };

            //act
            rm.WorkEfforts.Add(task);

            //assert
            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            var a = rm.Assignments.SingleOrDefault(t => t.WorkEffort == task);
            Assert.IsNotNull(a);
            Assert.AreEqual(w1, a.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a.Status);
            #endregion
        }
        /// <summary>
        /// тестирование правила поиска пользователя
        /// 
        /// начальные условия: 
        /// 1) зарегестриовано 2 сотрудника. 
        /// 2) оба сотрудника заняты
        /// 2) они умеют выполнять сотрудникй = роль1 и роль2, сотрудник2 = роль1 и роль3, 
        /// 3) для выполнения задачи нужен пользователь умеющий выполнять роль1 
        /// 
        /// в результате система не должна назначить задачу ни одному сотруднику
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_0UserFree_2UsersWithRequredRole_SuceesTest()
        {
            //arrenge
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1")) { IsBussy = true };
            var w2 = new EmployeeRole(role2, new Person("w2", "2")) { IsBussy = true };
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForTaskRule(rm));

            var task = new WorkEffort(new WorkEffortType("type1", "1")) { RequerdRole = role1 };
            //act
            rm.WorkEfforts.Add(task);

            //assert
            #region validation
            Assert.AreEqual(1, rm.WorkEfforts.Count);
            Assert.AreEqual(0, rm.Assignments.Count);
            #endregion
        }
        /// <summary>
        /// тестирование правила поиска пользователя
        /// 
        /// начальные условия: 
        /// 1) зарегестриовано 2 сотрудника. 
        /// 2) сотрудник1 занят, сотрудник2 свободен
        /// 2) они умеют выполнять сотрудникй = роль1 , сотрудник2 =  роль2, 
        /// 3) для выполнения задачи нужен сотрудник умеющий выполнять роль2 
        /// 
        /// в результате система должна назначить задачу сотруднику2
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_1UserFree_2UsersWithRequredRole_SuceesTest()
        {
            //arrenge
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("1", "Role1");
            var role2 = new RoleType("2", "Role2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1")) { IsBussy = true };
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForTaskRule(rm));

            var task = new WorkEffort(new WorkEffortType("type1", "1")) { RequerdRole = role2 };
            //act
            rm.WorkEfforts.Add(task);

            //assert
            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            var a = rm.Assignments.SingleOrDefault(t => t.WorkEffort == task);
            Assert.IsNotNull(a);
            Assert.AreEqual(w2, a.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a.Status);
            #endregion
        }
    }
}
