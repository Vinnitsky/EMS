using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WoaW.CRM.Model.Persons;
using WoaW.CRM.Model.Repationships;
using WoaW.NS;
using WoaW.TMS.Tasks.Rules;

namespace WoaW.TMS.Tasks.UnitTests
{
    [TestClass]
    public class PublicAPIUnitTests
    {
        /// <summary>
        /// метод проверяет тот случай когда:
        /// 1) есть сотрудник 
        /// 2) он не занят 
        /// 
        /// результат: система должна назначить ему задачу
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.API")]
        public void AssignTaskAtUser_UsingManager_SuccessTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var manager = new EmployeeRole(new RoleType("Manager", "0"), new Person("person", "3"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 }) { InManualMode = true };
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var task = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };

            //act
            var assignment = rm.AssignTask(manager, w1, task);

            //assert
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment.Status);
            Assert.AreEqual(w1, assignment.AssignedTo);
        }
        /// <summary>
        /// метод проверяет тот случай когда:
        /// 1) есть один сотрудник
        /// 2) сотрудник занят
        /// 2) менеджер назначает задачу
        /// 3) зачача никем не испольняется
        /// 
        /// в результате: задачу которую делает сотрудник необходимо поставить в стек и 
        /// назначить новую 
        /// </summary>
        /// <remarks>
        /// TODO: выяснить как быть в ситуации:
        /// если есть разница между назначена и принята, то может возникнуть ситуация когда 
        /// на пользователя назначена задача, но пока не принята ним. и при этом его руководитель
        /// уже назначил ему новую задачу... как быть в этом случае? по сокольку первая задача 
        /// была назначена но не принята, то ставить ее в стек не очень правильно - он ее не выполнял,
        /// но как обслуживать этот случай не понятно ?
        /// </remarks>
        [TestMethod]
        [TestCategory("TMS.API")]
        public void AssignTaskAtUser_1UserBusssy_SuccessTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var manager = new EmployeeRole(new RoleType("Manager", "0"), new Person("person", "3"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var task1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(task1);

            #region validation
            var assignment1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == task1);
            Assert.IsNotNull(assignment1);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, assignment1.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment1.Status);
            #endregion

            var task2 = new WorkEffort(new WorkEffortType("type1", "1"), new Task2() { Id = "2" }) { RequerdRole = role1 };

            //act
            var assignment2 = rm.AssignTask(manager, w1, task2);

            //assert
            #region validation
            Assert.IsNotNull(assignment2);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            Assert.AreEqual(1, w1.Tasks.Count);
            Assert.AreEqual(EWorkEffortStatus.OnHold, assignment1.Status);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment2.Status);
            #endregion
        }
        /// <summary>
        /// метод проверяет тот случай когда:
        /// 1) есть два сотрудника 
        /// 2) оба сотрудника заняты: сотрудник1=задача1, сотрудниу2=задача2
        /// 3) менеджер освобождает сотрудника1 от задачи1
        /// 4) менеджер назначает задачу1 сотруднику2, котороый уже выполняет задачу2 
        /// 
        /// задача 1 назначается сотруднику2, задача2 котрорую сотрудник2 выполнял ставиться в стек
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.API")]
        public void AssignTaskAtUser_2Users_1TaskPerforms_SuccessTest()
        {
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var manager = new EmployeeRole(new RoleType("Manager", "0"), new Person("person", "3"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);

            #region validation
            var assignment1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(assignment1);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, assignment1.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment1.Status);
            #endregion

            var t2 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "2" }) { RequerdRole = role2 };
            rm.WorkEfforts.Add(t2);

            #region validatin
            var assignment2 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t2);
            Assert.IsNotNull(assignment2);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            Assert.AreEqual(0, w2.Tasks.Count);
            Assert.AreEqual(w2, assignment2.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment2.Status);
            #endregion

            rm.RemoveTask(manager, assignment1);
            var assignment3 = rm.AssignTask(manager, w2, t1);

            #region validatin
            Assert.IsNotNull(assignment3);
            Assert.AreEqual(false, w1.IsBussy);
            Assert.AreEqual(w2, assignment3.AssignedTo);
            Assert.AreEqual(t1, assignment3.WorkEffort);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment3.Status);
            Assert.AreEqual(1, w2.Tasks.Count);
            Assert.IsTrue(w2.Tasks.Contains(assignment2));
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(3, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Closed, assignment1.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            #endregion
        }
        /// <summary>
        /// метод проверяет тот случай когда:
        /// 1) есть два сотрудника 
        /// 2) оба сотрудника заняты
        /// 3) менеджер назначает задачу1 сотруднику2, котороый уже выполняет задачу2 
        /// 
        /// система выбрасывает исключение потому что задача1 не освобождена
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.API")]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignTaskAtUser_2Users_2TaskPerforms_FailTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var manager = new EmployeeRole(new RoleType("Manager", "0"), new Person("person", "3"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);

            #region validation
            var assignment1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(assignment1);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, assignment1.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment1.Status);
            #endregion

            var t2 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "2" }) { RequerdRole = role2 };
            rm.WorkEfforts.Add(t2);

            #region validatin
            var assignment2 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t2);
            Assert.IsNotNull(assignment2);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(2, rm.Assignments.Count);
            Assert.AreEqual(0, w2.Tasks.Count);
            Assert.AreEqual(w2, assignment2.AssignedTo);
            Assert.AreEqual(EWorkEffortStatus.Assigned, assignment2.Status);
            #endregion

            //Assert.ThrowsException<ArgumentException>(() =>
            //{
            //act
            var assignment3 = rm.AssignTask(manager, w2, t1);
            //});
        }
        [TestMethod]
        [TestCategory("TMS.API")]
        public void RemoveTask_SuccessTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var manager = new EmployeeRole(new RoleType("Manager", "0"), new Person("person", "3"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);

            #region validation
            var a1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(a1);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a1.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, a1.AssignedTo);
            #endregion

            //act
            rm.RemoveTask(manager, a1);

            //assert
            #region validation
            var a2 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(a2);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Closed, a2.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            #endregion
        }
        /// <summary>
        /// ===============
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.API")]
        public void AcceptTask_SuccessTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);

            #region validation
            var a1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(a1);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a1.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, a1.AssignedTo);
            #endregion

            //act
            rm.AcceptTask(a1);

            //assert
            #region validation
            var a2 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(a2);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Accepted, a2.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, a2.AssignedTo);
            Assert.IsNotNull(a2.AssignedAt);
            Assert.IsNotNull(a2.AcceptedAt);
            #endregion
        }
        [TestMethod]
        [TestCategory("TMS.API")]
        public void RejectTask_SuccessTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);

            #region validation
            var a1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(a1);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Assigned, a1.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, a1.AssignedTo);
            #endregion

            //act
            rm.RejectTask(a1);

            //assert
            #region validation
            var a2 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == t1);
            Assert.IsNotNull(a2);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Rejected, a2.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, a2.AssignedTo);
            Assert.IsNotNull(a2.AssignedAt);
            Assert.IsNull(a2.AcceptedAt);
            #endregion
        }
        /// <summary>
        /// 1) есть свободный пользователь
        /// 2) есть задача которую можно назначить руками
        /// 
        /// в результате будет назначена задача1
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.API")]
        public void AcceptTaskManually_1UsersFree_AllowManualAcceptance_SuceesTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1, AllowManualAcceptance = true };

            //act
            var assignment = rm.AcceptTaskManually(w1, t1);

            //assert
            #region validation
            Assert.IsNotNull(assignment);
            Assert.AreEqual(0, rm.WorkEfforts.Count);
            Assert.AreEqual(1, rm.Assignments.Count);
            Assert.AreEqual(EWorkEffortStatus.Accepted, assignment.Status);
            Assert.AreEqual(0, w1.Tasks.Count);
            Assert.AreEqual(w1, assignment.AssignedTo);
            Assert.IsNotNull(assignment.AssignedAt);
            Assert.IsNotNull(assignment.AcceptedAt);
            Assert.AreEqual(t1, assignment.WorkEffort);
            #endregion
        }
        /// <summary>
        /// нет свободного пользователя
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.API")]
        [ExpectedException(typeof(ArgumentException))]
        public void AcceptTaskManually_0UsersFree_AllowManualAcceptance_FailTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1, AllowManualAcceptance = true };
            rm.WorkEfforts.Add(t1);

            //Assert.ThrowsException<ArgumentException>(() =>
            //{
            //act
            var assignment = rm.AcceptTaskManually(w1, t1);
            //});

        }
        /// <summary>
        /// есть свободный пользователь но нет задачи которую можно назначить руками
        /// </summary>
        [TestMethod]
        [TestCategory("TMS.API")]
        [ExpectedException(typeof(ArgumentException))]
        public void AcceptTaskManually_1UsersFree_DontAllowManualAcceptance_FailTest()
        {
            //arrange
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 });
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "1" }) { RequerdRole = role1 };

            //Assert.ThrowsException<ArgumentException>(() =>
            //{
            //act
            var assignment = rm.AcceptTaskManually(w1, t1);
            //});

        }

    }
}
