using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WoaW.CRM.Model.Persons;
using WoaW.CRM.Model.Repationships;
using WoaW.NS;
using WoaW.TMS.Tasks.Rules;

namespace WoaW.TMS.Tasks.UnitTests
{
    //TODO: важно помнить что если нет назначеной роли то работать не будет 
    //вопрос - может это и хорошо ... сделать так что бы работало без роли 
    //не вопрос, но это будет стимулировать проектировщика думать о пролях

    /// <summary>
    /// этот юнит тест создан для того чтобы протестировать работу правила FindUserForSameTaskRule
    /// </summary>
    /// <seealso cref="FindUserForSameTaskRule"/>
    [TestClass]
    public class FindUserForSameTaskRuleUnitTest
    {
        /// <summary>
        /// тестирование правила поиска пользователя FindUserForSameTaskRule при условиях:
        /// 1) зарегестриовано 2 сотрудника. 
        /// 2) сотрудник1 свободен,  сотрудник2 занят
        /// 3) они умеют выполнять сотрудникй = роль1, сотрудник2 = роль1, 
        /// 4) для выполнения задачи нужен сотрудник умеющий выполнять роль1 
        /// 5) задача 3 не связана ни с задачей1, ни с задачей2
        /// 
        /// в результате система должна назначить задачу сотруднику1
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_1UsersFree_0SameTasks_SuccesTest()
        {
            //arrange
            var time = TimeSpan.FromSeconds(2);

            var role1 = new RoleType("Role1", "1");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role1, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));

            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "t1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);
            var t2 = new WorkEffort(new WorkEffortType("type2", "2"), new Task2() { Id = "t2" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t2);

            var a1 = rm.Assignments.SingleOrDefault(t => t.WorkEffort == t1);
            rm.CloseTask(a1);

            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Closed, a1.Status);
            Assert.AreEqual(false, a1.AssignedTo.IsBussy);//w1
            Assert.AreEqual(t1, a1.WorkEffort);
            Assert.AreEqual(w1, a1.AssignedTo);
            #endregion

            var t3 = new WorkEffort(new WorkEffortType("type31", "3"), new Task3() { Id = "t3" }) { RequerdRole = role1 };

            //act
            rm.WorkEfforts.Add(t3);

            //assert
            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(3, rm.Assignments.Count);
            var a2 = rm.Assignments.SingleOrDefault(t => t.WorkEffort == t3);
            Assert.IsNotNull(a2);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a2.Status);
            Assert.AreEqual(true, a2.AssignedTo.IsBussy);//w1
            Assert.AreEqual(w1, a2.AssignedTo);
            #endregion
        }
        /// <summary>
        /// тестирование правила поиска пользователя FindUserForSameTaskRule при условиях:
        /// 1) зарегестриовано 2 сотрудника. 
        /// 2) сотрудник1 свободен,  сотрудник2 занят
        /// 3) они умеют выполнять сотрудникй = роль1, сотрудник2 = роль1, 
        /// 4) для выполнения задачи нужен сотрудник умеющий выполнять роль1 
        /// 5) задача 3 связана по типу с задачей 1
        /// 
        /// в результате система должна назначить задачу сотруднику1
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_1UsersFree_1SameTasks_SuccesTest()
        {
            //arrange
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role1, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));

            var t1 = new WorkEffort(new WorkEffortType("1", "type1"), new Task1() { Id = "t1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);
            var t2 = new WorkEffort(new WorkEffortType("2", "type2"), new Task2() { Id = "t2" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t2);

            var a1 = rm.Assignments.SingleOrDefault(t => t.WorkEffort == t1);
            rm.CloseTask(a1);

            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Closed, a1.Status);
            Assert.AreEqual(false, a1.AssignedTo.IsBussy);//w1
            Assert.AreEqual(t1, a1.WorkEffort);
            Assert.AreEqual(w1, a1.AssignedTo);
            #endregion

            var t3 = new WorkEffort(new WorkEffortType("type3", "3"), new Task3() { Id = "t3" }) { RequerdRole = role1 };
            t3.WorkEffortAssociations.Add(new WorkEffortAssociationByType(typeof(Task1)));

            //act
            rm.WorkEfforts.Add(t3);

            //assert
            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(3, rm.Assignments.Count);
            var a2 = rm.Assignments.SingleOrDefault(t => t.WorkEffort == t3);
            Assert.IsNotNull(a2);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a2.Status);
            Assert.AreEqual(true, a2.AssignedTo.IsBussy);//w1
            Assert.AreEqual(w1, a2.AssignedTo);
            #endregion
        }
        /// <summary>
        /// тестирование правила поиска пользователя FindUserForSameTaskRule при условиях:
        /// 1) зарегестриовано 2 сотрудника. 
        /// 2) сотрудник1 свободен,  сотрудник2 занят
        /// 3) они умеют выполнять сотрудникй = роль1, сотрудник2 = роль1, 
        /// 4) для выполнения задачи нужен сотрудник умеющий выполнять роль1 
        /// 5) задача 3 связана по типу с задачей1 и задачей2
        /// 
        /// в результате система должна назначить задачу сотруднику1
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_1UsersFree_2SameTasks_SuccesTest()
        {
            //arrange
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role1, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));

            var t1 = new WorkEffort(new WorkEffortType("1", "type1"), new Task1() { Id = "t1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);
            var t2 = new WorkEffort(new WorkEffortType("2", "type2"), new Task2() { Id = "t2" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t2);

            var a1 = rm.Assignments.SingleOrDefault(t => t.WorkEffort == t1);
            rm.CloseTask(a1);

            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Closed, a1.Status);
            Assert.AreEqual(false, a1.AssignedTo.IsBussy);//w1
            Assert.AreEqual(t1, a1.WorkEffort);
            Assert.AreEqual(w1, a1.AssignedTo);
            #endregion

            var t3 = new WorkEffort(new WorkEffortType("type3", "3"), new Task3() { Id = "t3" }) { RequerdRole = role1 };
            t3.WorkEffortAssociations.Add(new WorkEffortAssociationByType(typeof(Task1)));
            t3.WorkEffortAssociations.Add(new WorkEffortAssociationByType(typeof(Task2)));

            //act
            rm.WorkEfforts.Add(t3);

            //assert
            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(3, rm.Assignments.Count);
            var a2 = rm.Assignments.SingleOrDefault(t => t.WorkEffort == t3);
            Assert.IsNotNull(a2);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a2.Status);
            Assert.AreEqual(true, a2.AssignedTo.IsBussy);//w1
            Assert.AreEqual(w1, a2.AssignedTo);
            #endregion
        }
        /// <summary>
        /// тестирование правила поиска пользователя FindUserForSameTaskRule при условиях:
        /// 1) зарегестриовано 2 сотрудника. 
        /// 2) сотрудник1 занят,  сотрудник2 занят
        /// 3) они умеют выполнять сотрудникй = роль1, сотрудник2 = роль1, 
        /// 4) для выполнения задачи нужен сотрудник умеющий выполнять роль1 
        /// 5) задача 3 связана по типу с задачей1 и задачей2
        /// 
        /// в результате система не должна назначить задачу ни одному сотруднику
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.UserRules")]
        public void FindUser_0UsersFree_2SameTasks_SuccesTest()
        {
            //arrange
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role1, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));

            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "t1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);
            var t2 = new WorkEffort(new WorkEffortType("type2", "2"), new Task2() { Id = "t2" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t2);

            #region validation
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            #endregion

            var t3 = new WorkEffort(new WorkEffortType("type3", "3"), new Task3() { Id = "t3" }) { RequerdRole = role1 };
            t3.WorkEffortAssociations.Add(new WorkEffortAssociationByType(typeof(Task1)));
            t3.WorkEffortAssociations.Add(new WorkEffortAssociationByType(typeof(Task2)));

            //act
            rm.WorkEfforts.Add(t3);

            //assert
            #region validation
            Assert.AreEqual(1, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            var a2 = rm.Assignments.SingleOrDefault(t => t.WorkEffort == t3);
            Assert.IsNull(a2);
            #endregion
        }
    }
}
