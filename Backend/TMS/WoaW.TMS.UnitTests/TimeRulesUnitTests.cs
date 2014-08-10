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
    public class TimeRulesUnitTests
    {
        [TestMethod]
        [TestCategory("TMS.TimeRules")]
        public async System.Threading.Tasks.Task MaxTimeBeforeAssignTask_noAlert_SuccesTest()
        {
            //arrange
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 }) { MaxTimeBeforeAcceptTask = time };
            rm.TimeValidators.Add(new WaitBeforeAssign(im));
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1")) { RequerdRole = role1 };

            //act
            rm.WorkEfforts.Add(t1);

            //asserts
            await System.Threading.Tasks.Task.Delay(time.Add(TimeSpan.FromSeconds(5))).ContinueWith((x) =>
               {
                   var assignment = rm.Assignments.SingleOrDefault(a => a.WorkEffort == t1);
                   Assert.AreEqual(0, rm.WorkEfforts.Count);
                   Assert.AreEqual(1, rm.Assignments.Count);
                   Assert.AreEqual(w1, assignment.AssignedTo);
                   Assert.AreEqual(0, im.Notifications.Count);
               });

            //System.Threading.Thread.Sleep(5000);
            //Assert.AreEqual(1, scheduler.WaitingTasks.Count);
            //Assert.AreEqual(1, scheduler.Alerts.Count);
        }
        [TestMethod]
        [TestCategory("TMS.TimeRules")]
        public async System.Threading.Tasks.Task MaxTimeBeforeAcceptTask_WithAlert_SuccesTest()
        {
            //arrange
            var time = TimeSpan.FromSeconds(2);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 }) { MaxTimeBeforeAcceptTask = time };
            rm.TimeValidators.Add(new WaitBeforeAccept(im));
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "t1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);
            var assignment = rm.Assignments.SingleOrDefault(a => a.WorkEffort == t1);
            //act
            rm.AcceptTask(assignment);

            //asserts
            await System.Threading.Tasks.Task.Delay(time.Add(TimeSpan.FromSeconds(time.Seconds + 2))).ContinueWith((x) =>
               {
                   Assert.AreEqual(EWorkEffortStatus.Accepted, assignment.Status);
                   Assert.AreEqual(0, rm.WorkEfforts.Count);
                   Assert.AreEqual(1, rm.Assignments.Count);
                   Assert.AreEqual(1, im.Notifications.Count);
               });
        }
        [TestMethod]
        [TestCategory("TMS.TimeRules")]
        public async System.Threading.Tasks.Task MaxTimeBeforeUserAcceptTask_NoAlert_SuccesTest()
        {
            //arrange
            var time = TimeSpan.FromSeconds(5);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 }) { MaxTimeBeforeAcceptTask = time };
            rm.TimeValidators.Add(new WaitBeforeAccept(im));
            rm.Rules.Add(new FindUserForSameTaskRule(rm));
            var t1 = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "t1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(t1);
            var assignment = rm.Assignments.SingleOrDefault(a => a.WorkEffort == t1);

            //act
            rm.AcceptTask(assignment);

            //asserts
            await System.Threading.Tasks.Task.Delay(time.Add(TimeSpan.FromSeconds(3))).ContinueWith((x) =>
            {
                Assert.AreEqual(EWorkEffortStatus.Accepted, assignment.Status);
                Assert.AreEqual(0, rm.WorkEfforts.Count);
                Assert.AreEqual(1, rm.Assignments.Count);
                Assert.AreEqual(0, im.Notifications.Count);
            });
        }

        [TestMethod]
        [TestCategory("TMS.TimeRules")]
        public async System.Threading.Tasks.Task MaxTimeWhenUserExecuteTask_NoAlert_SuccesTest()
        {
            var time = TimeSpan.FromSeconds(2);

            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 }) { MaxTimeInExecuteTask = time };
            rm.Rules.Add(new FindUserForTaskRule(rm));
            rm.TimeValidators.Add(new WaitFinishRule(im));
            var task = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "task1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(task);

            var a1 = rm.Assignments.SingleOrDefault(x => x.WorkEffort == task);
            a1.Status = EWorkEffortStatus.Closed;

            var uInRole = rm.Assignments.SingleOrDefault(t => t.WorkEffort == task);
            Assert.AreEqual(w1, uInRole.AssignedTo);

            await System.Threading.Tasks.Task.Delay(time.Add(TimeSpan.FromSeconds(5))).ContinueWith((x) =>
               {
                   Assert.AreEqual(1, rm.Assignments.Count);
                   Assert.AreEqual(0, im.Notifications.Count);
               });
        }
        [TestMethod]
        [TestCategory("TMS.TimeRules")]
        public async System.Threading.Tasks.Task MaxTimeWhenUserExecuteTask_WithAlert_SuccesTest()
        {
            var time = TimeSpan.FromSeconds(5);
            var role1 = new RoleType("Role1", "1");
            var role2 = new RoleType("Role2", "2");
            var w1 = new EmployeeRole(role1, new Person("w1", "1"));
            var w2 = new EmployeeRole(role2, new Person("w2", "2"));
            var im = new NotificationCenter();
            var rm = new ResourceManager(im, new EmployeeRole[] { w1, w2 }) { MaxTimeInExecuteTask = time };

            rm.Rules.Add(new FindUserForTaskRule(rm));
            rm.TimeValidators.Add(new WaitFinishRule(im));

            var task = new WorkEffort(new WorkEffortType("type1", "1"), new Task1() { Id = "task1" }) { RequerdRole = role1 };
            rm.WorkEfforts.Add(task);
            //task.Status = ETaskStatus.Closed;

            var uInRole = rm.Assignments.SingleOrDefault(t => t.WorkEffort == task);
            Assert.AreEqual(w1, uInRole.AssignedTo);

            await System.Threading.Tasks.Task.Delay(time.Add(TimeSpan.FromSeconds(5))).ContinueWith((x) =>
               {
                   Assert.AreEqual(1, rm.Assignments.Count);
                   Assert.AreEqual(1, im.Notifications.Count);
               });
        }
    }
}